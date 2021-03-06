﻿namespace GlobalSTB
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class Form1 : Form
    {
        private string _registerMap = "00,STATUS,Register: 0x00 Status Register Read Only,,,XDR,YDR,ZDR,ZYXDR,XOW,YOW,ZOW,ZYXOW,13\r\n01,OUT_X_MSB,Register: 0x01 OUT_X_MSB Read Only,,,XD6,XD7,XD8,XD9,XD10,XD11,XD12,XD13,13\r\n02,OUT_X_LSB,Register: 0x02 OUT_X_LSB Read Only,,,XD0,XD1,XD2,XD3,XD4,XD5,XD6,XD7,13\r\n03,OUT_Y_MSB,Register: 0x03 OUT_Y_MSB Read Only,,,YD6,YD7,YD8,YD9,YD10,YD11,YD12,YD13,13\r\n04,OUT_Y_LSB,Register: 0x04 OUT_Y_LSB Read Only,,,YD0,YD1,YD2,YD3,YD4,YD5,YD6,YD7,13\r\n05,OUT_Z_MSB,Register: 0x05 OUT_Z_MSB Read Only,,,ZD6,ZD7,ZD8,ZD9,ZD10,ZD11,ZD12,ZD13,13\r\n06,OUT_Z_LSB,Register: 0x06 OUT_Z_LSB Read Only,,,ZD0,ZD1,ZD2,ZD3,ZD4,ZD5,ZD6,ZD7,13\r\n09,F_SETUP,Register: 0x09 FIFO Setup Read/Write,,,F_WMRK0,F_WMRK1,F_WMRK2,F_WMRK3,F_WMRK4,F_WMRK5,F_MODE0,F_MODE1,39\r\n0A,Trig_SOURCE,Register: 0x0A FIFO Triggers Read/Write,,,--,--,Trig_FF_MT,Trig_PULSE,Trig_LNDPRT,Trig_TRANS,--,--,23\r\n0B,SYSMOD,Register: 0x0B System Mode Read Only,,,SYSMOD0,SYSMOD1,FGT_0,FGT_1,FGT_2,FGT_3,FGT_4,FGERR,13\r\n0C,INT_SOURCE,Register: 0x0C Interrupt Source Read Only,,,SRC_DRDY,--,SRC_FF_MT,SRC_PULSE,SRC_LNDPRT,SRC_TRANS,SRC_FIFO,SRC_ASLP,13\r\n0D,WHO_AM_I,Register: 0x0D Who AM I Device ID Read Only,,,ID0,ID1,ID2,ID3,ID4,ID5,ID6,ID7,5\r\n0E,XYZ_DATA_CFG,Register: 0x0E XXY Data Configuration Read/Write,,,FS0,FS1,--,--,HPF_OUT,--,--,--,23\r\n0F,HP_FILTER_CUTOFF,Register: 0x0F High Pass Filter Settings Read/Write,,,SEL0,SEL1,--,--,LPF_EN,HPF_BYP,--,--,23\r\n10,PL_STATUS,Register: 0x10 Portrait/Landscape Status Read Only,,,BAFR0,LAPO0,LAPO1,--,--,--,LO,NEWLP,13\r\n11,PL_CFG,Register: 0x11 Portrait/Landscape Configuration Read/Write ,,,--,--,--,--,--,--,PLEN,DBCNTM,23\r\n12,PL_COUNT,Register: 0x12 Portrait/Landscape Debounce Counter Read/Write ,,,DBNCE0,DBNCE1,DBNCE2,DBNCE3,DBNCE4,DBNCE5,DBNCE6,DBNCE7,39\r\n13,PL_BF_ZCOMP,Register: 0x13 Portrait/Landscape Back/Front,,,ZLOCK0,ZLOCK1,ZLOCK2,--,--,--,BKFR0,BKFR1,23\r\n14,PL_P_L_THS_REG,Register: 0x14 Portrait/Landscape Threshold Read/Write ,,,HYS0,HYS1,HYS2,P_L_THS0,P_L_THS1,P_L_THS2,P_L_THS3,P_L_THS4,23\r\n15,FF_MT_CFG,Register: 0x15 Motion/Freefall Configuration Read/Write ,,,--,--,--,XEFE,YEFE,ZEFE,OAE,ELE,23\r\n16,FF_MT_SRC,Register: 0x16 Motion/Freefall Source Read Only ,,,XHP,XHE,YHP,YHE,ZHP,ZHE,--,EA,23\r\n17,FF_MT_THS,Register: 0x17 Motion/Freefall Threshold Read/Write,,,THS0,THS1,THS2,THS3,THS4,THS5,THS6,DBCNTM,13\r\n18,FF_MT_COUNT,Register: 0x18 Motion/Freefall Debounce Counter Read/Write,,,DBNCE0,DBNCE1,DBNCE2,DBNCE3,DBNCE4,DBNCE5,DBNCE6,DBNCE7,39\r\n1D,TRANSIENT_CFG,Register: 0x1D Transient Configuration Read/Write,,,HPF_BYP,XEFE,YEFE,ZEFE,ELE,--,--,--,23\r\n1E,TRANSIENT_SRC,Register: 0x1E Transient Source Read Only,,,X_TRANS_POL,XTRANSE,Y_TRANS_POL,YTRANSE,Z_TRANS_POL,ZTRANSE,EA,--,13\r\n1F,TRANSIENT_THS,Register: 0x1F Transient Threshold Read/Write,,,THS0,THS1,THS2,THS3,THS4,THS5,THS6,DBCNTM,39\r\n20,TRANSIENT_COUNT,Register: 0x20 Transient Debounce Counter Read/Write,,,DBNCE0,DBNCE1,DBNCE2,DBNCE3,DBNCE4,DBNCE5,DBNCE6,DBNCE7,39\r\n21,PULSE_CFG,Register: 0x21 Pulse Configuration Read/Write,,,XSPEFE,XDPEFE,YSPEFE,YDPEFE,ZSPEFE,ZDPEFE,ELE,DPA,23\r\n22,PULSE_SRC,Register: 0x22 Pulse Source Read Only,,,POL_X,POL_Y,POL_Z,DPE,AxX,AxY,AxZ,EA,13\r\n23,PULSE_THSX,Register: 0x23 Pulse Threshold X Axis Read/Write,,,THSX0,THSX1,THSX2,THSX3,THSX4,THSX5,THSX6,--,39\r\n24,PULSE_THSY,Register: 0x24 Pulse Threshold Y Axis Read/Write,,,THSY0,THSY1,THSY2,THSY3,THSY4,THSY5,THSY6,--,39\r\n25,PULSE_THSZ,Register: 0x25 Pulse Threshold Z Axis Read/Write,,,THSZ0,THSZ1,THSZ2,THSZ3,THSZ4,THSZ5,THSZ6,--,39\r\n26,PULSE_TMLT,Register: 0x26 Pulse Time Limit Read/Write,,,TMLT0,TMLT1,TMLT2,TMLT3,TMLT4,TMLT5,TMLT6,TMLT7,23\r\n27,PULSE_LTCY,Register: 0x27 Pulse Time Latency Read/Write,,,LTCY0,LTCY1,LTCY2,LTCY3,LTCY4,LTCY5,LTCY6,LTCY7,23\r\n28,PULSE_WIND,Register: 0x28 Pulse Second Time Window Read/Write,,,WIND0,WIND1,WIND2,WIND3,WIND4,WIND5,WIND6,WIND7,23\r\n29,ASLP_COUNT,Register: 0x29 AutoSleep Time-out Counter Read/Write,,,D0,D1,D2,D3,D4,D5,D6,D7,23\r\n2A,CTRL_REG1,Register: 0x2A Control 1 Read/Write,,,ACTIVE,F_READ,LNOISE,DR0,DR1,DR2,ASLP_RATE0,ASLP_RATE1,23\r\n2B,CTRL_REG2,Register: 0x2B Control 2 Read/Write,,,MODS0,MODS1,SLPE,SMODS0,SMODS1,--,RST,ST,23\r\n2C,CTRL_REG3,Register: 0x2C Control 3 Read/Write,,,PP_OD,IPOL,--,WAKE_FF_MT,WAKE_PULSE,WAKE_LNDPRT,WAKE_TRANS,FIFO_GATE,23\r\n2D,CTRL_REG4,Register: 0x2D Control 4 Read/Write,,,INT_EN_DRDY,--,INT_EN_FF_MT,INT_EN_PULSE,INT_EN_LNDPRT,INT_EN_TRANS,INT_EN_FIFO,INT_EN_ASLP,23\r\n2E,CTRL_REG5,Register: 0x2E Control 5 Read/Write,,,INT_CFG_DRDY,--,INT_CFG_FF_MT,INT_CFG_PULSE,INT_CFG_LNDPRT,INT_CFG_TRANS,INT_CFG_FIFO,INT_CFG_ASLP,23\r\n2F,OFF_X,Register: 0x2F X-Offset Read/Write,,,D0,D1,D2,D3,D4,D5,D6,D7,23\r\n30,OFF_Y,Register: 0x30 Y-Offset Read/Write,,,D0,D1,D2,D3,D4,D5,D6,D7,23\r\n31,OFF_Z,Register: 0x31 Z-Offset Read/Write,,,D0,D1,D2,D3,D4,D5,D6,D7,23";
        private Button button1;
        private IContainer components = null;
        private FlowLayoutPanel flowLayoutPanel1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabControl TabTool;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegisterView view = new RegisterView(tabPage1.Width, tabPage1.Height) {
                BackColor = Color.SandyBrown
            };
            view.ParseMap(_registerMap);
            tabPage1.Controls.Add(view);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            button1 = new Button();
            TabTool = new TabControl();
            tabPage1 = new TabPage();
            flowLayoutPanel1 = new FlowLayoutPanel();
            tabPage2 = new TabPage();
            TabTool.SuspendLayout();

            base.SuspendLayout();
            button1.Location = new Point(0x5d, 0x31);
            button1.Name = "button1";
            button1.Size = new Size(0x4b, 0x17);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new EventHandler(button1_Click);
            TabTool.Controls.Add(tabPage1);
            TabTool.Controls.Add(tabPage2);
            TabTool.Location = new Point(0x30, 0x5e);
            TabTool.Name = "TabTool";
            TabTool.SelectedIndex = 0;
            TabTool.Size = new Size(0x3c9, 0x1da);
            TabTool.TabIndex = 1;
            tabPage1.Location = new Point(4, 0x16);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(0x3c1, 0x1c0);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            flowLayoutPanel1.BackColor = Color.DarkGray;
            flowLayoutPanel1.Location = new Point(0x13d, 0x13);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(200, 0x35);
            flowLayoutPanel1.TabIndex = 0;
            tabPage2.Location = new Point(4, 0x16);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(0x3a9, 0x188);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x405, 580);
            base.Controls.Add(flowLayoutPanel1);
            base.Controls.Add(TabTool);
            base.Controls.Add(button1);
            base.Name = "Form1";
            Text = "Form1";
            TabTool.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}

