﻿namespace NVMDatalogger
{
    using Freescale.SASD.Communication;
    using NationalInstruments.UI;
    using NationalInstruments.UI.WindowsForms;
    using NVMDatalogger.Properties;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public class Datalogger : Form
    {
        private GroupBox AccelControllerGroup;
        private bool bClosing = false;
        private Button bNVMDownload;
        private Button bNVMErase;
        private Button bNVMWrite;
        private CommClass cc;
        private eCommMode CommMode = eCommMode.Closed;
        private StatusStrip CommStrip;
        private ToolStripDropDownButton CommStripButton;
        private IContainer components = null;
        private object ControllerEventsLock = new object();
        private AccelController ControllerObj;
        private ComboBox ddlDataRate;
        private deviceID DeviceID = deviceID.Unsupported;
        private bool DoReconnect = false;
        private BoardComm dv;
        private ToolStripMenuItem enableLogDataApplicationToolStripMenuItem;
        private bool FirstTimeLoad = true;
        private GroupBox gbDR;
        private GroupBox gbOM;
        private GroupBox groupBox1;
        private GroupBox groupDelay;
        private GroupBox groupOptions;
        private Image ImageGreen;
        private Image ImageRed;
        private Image ImageYellow;
        private bool InitializeController = false;
        private bool IsScopeActive = false;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label lbl2gRange;
        private Label lbl4gRange;
        private Label lbl8gRange;
        private Label lblConfiguration;
        private Label lblData;
        private Label lblMessage;
        private Label lblNVMErased;
        private Label lblSetODR;
        private Led led1;
        private Led led2;
        private Led ledNVMErased;
        private bool LoadingFW = false;
        private ToolStripMenuItem logDataToolStripMenuItem1;
        private MenuStrip menuStrip1;
        private object objectLock = new object();
        private OpenFileDialog openFileDialog1;
        private Panel panelAdvanced;
        private SplitContainer panelGeneral;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private ProgressBar progressNVM;
        private RadioButton r0Sec;
        private RadioButton r15Sec;
        private RadioButton r30Sec;
        private RadioButton r5Sec;
        private RadioButton r60Sec;
        private RadioButton rdo2g;
        private RadioButton rdo4g;
        private RadioButton rdo8g;
        private RadioButton rTethered;
        private RadioButton rUntethered;
        private SaveFileDialog saveFileDialog1;
        private ComboBox stream;
        private System.Windows.Forms.Timer tmrTiltTimer;
        private ToolStripStatusLabel toolStripStatusLabel;
        private ToolTip toolTip1;

        public Datalogger()
        {
            InitializeComponent();
            panelGeneral.SplitterDistance = 350;
            ControllerObj = new AccelController();
            ControllerObj.ControllerEvents += new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
            ControllerObj.InitializeController();
            InitializeForm();
            tmrTiltTimer.Enabled = true;
            tmrTiltTimer.Start();
        }

        private void bNVMDownload_Click(object sender, EventArgs e)
        {
            bNVMErase.Enabled = false;
            bNVMDownload.Text = "Downloading data, please wait...";
            bNVMDownload.Enabled = false;
            ControllerObj.DLDownloadFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 110, null);
            byte[] buffer = ControllerObj.NVMBytesWritten();
            int num = (((buffer[0] << 0x10) + (buffer[1] << 8)) + buffer[2]) + 0x800;
            double num2 = 14400.0;
            int millisecondsTimeout = ((int) Math.Ceiling((double) (((double) num) / num2))) * 10;
            for (int i = 0; i < 100; i++)
            {
                progressNVM.Value = i;
                Thread.Sleep(millisecondsTimeout);
            }
        }

        private void CheckNVMIsErased()
        {
            ControllerObj.NVMIsErasedFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x6f, null);
        }

        private void CommCallback(object o, CommEvent cmd)
        {
        }

        private void CommStripButton_Click(object sender, EventArgs e)
        {
            switch (CommMode)
            {
                case eCommMode.Closed:
                    dv.FindHw("I");
                    break;

                case eCommMode.Bootloader:
                    CommStripButton.Text = "Find HW";
                    dv.Close();
                    break;

                case eCommMode.Running:
                    CommStripButton.Text = "Find HW";
                    dv.Close();
                    break;
            }
        }

        private void ControllerObj_ControllerEvents(ControllerEventType evt, object o)
        {
        }

        private void Datalogger_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!bClosing)
            {
                bClosing = true;
                ControllerObj.DisableData();
                ControllerObj.EndController();
                Reconnect();
            }
        }

        private void DecodeGUIPackets()
        {
            GUIUpdatePacket packet = new GUIUpdatePacket();
            ControllerObj.Dequeue_GuiPacket(ref packet);
            if (packet != null)
            {
                if (packet.TaskID == 110)
                {
                    try
                    {
                        byte[] buffer = (byte[]) packet.PayLoad.Dequeue();
                        if (((buffer.Length - 1) % 8) != 0)
                        {
                            throw new FormatException();
                        }
                        progressNVM.Value = 0;
                        saveFileDialog1.AddExtension = true;
                        saveFileDialog1.OverwritePrompt = true;
                        saveFileDialog1.Filter = "Comma Separated Values (*.csv) | *.csv";
                        if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                        {
                            return;
                        }
                        TextWriter writer = File.CreateText(saveFileDialog1.FileName);
                        writer.WriteLine("File {0} saved on {1:d} at {1:T}", saveFileDialog1.FileName, DateTime.Now);
                        writer.WriteLine("Device,HWID,g-range,ODR,ADC,X0,Y0,Z0,X1,Y1,Z1");
                        byte[] buffer2 = new byte[0x29];
                        buffer2 = ControllerObj.NVMGetConstants();
                        int num = 0;
                        double num2 = ((double) buffer[1]) / 10.0;
                        double y = buffer[4];
                        double num4 = ((double) ((buffer[2] * 0x100) + buffer[3])) / 16.0;
                        int num5 = 0x40;
                        switch ((int)y)
                        {
                            case 8:
                                num5 = 1;
                                break;

                            case 10:
                                num5 = 4;
                                break;

                            case 12:
                                num5 = 0x10;
                                break;

                            case 14:
                                num5 = 0x40;
                                break;
                        }
                        StringBuilder builder = new StringBuilder();
                        string str = "";
                        if (ControllerObj.DeviceID == deviceID.MMA8450Q)
                        {
                            num = (y == 8.0) ? 4 : 0;
                            str = "MMA8450Q";
                        }
                        if (ControllerObj.DeviceID == deviceID.MMA8451Q)
                        {
                            num = (y == 8.0) ? 6 : 0;
                            str = "MMA8451Q";
                        }
                        if (ControllerObj.DeviceID == deviceID.MMA8452Q)
                        {
                            num = (y == 8.0) ? 4 : 0;
                            str = "MMA8452Q";
                        }
                        if (ControllerObj.DeviceID == deviceID.MMA8453Q)
                        {
                            num = (y == 8.0) ? 2 : 0;
                            str = "MMA8453Q";
                        }
                        if (ControllerObj.DeviceID == deviceID.MMA8652FC)
                        {
                            num = (y == 8.0) ? 4 : 0;
                            str = "MMA8652FC";
                        }
                        if (ControllerObj.DeviceID == deviceID.MMA8653FC)
                        {
                            num = (y == 8.0) ? 2 : 0;
                            str = "MMA8653FC";
                        }
                        builder.Append(str);
                        builder.Append(",");
                        builder.AppendFormat("{0:X2}{1:X2}", buffer[5], buffer[6]);
                        builder.Append(",");
                        builder.Append((int) num2);
                        builder.Append(",");
                        builder.Append((int) num4);
                        builder.Append(",");
                        builder.Append((int) y);
                        builder.Append(",");
                        builder.Append((int) (buffer2[0x23] * num5));
                        builder.Append(",");
                        builder.Append((int) (buffer2[0x24] * num5));
                        builder.Append(",");
                        builder.Append((int) (buffer2[0x25] * num5));
                        builder.Append(",");
                        builder.Append((int) (buffer2[0x26] * num5));
                        builder.Append(",");
                        builder.Append((int) (buffer2[0x27] * num5));
                        builder.Append(",");
                        builder.Append((int) (buffer2[40] * num5));
                        writer.WriteLine(builder.ToString());
                        writer.WriteLine("T(s),X-raw,Y-raw,Z-raw,,T(s),X-g,Y-g,Z-g,Period");
                        double num6 = Math.Pow(2.0, y);
                        int num7 = 0;
                        for (int i = 8; i < (buffer.Length - 1); i += 8)
                        {
                            if ((buffer[i + 6] != 0xff) && (buffer[i + 7] != 0xff))
                            {
                                builder = new StringBuilder();
                                int num9 = ((buffer[i] * 0x100) + buffer[i + 1]) >> num;
                                int num10 = ((buffer[i + 2] * 0x100) + buffer[i + 3]) >> num;
                                int num11 = ((buffer[i + 4] * 0x100) + buffer[i + 5]) >> num;
                                int num12 = (buffer[i + 6] * 0x100) + buffer[i + 7];
                                builder.AppendFormat("{0:F4}", ((double) num7) / num4);
                                builder.Append(",");
                                builder.Append(num9);
                                builder.Append(",");
                                builder.Append(num10);
                                builder.Append(",");
                                builder.Append(num11);
                                builder.Append(",");
                                builder.Append(",");
                                double num13 = -num2 + (((2.0 * num2) / num6) * num9);
                                double num14 = -num2 + (((2.0 * num2) / num6) * num10);
                                double num15 = -num2 + (((2.0 * num2) / num6) * num11);
                                builder.AppendFormat("{0:F4}", ((double) num7) / num4);
                                builder.Append(",");
                                builder.AppendFormat("{0:F3}", num13);
                                builder.Append(",");
                                builder.AppendFormat("{0:F3}", num14);
                                builder.Append(",");
                                builder.AppendFormat("{0:F3}", num15);
                                builder.Append(",");
                                builder.Append(num12);
                                writer.WriteLine(builder);
                            }
                            num7++;
                        }
                        writer.Close();
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("NVM data is corrupted!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("There has been an error when trying to open the file for writing.  Please make sure that it is not being used", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    finally
                    {
                        bNVMErase.Enabled = true;
                        bNVMDownload.Text = "Download to Excel";
                        bNVMDownload.Enabled = true;
                    }
                }
                if (packet.TaskID == 0x6f)
                {
                    if ((bool) packet.PayLoad.Dequeue())
                    {
                        ledNVMErased.Value = true;
                        bNVMErase.Enabled = false;
                        bNVMWrite.Enabled = true;
                        bNVMDownload.Enabled = false;
                        lblNVMErased.Text = "NVM is erased";
                    }
                    else
                    {
                        ledNVMErased.Value = false;
                        bNVMErase.Enabled = true;
                        bNVMWrite.Enabled = false;
                        bNVMDownload.Enabled = true;
                        lblNVMErased.Text = "NVM is not erased";
                    }
                }
                if (packet.TaskID == 0x6c)
                {
                    for (int j = 0; j < 100; j++)
                    {
                        progressNVM.Value = j;
                        Thread.Sleep(5);
                    }
                    progressNVM.Value = 0;
                    ledNVMErased.Value = true;
                    lblNVMErased.Text = "NVM is erased";
                    bNVMErase.Enabled = false;
                    bNVMWrite.Enabled = true;
                    bNVMDownload.Enabled = false;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void EndDemo()
        {
            lock (ControllerEventsLock)
            {
                tmrTiltTimer.Stop();
                ControllerObj.ControllerEvents -= new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
                ControllerObj.EndController();
                base.Close();
            }
        }

        ~Datalogger()
        {
            ControllerObj.ControllerEvents -= new AccelController.ControllerEventHandler(ControllerObj_ControllerEvents);
        }

        private void hideConfigPanel(object sender, EventArgs e)
        {
            if (panelGeneral.SplitterDistance < 210)
            {
                panelGeneral.SplitterDistance = 350;
            }
        }

        private void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Datalogger));
            tmrTiltTimer = new System.Windows.Forms.Timer(components);
            menuStrip1 = new MenuStrip();
            logDataToolStripMenuItem1 = new ToolStripMenuItem();
            enableLogDataApplicationToolStripMenuItem = new ToolStripMenuItem();
            CommStrip = new StatusStrip();
            CommStripButton = new ToolStripDropDownButton();
            toolStripStatusLabel = new ToolStripStatusLabel();
            stream = new ComboBox();
            ddlDataRate = new ComboBox();
            lblData = new Label();
            lblSetODR = new Label();
            bNVMWrite = new Button();
            bNVMErase = new Button();
            bNVMDownload = new Button();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            lblMessage = new Label();
            panelGeneral = new SplitContainer();
            groupOptions = new GroupBox();
            led2 = new Led();
            label11 = new Label();
            led1 = new Led();
            label12 = new Label();
            label10 = new Label();
            rUntethered = new RadioButton();
            rTethered = new RadioButton();
            groupBox1 = new GroupBox();
            pictureBox2 = new PictureBox();
            label13 = new Label();
            progressNVM = new ProgressBar();
            ledNVMErased = new Led();
            lblNVMErased = new Label();
            lblConfiguration = new Label();
            panelAdvanced = new Panel();
            AccelControllerGroup = new GroupBox();
            groupDelay = new GroupBox();
            r60Sec = new RadioButton();
            r30Sec = new RadioButton();
            r15Sec = new RadioButton();
            r5Sec = new RadioButton();
            r0Sec = new RadioButton();
            gbDR = new GroupBox();
            lbl4gRange = new Label();
            rdo2g = new RadioButton();
            rdo8g = new RadioButton();
            rdo4g = new RadioButton();
            lbl2gRange = new Label();
            lbl8gRange = new Label();
            gbOM = new GroupBox();
            toolTip1 = new ToolTip(components);
            pictureBox1 = new PictureBox();
            menuStrip1.SuspendLayout();
            CommStrip.SuspendLayout();
            panelGeneral.Panel1.SuspendLayout();
            panelGeneral.Panel2.SuspendLayout();
            panelGeneral.SuspendLayout();
            groupOptions.SuspendLayout();
            ((ISupportInitialize) led2).BeginInit();
            ((ISupportInitialize) led1).BeginInit();
            groupBox1.SuspendLayout();
            ((ISupportInitialize) pictureBox2).BeginInit();
            ((ISupportInitialize) ledNVMErased).BeginInit();
            panelAdvanced.SuspendLayout();
            AccelControllerGroup.SuspendLayout();
            groupDelay.SuspendLayout();
            gbDR.SuspendLayout();
            gbOM.SuspendLayout();
            ((ISupportInitialize) pictureBox1).BeginInit();
            base.SuspendLayout();
            tmrTiltTimer.Interval = 1;
            tmrTiltTimer.Tick += new EventHandler(tmrTiltTimer_Tick);
            menuStrip1.BackColor = SystemColors.ButtonFace;
            menuStrip1.Enabled = false;
            menuStrip1.Font = new Font("Arial Rounded MT Bold", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            menuStrip1.Items.AddRange(new ToolStripItem[] { logDataToolStripMenuItem1 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(0x3c6, 0x18);
            menuStrip1.TabIndex = 0x40;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.Visible = false;
            logDataToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { enableLogDataApplicationToolStripMenuItem });
            logDataToolStripMenuItem1.Name = "logDataToolStripMenuItem1";
            logDataToolStripMenuItem1.Size = new Size(0x4d, 20);
            logDataToolStripMenuItem1.Text = "Log Data";
            enableLogDataApplicationToolStripMenuItem.Name = "enableLogDataApplicationToolStripMenuItem";
            enableLogDataApplicationToolStripMenuItem.Size = new Size(0x120, 0x16);
            enableLogDataApplicationToolStripMenuItem.Text = "Log Data XYZ with Tilt Detection";
            CommStrip.Items.AddRange(new ToolStripItem[] { CommStripButton, toolStripStatusLabel });
            CommStrip.Location = new Point(0, 0x1af);
            CommStrip.Name = "CommStrip";
            CommStrip.Size = new Size(0x342, 0x16);
            CommStrip.TabIndex = 0x4c;
            CommStrip.Text = "statusStrip1";
            CommStrip.MouseEnter += new EventHandler(hideConfigPanel);
            CommStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            CommStripButton.Image = Resources.YellowState;
            CommStripButton.ImageTransparentColor = Color.Magenta;
            CommStripButton.Name = "CommStripButton";
            CommStripButton.ShowDropDownArrow = false;
            CommStripButton.Size = new Size(20, 20);
            CommStripButton.Text = "toolStripDropDownButton1";
            CommStripButton.Click += new EventHandler(CommStripButton_Click);
            toolStripStatusLabel.BackColor = SystemColors.ButtonFace;
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(0xd1, 0x11);
            toolStripStatusLabel.Text = "COM Port Not Connected, Please Connect";
            stream.DropDownStyle = ComboBoxStyle.DropDownList;
            stream.Font = new Font("Verdana", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            stream.FormattingEnabled = true;
            stream.Items.AddRange(new object[] { "8 Bit", "12 Bit" });
            stream.Location = new Point(7, 0x29);
            stream.Name = "stream";
            stream.Size = new Size(0x79, 0x18);
            stream.TabIndex = 0xc6;
            ddlDataRate.DisplayMember = "(none)";
            ddlDataRate.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlDataRate.Font = new Font("Verdana", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            ddlDataRate.FormattingEnabled = true;
            ddlDataRate.Items.AddRange(new object[] { "800 Hz", "400 Hz", "200 Hz", "100 Hz", "50 Hz", "12.5 Hz", "6.25 Hz", "1.563 Hz" });
            ddlDataRate.Location = new Point(0x92, 0x2a);
            ddlDataRate.Name = "ddlDataRate";
            ddlDataRate.Size = new Size(0x62, 0x18);
            ddlDataRate.TabIndex = 0x7d;
            lblData.AutoSize = true;
            lblData.Font = new Font("Verdana", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblData.ForeColor = Color.Black;
            lblData.Location = new Point(6, 0x1a);
            lblData.Name = "lblData";
            lblData.Size = new Size(0x58, 0x10);
            lblData.TabIndex = 0xc7;
            lblData.Text = "Data source";
            lblSetODR.AutoSize = true;
            lblSetODR.BackColor = Color.AntiqueWhite;
            lblSetODR.Font = new Font("Verdana", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSetODR.ForeColor = Color.Black;
            lblSetODR.Location = new Point(0x8f, 0x1a);
            lblSetODR.Name = "lblSetODR";
            lblSetODR.Size = new Size(0x58, 0x10);
            lblSetODR.TabIndex = 0x7e;
            lblSetODR.Text = "Sensor Rate";
            bNVMWrite.BackColor = Color.DimGray;
            bNVMWrite.FlatAppearance.BorderColor = Color.FromArgb(0, 0xc0, 0);
            bNVMWrite.FlatAppearance.BorderSize = 5;
            bNVMWrite.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            bNVMWrite.ForeColor = Color.White;
            bNVMWrite.Location = new Point(0x89, 0x115);
            bNVMWrite.Name = "bNVMWrite";
            bNVMWrite.Size = new Size(0xda, 0x34);
            bNVMWrite.TabIndex = 0xed;
            bNVMWrite.Text = "Start a new Datalog";
            bNVMWrite.UseVisualStyleBackColor = false;
            bNVMWrite.Click += new EventHandler(NVMWrite);
            bNVMWrite.MouseHover += new EventHandler(hideConfigPanel);
            bNVMErase.BackColor = Color.DarkGray;
            bNVMErase.Enabled = false;
            bNVMErase.FlatAppearance.BorderColor = Color.FromArgb(0, 0xc0, 0);
            bNVMErase.FlatAppearance.BorderSize = 5;
            bNVMErase.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            bNVMErase.ForeColor = Color.White;
            bNVMErase.Location = new Point(6, 0x7e);
            bNVMErase.Name = "bNVMErase";
            bNVMErase.Size = new Size(0xda, 0x34);
            bNVMErase.TabIndex = 0xee;
            bNVMErase.Text = "Erase NVM memory";
            bNVMErase.UseVisualStyleBackColor = false;
            bNVMErase.Click += new EventHandler(NVMErase);
            bNVMDownload.BackColor = Color.DarkGray;
            bNVMDownload.FlatAppearance.BorderColor = Color.FromArgb(0, 0xc0, 0);
            bNVMDownload.FlatAppearance.BorderSize = 5;
            bNVMDownload.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            bNVMDownload.ForeColor = Color.White;
            bNVMDownload.Location = new Point(6, 0xbc);
            bNVMDownload.Name = "bNVMDownload";
            bNVMDownload.Size = new Size(0xda, 0x34);
            bNVMDownload.TabIndex = 0xf4;
            bNVMDownload.Text = "Download to Excel";
            bNVMDownload.UseVisualStyleBackColor = false;
            bNVMDownload.Click += new EventHandler(bNVMDownload_Click);
            openFileDialog1.FileName = "openFileDialog1";
            lblMessage.AutoSize = true;
            lblMessage.BackColor = Color.White;
            lblMessage.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMessage.ForeColor = Color.Red;
            lblMessage.Location = new Point(0x102, 0x5c);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(0, 13);
            lblMessage.TabIndex = 0xda;
            lblMessage.Visible = false;
            panelGeneral.IsSplitterFixed = true;
            panelGeneral.Location = new Point(0, 0x3b);
            panelGeneral.Name = "panelGeneral";
            panelGeneral.Orientation = Orientation.Horizontal;
            panelGeneral.Panel1.Controls.Add(groupOptions);
            panelGeneral.Panel1.Controls.Add(groupBox1);
            panelGeneral.Panel1.Controls.Add(bNVMWrite);
            panelGeneral.Panel1.MouseEnter += new EventHandler(hideConfigPanel);
            panelGeneral.Panel2.Controls.Add(lblConfiguration);
            panelGeneral.Panel2.Controls.Add(panelAdvanced);
            panelGeneral.Panel2.MouseEnter += new EventHandler(showConfigPanel);
            panelGeneral.Panel2MinSize = 1;
            panelGeneral.Size = new Size(840, 0x195);
            panelGeneral.SplitterDistance = 0x15d;
            panelGeneral.SplitterWidth = 1;
            panelGeneral.TabIndex = 0xdb;
            groupOptions.BackColor = Color.LightSlateGray;
            groupOptions.Controls.Add(led2);
            groupOptions.Controls.Add(label11);
            groupOptions.Controls.Add(led1);
            groupOptions.Controls.Add(label12);
            groupOptions.Controls.Add(label10);
            groupOptions.Controls.Add(rUntethered);
            groupOptions.Controls.Add(rTethered);
            groupOptions.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupOptions.ForeColor = Color.Black;
            groupOptions.Location = new Point(500, 3);
            groupOptions.Name = "groupOptions";
            groupOptions.Size = new Size(0x14b, 0x146);
            groupOptions.TabIndex = 2;
            groupOptions.TabStop = false;
            groupOptions.Text = "Datalog options";
            groupOptions.MouseHover += new EventHandler(hideConfigPanel);
            led2.LedStyle = LedStyle.Square3D;
            led2.Location = new Point(0x5c, 0xf1);
            led2.Name = "led2";
            led2.OffColor = Color.DarkRed;
            led2.OnColor = Color.Red;
            led2.Size = new Size(20, 20);
            led2.TabIndex = 0xf4;
            led2.Value = true;
            label11.AutoSize = true;
            label11.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label11.ForeColor = Color.Black;
            label11.Location = new Point(0x20, 0x9e);
            label11.Name = "label11";
            label11.Size = new Size(0xce, 60);
            label11.TabIndex = 3;
            label11.Text = "- Board disconnected from PC\r\n- Board needs external power supply\r\n- Programmable delay\r\n- NVM must be erased first";
            label11.MouseEnter += new EventHandler(hideConfigPanel);
            led1.LedStyle = LedStyle.Square3D;
            led1.Location = new Point(0x5c, 0x100);
            led1.Name = "led1";
            led1.Size = new Size(20, 20);
            led1.TabIndex = 0xf3;
            led1.Value = true;
            label12.AutoSize = true;
            label12.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label12.ForeColor = Color.Maroon;
            label12.Location = new Point(0x20, 0xe3);
            label12.Name = "label12";
            label12.Size = new Size(0xc9, 0x4b);
            label12.TabIndex = 4;
            label12.Text = "1. Provide power to board\r\n2. If LED is    , memory is not erased\r\n3. If LED is    , memory is erased\r\n4. Press pushbutton once to begin\r\n5. Remove power to stop datalog";
            label12.MouseEnter += new EventHandler(hideConfigPanel);
            label10.AutoSize = true;
            label10.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label10.ForeColor = Color.Black;
            label10.Location = new Point(0x20, 0x31);
            label10.Name = "label10";
            label10.Size = new Size(0x97, 60);
            label10.TabIndex = 1;
            label10.Text = "- Board connected to PC\r\n- Board powered by USB\r\n- No delay\r\n- NVM must be erased first";
            label10.MouseEnter += new EventHandler(hideConfigPanel);
            rUntethered.AutoSize = true;
            rUntethered.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Underline, GraphicsUnit.Point, 0);
            rUntethered.ForeColor = Color.Black;
            rUntethered.Location = new Point(6, 0x87);
            rUntethered.Name = "rUntethered";
            rUntethered.Size = new Size(0x90, 20);
            rUntethered.TabIndex = 1;
            rUntethered.Text = "Untethered datalog:";
            rUntethered.UseVisualStyleBackColor = true;
            rUntethered.MouseEnter += new EventHandler(hideConfigPanel);
            rUntethered.CheckedChanged += new EventHandler(rUntethered_CheckedChanged);
            rTethered.AutoSize = true;
            rTethered.Checked = true;
            rTethered.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Underline, GraphicsUnit.Point, 0);
            rTethered.ForeColor = Color.Black;
            rTethered.Location = new Point(6, 0x1b);
            rTethered.Name = "rTethered";
            rTethered.Size = new Size(0x85, 20);
            rTethered.TabIndex = 0;
            rTethered.TabStop = true;
            rTethered.Text = "Tethered datalog:";
            rTethered.UseVisualStyleBackColor = true;
            rTethered.MouseEnter += new EventHandler(hideConfigPanel);
            rTethered.CheckedChanged += new EventHandler(rTethered_CheckedChanged);
            groupBox1.Controls.Add(bNVMDownload);
            groupBox1.Controls.Add(pictureBox2);
            groupBox1.Controls.Add(label13);
            groupBox1.Controls.Add(bNVMErase);
            groupBox1.Controls.Add(progressNVM);
            groupBox1.Controls.Add(ledNVMErased);
            groupBox1.Controls.Add(lblNVMErased);
            groupBox1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.ForeColor = Color.Black;
            groupBox1.Location = new Point(11, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(0x1d7, 0xf6);
            groupBox1.TabIndex = 0xf2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Non-Volatile Memory";
            groupBox1.MouseHover += new EventHandler(hideConfigPanel);
            pictureBox2.Image = Resources.flash_memory;
            pictureBox2.Location = new Point(0x10f, 0x31);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(0xa5, 0x5f);
            pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox2.TabIndex = 0xf3;
            pictureBox2.TabStop = false;
            pictureBox2.MouseHover += new EventHandler(hideConfigPanel);
            label13.AutoSize = true;
            label13.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label13.Location = new Point(6, 0x4f);
            label13.Name = "label13";
            label13.Size = new Size(0x3b, 15);
            label13.TabIndex = 0xf2;
            label13.Text = "Progress:";
            progressNVM.Location = new Point(6, 0x61);
            progressNVM.Maximum = 0x63;
            progressNVM.Name = "progressNVM";
            progressNVM.Size = new Size(0xda, 0x17);
            progressNVM.TabIndex = 0xf1;
            ledNVMErased.LedStyle = LedStyle.Round3D;
            ledNVMErased.Location = new Point(6, 0x15);
            ledNVMErased.Name = "ledNVMErased";
            ledNVMErased.OffColor = Color.Red;
            ledNVMErased.OnColor = Color.Green;
            ledNVMErased.Size = new Size(50, 50);
            ledNVMErased.TabIndex = 0xef;
            ledNVMErased.Value = true;
            lblNVMErased.AutoSize = true;
            lblNVMErased.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNVMErased.Location = new Point(0x3e, 0x23);
            lblNVMErased.Name = "lblNVMErased";
            lblNVMErased.Size = new Size(100, 15);
            lblNVMErased.TabIndex = 240;
            lblNVMErased.Text = "NVM is erased";
            lblConfiguration.BackColor = Color.SandyBrown;
            lblConfiguration.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblConfiguration.Location = new Point(0, 1);
            lblConfiguration.Name = "lblConfiguration";
            lblConfiguration.Size = new Size(840, 20);
            lblConfiguration.TabIndex = 0x51;
            lblConfiguration.Text = "Configuration";
            lblConfiguration.TextAlign = ContentAlignment.MiddleCenter;
            lblConfiguration.MouseHover += new EventHandler(showConfigPanel);
            panelAdvanced.BackColor = Color.SandyBrown;
            panelAdvanced.Controls.Add(AccelControllerGroup);
            panelAdvanced.Location = new Point(0, 0x15);
            panelAdvanced.Name = "panelAdvanced";
            panelAdvanced.Size = new Size(840, 0x95);
            panelAdvanced.TabIndex = 80;
            AccelControllerGroup.BackColor = Color.AntiqueWhite;
            AccelControllerGroup.Controls.Add(groupDelay);
            AccelControllerGroup.Controls.Add(gbDR);
            AccelControllerGroup.Controls.Add(gbOM);
            AccelControllerGroup.ForeColor = SystemColors.ControlText;
            AccelControllerGroup.Location = new Point(0, 0);
            AccelControllerGroup.Name = "AccelControllerGroup";
            AccelControllerGroup.Size = new Size(840, 150);
            AccelControllerGroup.TabIndex = 0xc5;
            AccelControllerGroup.TabStop = false;
            groupDelay.Controls.Add(r60Sec);
            groupDelay.Controls.Add(r30Sec);
            groupDelay.Controls.Add(r15Sec);
            groupDelay.Controls.Add(r5Sec);
            groupDelay.Controls.Add(r0Sec);
            groupDelay.Enabled = false;
            groupDelay.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupDelay.ForeColor = Color.SteelBlue;
            groupDelay.Location = new Point(0x161, 9);
            groupDelay.Name = "groupDelay";
            groupDelay.Size = new Size(0xd4, 0x87);
            groupDelay.TabIndex = 0xc5;
            groupDelay.TabStop = false;
            groupDelay.Text = "Delay options";
            r60Sec.AutoSize = true;
            r60Sec.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            r60Sec.ForeColor = Color.Black;
            r60Sec.Location = new Point(0x4c, 0x59);
            r60Sec.Name = "r60Sec";
            r60Sec.Size = new Size(0x3b, 0x13);
            r60Sec.TabIndex = 4;
            r60Sec.Text = "1 min.";
            r60Sec.UseVisualStyleBackColor = true;
            r30Sec.AutoSize = true;
            r30Sec.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            r30Sec.ForeColor = Color.Black;
            r30Sec.Location = new Point(6, 0x59);
            r30Sec.Name = "r30Sec";
            r30Sec.Size = new Size(0x40, 0x13);
            r30Sec.TabIndex = 3;
            r30Sec.Text = "30 sec.";
            r30Sec.UseVisualStyleBackColor = true;
            r15Sec.AutoSize = true;
            r15Sec.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            r15Sec.ForeColor = Color.Black;
            r15Sec.Location = new Point(0x4c, 0x37);
            r15Sec.Name = "r15Sec";
            r15Sec.Size = new Size(0x40, 0x13);
            r15Sec.TabIndex = 2;
            r15Sec.Text = "15 sec.";
            r15Sec.UseVisualStyleBackColor = true;
            r5Sec.AutoSize = true;
            r5Sec.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            r5Sec.ForeColor = Color.Black;
            r5Sec.Location = new Point(6, 0x37);
            r5Sec.Name = "r5Sec";
            r5Sec.Size = new Size(0x39, 0x13);
            r5Sec.TabIndex = 1;
            r5Sec.Text = "5 sec.";
            r5Sec.UseVisualStyleBackColor = true;
            r0Sec.AutoSize = true;
            r0Sec.Checked = true;
            r0Sec.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            r0Sec.ForeColor = Color.Black;
            r0Sec.Location = new Point(6, 0x18);
            r0Sec.Name = "r0Sec";
            r0Sec.Size = new Size(0x49, 0x13);
            r0Sec.TabIndex = 0;
            r0Sec.TabStop = true;
            r0Sec.Text = "No delay";
            r0Sec.UseVisualStyleBackColor = true;
            gbDR.BackColor = Color.AntiqueWhite;
            gbDR.Controls.Add(lbl4gRange);
            gbDR.Controls.Add(rdo2g);
            gbDR.Controls.Add(rdo8g);
            gbDR.Controls.Add(rdo4g);
            gbDR.Controls.Add(lbl2gRange);
            gbDR.Controls.Add(lbl8gRange);
            gbDR.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbDR.ForeColor = Color.SteelBlue;
            gbDR.Location = new Point(0x28e, 9);
            gbDR.Name = "gbDR";
            gbDR.Size = new Size(0xae, 0x87);
            gbDR.TabIndex = 0xc3;
            gbDR.TabStop = false;
            gbDR.Text = "Dynamic Range";
            lbl4gRange.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl4gRange.ForeColor = Color.Black;
            lbl4gRange.Location = new Point(0x35, 0x34);
            lbl4gRange.Name = "lbl4gRange";
            lbl4gRange.Size = new Size(0x67, 0x10);
            lbl4gRange.TabIndex = 0x77;
            lbl4gRange.Text = "512 counts/g";
            lbl4gRange.TextAlign = ContentAlignment.MiddleRight;
            rdo2g.AutoSize = true;
            rdo2g.Checked = true;
            rdo2g.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdo2g.ForeColor = Color.Black;
            rdo2g.Location = new Point(12, 0x18);
            rdo2g.Name = "rdo2g";
            rdo2g.Size = new Size(0x29, 20);
            rdo2g.TabIndex = 0x73;
            rdo2g.TabStop = true;
            rdo2g.Text = "2g";
            rdo2g.UseVisualStyleBackColor = true;
            rdo8g.AutoSize = true;
            rdo8g.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdo8g.ForeColor = Color.Black;
            rdo8g.Location = new Point(12, 0x4c);
            rdo8g.Name = "rdo8g";
            rdo8g.Size = new Size(0x29, 20);
            rdo8g.TabIndex = 0x74;
            rdo8g.Text = "8g";
            rdo8g.UseVisualStyleBackColor = true;
            rdo4g.AutoSize = true;
            rdo4g.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdo4g.ForeColor = Color.Black;
            rdo4g.Location = new Point(12, 50);
            rdo4g.Name = "rdo4g";
            rdo4g.Size = new Size(0x29, 20);
            rdo4g.TabIndex = 0x75;
            rdo4g.Text = "4g";
            rdo4g.UseVisualStyleBackColor = true;
            lbl2gRange.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl2gRange.ForeColor = Color.Black;
            lbl2gRange.Location = new Point(0x35, 0x1a);
            lbl2gRange.Name = "lbl2gRange";
            lbl2gRange.Size = new Size(0x67, 0x10);
            lbl2gRange.TabIndex = 0x76;
            lbl2gRange.Text = "1024 counts/g";
            lbl2gRange.TextAlign = ContentAlignment.MiddleRight;
            lbl8gRange.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl8gRange.ForeColor = Color.Black;
            lbl8gRange.Location = new Point(0x35, 0x4e);
            lbl8gRange.Name = "lbl8gRange";
            lbl8gRange.Size = new Size(0x67, 0x10);
            lbl8gRange.TabIndex = 120;
            lbl8gRange.Text = "256 counts/g";
            lbl8gRange.TextAlign = ContentAlignment.MiddleRight;
            gbOM.BackColor = Color.AntiqueWhite;
            gbOM.Controls.Add(stream);
            gbOM.Controls.Add(ddlDataRate);
            gbOM.Controls.Add(lblData);
            gbOM.Controls.Add(lblSetODR);
            gbOM.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbOM.ForeColor = Color.SteelBlue;
            gbOM.Location = new Point(5, 9);
            gbOM.Name = "gbOM";
            gbOM.Size = new Size(0x103, 0x87);
            gbOM.TabIndex = 0xc4;
            gbOM.TabStop = false;
            gbOM.Text = "Datalogger operation";
            pictureBox1.Image = Resources.background;
            pictureBox1.Location = new Point(-5, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(840, 60);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 220;
            pictureBox1.TabStop = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = Color.LightSlateGray;
            base.ClientSize = new Size(0x342, 0x1c5);
            base.Controls.Add(pictureBox1);
            base.Controls.Add(CommStrip);
            base.Controls.Add(panelGeneral);
            base.Controls.Add(lblMessage);
            base.Controls.Add(menuStrip1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            base.HelpButton = true;
            base.Icon = (Icon) resources.GetObject("$Icon");
            base.MaximizeBox = false;
            base.Name = "Datalogger";
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            Text = "MMA845xQ Datalogger";
            base.MouseEnter += new EventHandler(hideConfigPanel);
            base.FormClosing += new FormClosingEventHandler(Datalogger_FormClosing);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            CommStrip.ResumeLayout(false);
            CommStrip.PerformLayout();
            panelGeneral.Panel1.ResumeLayout(false);
            panelGeneral.Panel2.ResumeLayout(false);
            panelGeneral.ResumeLayout(false);
            groupOptions.ResumeLayout(false);
            groupOptions.PerformLayout();
            ((ISupportInitialize) led2).EndInit();
            ((ISupportInitialize) led1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((ISupportInitialize) pictureBox2).EndInit();
            ((ISupportInitialize) ledNVMErased).EndInit();
            panelAdvanced.ResumeLayout(false);
            AccelControllerGroup.ResumeLayout(false);
            groupDelay.ResumeLayout(false);
            groupDelay.PerformLayout();
            gbDR.ResumeLayout(false);
            gbDR.PerformLayout();
            gbOM.ResumeLayout(false);
            gbOM.PerformLayout();
            ((ISupportInitialize) pictureBox1).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializeForm()
        {
            stream.SelectedIndex = 1;
            ddlDataRate.SelectedIndex = 4;
            if (InitializeController)
            {
                ControllerObj.StartDevice();
                UpdateFormState();
                ddlDataRate.SelectedIndex = 4;
                UpdateFormState();
                rdo2g.Checked = true;
                stream.SelectedIndex = 1;
                UpdateFormState();
                InitializeController = false;
            }
            InitializeStatusBar();
            CommStripButton_Click(this, new EventArgs());
        }

        private void InitializeStatusBar()
        {
            dv = new BoardComm();
            cc = dv;
            ControllerObj.GetCommObject(ref cc);
            dv = (BoardComm) cc;
            dv.CommEvents += new CommEventHandler(CommCallback);
            LoadResource();
        }

        private void label7_Click(object sender, EventArgs e)
        {
        }

        private void LoadResource()
        {
            try
            {
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                ResourceManager manager = new ResourceManager("NVMDatalogger.Properties.Resources", executingAssembly);
                ImageGreen = (Image) manager.GetObject("GreenState");
                ImageYellow = (Image) manager.GetObject("YellowState");
                ImageRed = (Image) manager.GetObject("RedState");
            }
            catch (Exception exception)
            {
                STBLogger.AddEvent(this, STBLogger.EventLevel.Information, "Exception", exception.Message + exception.Source + exception.StackTrace);
                ImageGreen = CommStripButton.Image;
                ImageYellow = CommStripButton.Image;
                ImageRed = CommStripButton.Image;
            }
        }

        private void NVMConfig()
        {
            int[] datapassed = new int[6];
            int num = rdo2g.Checked ? 20 : (rdo4g.Checked ? 40 : 80);
            int num2 = 0;
            if (ControllerObj.DeviceID == deviceID.MMA8450Q)
            {
                if (ddlDataRate.SelectedIndex == 0)
                {
                    num2 = 0x1900;
                }
                else if (ddlDataRate.SelectedIndex == 1)
                {
                    num2 = 0xc80;
                }
                else if (ddlDataRate.SelectedIndex == 2)
                {
                    num2 = 0x640;
                }
                else if (ddlDataRate.SelectedIndex == 3)
                {
                    num2 = 800;
                }
                else if (ddlDataRate.SelectedIndex == 4)
                {
                    num2 = 200;
                }
                else if (ddlDataRate.SelectedIndex == 5)
                {
                    num2 = 0x19;
                }
            }
            else if (ddlDataRate.SelectedIndex == 0)
            {
                num2 = 0x3200;
            }
            else if (ddlDataRate.SelectedIndex == 1)
            {
                num2 = 0x1900;
            }
            else if (ddlDataRate.SelectedIndex == 2)
            {
                num2 = 0xc80;
            }
            else if (ddlDataRate.SelectedIndex == 3)
            {
                num2 = 0x640;
            }
            else if (ddlDataRate.SelectedIndex == 4)
            {
                num2 = 800;
            }
            else if (ddlDataRate.SelectedIndex == 5)
            {
                num2 = 200;
            }
            else if (ddlDataRate.SelectedIndex == 6)
            {
                num2 = 100;
            }
            else if (ddlDataRate.SelectedIndex == 7)
            {
                num2 = 0x19;
            }
            int num3 = 8;
            if (stream.SelectedIndex == 0)
            {
                num3 = 8;
            }
            else if (ControllerObj.DeviceID == deviceID.MMA8451Q)
            {
                num3 = 14;
            }
            else if (((ControllerObj.DeviceID == deviceID.MMA8452Q) || (ControllerObj.DeviceID == deviceID.MMA8450Q)) || (ControllerObj.DeviceID == deviceID.MMA8652FC))
            {
                num3 = 12;
            }
            else if ((ControllerObj.DeviceID == deviceID.MMA8453Q) || (ControllerObj.DeviceID == deviceID.MMA8653FC))
            {
                num3 = 10;
            }
            int num4 = r0Sec.Checked ? 0 : (r5Sec.Checked ? 0x9c4 : (r15Sec.Checked ? 0x1d4c : (r30Sec.Checked ? 0x3a98 : 0x7530)));
            datapassed[0] = num;
            datapassed[1] = num2 / 0x100;
            datapassed[2] = num2 % 0x100;
            datapassed[3] = num3;
            datapassed[4] = num4 / 0x100;
            datapassed[5] = num4 % 0x100;
            ControllerObj.NVMConfigFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x70, datapassed);
        }

        private void NVMErase(object sender, EventArgs e)
        {
            bNVMErase.Enabled = false;
            bNVMDownload.Enabled = false;
            int[] datapassed = new int[] { 0x40 };
            ControllerObj.NVMEraseFlag = true;
            ControllerReqPacket reqName = new ControllerReqPacket();
            ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x6c, datapassed);
        }

        private void NVMWrite(object sender, EventArgs e)
        {
            if (!IsScopeActive)
            {
                IsScopeActive = true;
                bNVMWrite.Text = "Stop current Datalog";
                bNVMErase.Enabled = false;
                bNVMDownload.Enabled = false;
                gbDR.Enabled = false;
                gbOM.Enabled = false;
                groupOptions.Enabled = false;
                NVMConfig();
                if (rTethered.Checked)
                {
                    ControllerObj.DLStartFlag = true;
                    ControllerReqPacket reqName = new ControllerReqPacket();
                    ControllerObj.CreateNewTaskFromGUI(reqName, 0, 0x6d, null);
                }
                else
                {
                    MessageBox.Show("Please run Datalogger from Battery Board.\nWhen done, plug to USB board and press \"Stop Current Datalog\" to refresh the GUI.", "Untethered Datalog", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                IsScopeActive = false;
                bNVMWrite.Text = "Start a new Datalog";
                gbDR.Enabled = true;
                gbOM.Enabled = true;
                groupOptions.Enabled = true;
                CheckNVMIsErased();
            }
        }

        private void Reconnect()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(Application.StartupPath, @"..\MainLauncher.exe"));
            startInfo.Arguments = "";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            Process process = Process.Start(startInfo);
            StreamReader standardOutput = process.StandardOutput;
            process.WaitForExit(1);
            base.Close();
        }

        private void rTethered_CheckedChanged(object sender, EventArgs e)
        {
            groupDelay.Enabled = false;
        }

        private void rUntethered_CheckedChanged(object sender, EventArgs e)
        {
            groupDelay.Enabled = true;
        }

        private void showConfigPanel(object sender, EventArgs e)
        {
            if (panelGeneral.SplitterDistance >= 340)
            {
                panelGeneral.SplitterDistance = 0xcd;
            }
        }

        private void tmrTiltTimer_Tick(object sender, EventArgs e)
        {
            UpdateFormState();
        }

        private void UpdateCommStrip()
        {
            if (DoReconnect)
            {
                DoReconnect = false;
                tmrTiltTimer.Enabled = false;
                Reconnect();
            }
            CommunicationState commState = dv.GetCommState();
            string commStatus = dv.GetCommStatus();
            switch (commState)
            {
                case CommunicationState.HWFind:
                    CommMode = eCommMode.FindingHW;
                    break;

                case CommunicationState.Idle:
                    CommMode = eCommMode.Closed;
                    break;

                case CommunicationState.Ready:
                {
                    CommMode = eCommMode.Running;
                    string hwid = commStatus.Substring(commStatus.IndexOf("HW:") + 3, 4);
                    if (hwid != "3001" &&
						hwid != "3002" &&
						hwid != "3003" &&
						hwid != "3004" &&
						hwid != "3005" &&
						hwid != "3006")
                    {
                        tmrTiltTimer.Enabled = false;
                        if (MessageBox.Show("Hardware not recognized, please connect a valid MMA845xQ device", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand) == DialogResult.Cancel)
                        {
                            base.Close();
                        }
                        dv.Close();
                        CommMode = eCommMode.Closed;
                        CommStripButton_Click(this, new EventArgs());
                        tmrTiltTimer.Enabled = true;
                        return;
                    }
                    if ((commStatus.Substring(commStatus.IndexOf("SW:") + 3, 4) != "d001" && hwid == "3001") ||
						(commStatus.Substring(commStatus.IndexOf("SW:") + 3, 4) != "d002") && (hwid == "3002") ||
						(commStatus.Substring(commStatus.IndexOf("SW:") + 3, 4) != "d003") && (hwid == "3003") ||
						(commStatus.Substring(commStatus.IndexOf("SW:") + 3, 4) != "d004") && (hwid == "3004") ||
						(commStatus.Substring(commStatus.IndexOf("SW:") + 3, 4) != "d005") && (hwid == "3005") ||
						(commStatus.Substring(commStatus.IndexOf("SW:") + 3, 4) != "d006" && hwid == "3006")
						)
                    {
                        CommMode = eCommMode.Bootloader;
                    }
                    if (FirstTimeLoad)
                    {
                        FirstTimeLoad = false;
                        CheckNVMIsErased();
                        if (ControllerObj.DeviceID == deviceID.MMA8450Q)
                        {
                            ddlDataRate.Items.RemoveAt(6);
                            ddlDataRate.Items.RemoveAt(0);
                        }
                    }
                    break;
                }
                default:
                    CommMode = eCommMode.Closed;
                    break;
            }
            if (CommMode == eCommMode.Closed)
            {
                toolStripStatusLabel.Text = commStatus;
            }
            else
            {
                toolStripStatusLabel.Text = commStatus;
            }
            if (ControllerObj.DeviceID != deviceID.Unsupported && ControllerObj.DeviceID != DeviceID)
            {
                DeviceID = ControllerObj.DeviceID;
            }
            if (CommMode == eCommMode.Bootloader)
            {
                panelGeneral.Enabled = false;
                CommStripButton.Image = ImageYellow;
                if (!LoadingFW)
                {
                    Loader loader;
                    LoadingFW = true;
                    if (ControllerObj.DeviceID == deviceID.MMA8450Q)
                        loader = new Loader(dv, "MMA8450Q-DL-FW.s19", "silent");
                    else
                        loader = new Loader(dv, "MMA845xQ-DL-FW.s19", "silent");

                    dv.Close();
                    CommMode = eCommMode.Closed;
                    CommStripButton_Click(this, new EventArgs());
                    return;
                }
            }
            if (CommMode == eCommMode.FindingHW)
            {
                panelGeneral.Enabled = false;
                CommStripButton.Image = ImageYellow;
            }
            else if (CommMode == eCommMode.Running)
            {
                panelGeneral.Enabled = true;
                CommStripButton.Image = ImageGreen;
                if (ControllerObj.DeviceID == deviceID.MMA8450Q)
                {
                    toolStripStatusLabel.Text = commStatus.Replace("ID", "[MMA8450Q] ID");
                }
                else if (ControllerObj.DeviceID == deviceID.MMA8451Q)
                {
                    toolStripStatusLabel.Text = commStatus.Replace("ID", "[MMA8451Q] ID");
                }
                else if (ControllerObj.DeviceID == deviceID.MMA8452Q)
                {
                    toolStripStatusLabel.Text = commStatus.Replace("ID", "[MMA8452Q] ID");
                }
                else if (ControllerObj.DeviceID == deviceID.MMA8453Q)
                {
                    toolStripStatusLabel.Text = commStatus.Replace("ID", "[MMA8453Q] ID");
                }
                else if (ControllerObj.DeviceID == deviceID.MMA8652FC)
                {
                    toolStripStatusLabel.Text = commStatus.Replace("ID", "[MMA8652FC] ID");
                }
                else if (ControllerObj.DeviceID == deviceID.MMA8653FC)
                {
                    toolStripStatusLabel.Text = commStatus.Replace("ID", "[MMA8653FC] ID");
                }
            }
            else if (CommMode == eCommMode.Closed)
            {
                panelGeneral.Enabled = false;
                CommStripButton.Image = ImageRed;
                DoReconnect = true;
            }
            CommStrip.Refresh();
        }

        private void UpdateFormState()
        {
            DecodeGUIPackets();
            UpdateCommStrip();
            if (ControllerObj.DeviceID == deviceID.MMA8451Q)
            {
                if (stream.Items.Count > 1)
                {
                    stream.Items[1] = "14 Bit";
                }
                lbl2gRange.Text = "4096 counts/g";
                lbl4gRange.Text = "2048 counts/g";
                lbl8gRange.Text = "1024 counts/g";
            }
            else if (((ControllerObj.DeviceID == deviceID.MMA8452Q) || (ControllerObj.DeviceID == deviceID.MMA8450Q)) || (ControllerObj.DeviceID == deviceID.MMA8652FC))
            {
                if (stream.Items.Count > 1)
                {
                    stream.Items[1] = "12 Bit";
                }
                lbl2gRange.Text = "1024 counts/g";
                lbl4gRange.Text = "512 counts/g";
                lbl8gRange.Text = "256 counts/g";
            }
            else if ((ControllerObj.DeviceID == deviceID.MMA8453Q) || (ControllerObj.DeviceID == deviceID.MMA8653FC))
            {
                if (stream.Items.Count > 1)
                {
                    stream.Items[1] = "10 Bit";
                }
                lbl2gRange.Text = "256 counts/g";
                lbl4gRange.Text = "128 counts/g";
                lbl8gRange.Text = "64 counts/g";
            }
        }

        private enum eCommMode
        {
            Bootloader = 3,
            Closed = 1,
            FindingHW = 2,
            Running = 4
        }
    }
}

