﻿namespace LCDLabel
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class LcdLabel : Control
    {
        private bool _autoPad;
        private int charh;
        private int charw;
        private int col;
        private int countLines;
        private Color FBackGround = Color.Silver;
        private Color FBorderColor = Color.Black;
        private int FBorderSpace;
        private int FCharSpacing;
        private LCDLabel.DotMatrix FDotMatrix = LCDLabel.DotMatrix.mat5x7;
        private int FHeight;
        private int first_c;
        private int FLineSpacing;
        private int FNoOfChars;
        private LCDLabel.PixelShape FPixelShape = LCDLabel.PixelShape.Square;
        private LCDLabel.PixelSize FPixelSize;
        private int FPixelSpacing;
        private Color FPixHalfColor;
        private int FPixHeight;
        private Color FPixOffColor = Color.Gray;
        private Color FPixOnColor = Color.Black;
        private int FPixWidth;
        private int FTextLines;
        private int FWidth;
        private int last_c;
        private int pix_x;
        private int pix_y;
        private int psx;
        private int psy;

        public LcdLabel()
        {
            DoubleBuffered = true;
            FWidth = 0;
            FHeight = 0;
            FCharSpacing = 2;
            FLineSpacing = 2;
            FPixelSpacing = 1;
            FBorderSpace = 3;
            FTextLines = 1;
            FNoOfChars = 10;
            FBorderColor = Color.Black;
            FBackGround = Color.Silver;
            FPixOnColor = Color.Black;
            FPixOffColor = Color.FromArgb(0xaaaaaa);
            FPixelSize = LCDLabel.PixelSize.pix2x2;
            CalcHalfColor();
            CalcSize();
        }

        public void AppendText(string text)
        {
            if (countLines > FTextLines)
            {
                countLines = col = 0;
            }
            base.Text = text;
            base.Invalidate();
        }

        private void CalcCharSize()
        {
            if (LCDLabel.PixelSize.pixCustom == FPixelSize)
            {
                psx = FPixWidth;
                psy = FPixHeight;
            }
            else
            {
                psx = ((int) FPixelSize) + 1;
                psy = psx;
                FPixWidth = psx;
                FPixHeight = psy;
            }
            switch (FDotMatrix)
            {
                case LCDLabel.DotMatrix.mat5x7:
                case LCDLabel.DotMatrix.Hitachi:
                    pix_x = 5;
                    pix_y = 7;
                    break;

                case LCDLabel.DotMatrix.mat5x8:
                    pix_x = 5;
                    pix_y = 8;
                    break;

                case LCDLabel.DotMatrix.mat7x9:
                    pix_x = 7;
                    pix_y = 9;
                    break;

                case LCDLabel.DotMatrix.mat9x12:
                    pix_x = 9;
                    pix_y = 12;
                    break;

                case LCDLabel.DotMatrix.Hitachi2:
                    pix_x = 5;
                    pix_y = 10;
                    break;

                case LCDLabel.DotMatrix.dos5x7:
                    pix_x = 5;
                    pix_y = 7;
                    break;
            }
            charw = (pix_x * psx) + ((pix_x - 1) * FPixelSpacing);
            charh = (pix_y * psy) + ((pix_y - 1) * FPixelSpacing);
            FNoOfChars = ((base.Width - (2 * FBorderSpace)) + FCharSpacing) / (charw + FCharSpacing);
            FTextLines = ((base.Height - (2 * FBorderSpace)) + FLineSpacing) / (charh + FLineSpacing);
            if (FNoOfChars < 1)
            {
                FNoOfChars = 1;
            }
            if (FTextLines < 1)
            {
                FTextLines = 1;
            }
            base.Width = (((FBorderSpace * 2) + (FCharSpacing * (FNoOfChars - 1))) + (charw * FNoOfChars)) + 2;
            base.Height = (((FBorderSpace * 2) + (FLineSpacing * (FTextLines - 1))) + (charh * FTextLines)) + 2;
            FWidth = base.Width;
            FHeight = base.Height;
        }

        private void CalcHalfColor()
        {
            byte num3 = (byte) (FPixOnColor.B / 2);
            byte num2 = (byte) (FPixOnColor.G / 2);
            byte num = (byte) (FPixOnColor.R / 2);
            byte a = FPixOnColor.A;
            FPixHalfColor = Color.FromArgb(((num3 + (num2 * 0x100)) + (num * 0x10000)) + (a * 0x1000000));
        }

        private void CalcSize()
        {
            if (LCDLabel.PixelSize.pixCustom == FPixelSize)
            {
                psx = FPixWidth;
                psy = FPixHeight;
            }
            else
            {
                psx = ((int) FPixelSize) + 1;
                psy = psx;
                FPixWidth = psx;
                FPixHeight = psy;
            }
            switch (FDotMatrix)
            {
                case LCDLabel.DotMatrix.mat5x7:
                case LCDLabel.DotMatrix.Hitachi:
                    pix_x = 5;
                    pix_y = 7;
                    break;

                case LCDLabel.DotMatrix.mat5x8:
                    pix_x = 5;
                    pix_y = 8;
                    break;

                case LCDLabel.DotMatrix.mat7x9:
                    pix_x = 7;
                    pix_y = 9;
                    break;

                case LCDLabel.DotMatrix.mat9x12:
                    pix_x = 9;
                    pix_y = 12;
                    break;

                case LCDLabel.DotMatrix.Hitachi2:
                    pix_x = 5;
                    pix_y = 10;
                    break;

                case LCDLabel.DotMatrix.dos5x7:
                    pix_x = 5;
                    pix_y = 7;
                    break;
            }
            charw = (pix_x * psx) + ((pix_x - 1) * FPixelSpacing);
            charh = (pix_y * psy) + ((pix_y - 1) * FPixelSpacing);
            base.Width = (((FBorderSpace * 2) + (FCharSpacing * (FNoOfChars - 1))) + (charw * FNoOfChars)) + 2;
            base.Height = (((FBorderSpace * 2) + (FLineSpacing * (FTextLines - 1))) + (charh * FTextLines)) + 2;
            FWidth = base.Width;
            FHeight = base.Height;
        }

        private void DrawCharacters(Graphics graphics)
        {
            if (Text != null)
            {
                string[] strArray = Text.Split(new char[] { '\n' });
                countLines = col = 0;
                int ypos = (FBorderSpace + 1) + (countLines * (charh + FLineSpacing));
                int xpos = (FBorderSpace + 1) + (col * (charw + FCharSpacing));
                for (int i = 0; i < strArray.Length; i++)
                {
                    string str;
                    countLines++;
                    if (AutoPad && (strArray[i].Length < FNoOfChars))
                    {
                        str = strArray[i].PadLeft(FNoOfChars, ' ');
                    }
                    else
                    {
                        str = strArray[i];
                    }
                    col = 1;
                    while (col <= str.Length)
                    {
                        if (col > FNoOfChars)
                        {
                            xpos = FBorderSpace + 1;
                            ypos = (ypos + charh) + FLineSpacing;
                            countLines++;
                        }
                        if (countLines > FTextLines)
                        {
                            break;
                        }
                        int charindex = Convert.ToInt32(str[col - 1]);
                        if (charindex < first_c)
                        {
                            charindex = first_c;
                        }
                        if (charindex > last_c)
                        {
                            charindex = last_c;
                        }
                        DrawMatrix(graphics, xpos, ypos, charindex);
                        xpos = (xpos + charw) + FCharSpacing;
                        col++;
                    }
                    xpos = FBorderSpace + 1;
                    ypos = (ypos + charh) + FLineSpacing;
                }
            }
        }

        private void DrawMatrix(Graphics graphics, int xpos, int ypos, int charindex)
        {
            int x = xpos;
            int y = ypos;
            charindex -= first_c;
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                for (int i = 0; i < pix_y; i++)
                {
                    for (int j = 0; j < pix_x; j++)
                    {
                        Color fPixOffColor = FPixOffColor;
                        switch (FDotMatrix)
                        {
                            case LCDLabel.DotMatrix.mat5x7:
                                if (Matrix.Char5x7[charindex, i, j] != 1)
                                {
                                    break;
                                }
                                fPixOffColor = FPixOnColor;
                                goto Label_020D;

                            case LCDLabel.DotMatrix.mat5x8:
                                if (Matrix.Char5x8[charindex, i, j] != 1)
                                {
                                    goto Label_00BB;
                                }
                                fPixOffColor = FPixOnColor;
                                goto Label_020D;

                            case LCDLabel.DotMatrix.mat7x9:
                                if (Matrix.Char7x9[charindex, i, j] != 1)
                                {
                                    goto Label_01A8;
                                }
                                fPixOffColor = FPixOnColor;
                                goto Label_020D;

                            case LCDLabel.DotMatrix.mat9x12:
                                if (Matrix.Char9x12[charindex, i, j] != 1)
                                {
                                    goto Label_01D6;
                                }
                                fPixOffColor = FPixOnColor;
                                goto Label_020D;

                            case LCDLabel.DotMatrix.Hitachi:
                                if (Matrix.CharHitachi[charindex, i, j] != 1)
                                {
                                    goto Label_00EC;
                                }
                                fPixOffColor = FPixOnColor;
                                goto Label_020D;

                            case LCDLabel.DotMatrix.Hitachi2:
                                if (charindex > 0xc1)
                                {
                                    goto Label_0150;
                                }
                                if (i >= 7)
                                {
                                    goto Label_0146;
                                }
                                if (Matrix.CharHitachi[charindex, i, j] != 1)
                                {
                                    goto Label_013C;
                                }
                                fPixOffColor = FPixOnColor;
                                goto Label_020D;

                            case LCDLabel.DotMatrix.dos5x7:
                                if (Matrix.CharDOS5x7[charindex, i, j] != 1)
                                {
                                    goto Label_0204;
                                }
                                fPixOffColor = FPixOnColor;
                                goto Label_020D;

                            default:
                                goto Label_020D;
                        }
                        fPixOffColor = FPixOffColor;
                        goto Label_020D;
                    Label_00BB:
                        fPixOffColor = FPixOffColor;
                        goto Label_020D;
                    Label_00EC:
                        fPixOffColor = FPixOffColor;
                        goto Label_020D;
                    Label_013C:
                        fPixOffColor = FPixOffColor;
                        goto Label_020D;
                    Label_0146:
                        fPixOffColor = FPixOffColor;
                        goto Label_020D;
                    Label_0150:
                        if (Matrix.CharHitachiExt[charindex, i, j] == 1)
                        {
                            fPixOffColor = FPixOnColor;
                        }
                        else
                        {
                            fPixOffColor = FPixOffColor;
                        }
                        goto Label_020D;
                    Label_01A8:
                        fPixOffColor = FPixOffColor;
                        goto Label_020D;
                    Label_01D6:
                        fPixOffColor = FPixOffColor;
                        goto Label_020D;
                    Label_0204:
                        fPixOffColor = FPixOffColor;
                    Label_020D:
                        brush.Color = fPixOffColor;
                        switch (FPixelShape)
                        {
                            case LCDLabel.PixelShape.Square:
                                graphics.FillRectangle(brush, x, y, psx, psy);
                                goto Label_02E4;

                            case LCDLabel.PixelShape.Round:
                                graphics.FillEllipse(brush, x, y, psx, psy);
                                goto Label_02E4;

                            case LCDLabel.PixelShape.Shaped:
                                if (!(fPixOffColor == FPixOnColor))
                                {
                                    break;
                                }
                                brush.Color = FPixHalfColor;
                                graphics.FillRectangle(brush, x, y, psx, psy);
                                brush.Color = fPixOffColor;
                                graphics.FillEllipse(brush, x, y, psx, psy);
                                goto Label_02E4;

                            default:
                                goto Label_02E4;
                        }
                        brush.Color = fPixOffColor;
                        graphics.FillRectangle(brush, x, y, psx, psy);
                    Label_02E4:
                        x = (x + psx) + FPixelSpacing;
                    }
                    x = xpos;
                    y = (y + psy) + FPixelSpacing;
                }
            }
        }

        private void GetAsciiInterval()
        {
            switch (FDotMatrix)
            {
                case LCDLabel.DotMatrix.mat5x7:
                case LCDLabel.DotMatrix.Hitachi:
                    first_c = 0x20;
                    last_c = 0xdf;
                    break;

                case LCDLabel.DotMatrix.mat5x8:
                    first_c = 0x20;
                    last_c = 0x7e;
                    break;

                case LCDLabel.DotMatrix.mat7x9:
                    first_c = 0x20;
                    last_c = 0x7e;
                    break;

                case LCDLabel.DotMatrix.mat9x12:
                    first_c = 0x20;
                    last_c = 0x7e;
                    break;

                case LCDLabel.DotMatrix.Hitachi2:
                    first_c = 0x20;
                    last_c = 0xdf;
                    break;

                case LCDLabel.DotMatrix.dos5x7:
                    first_c = 0;
                    last_c = 0xff;
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            bool flag = false;
            if (base.Width != FWidth)
            {
                flag = true;
                FWidth = base.Width;
            }
            if (base.Height != FHeight)
            {
                flag = true;
                FHeight = base.Height;
            }
            GetAsciiInterval();
            if (flag)
            {
                CalcCharSize();
            }
            else
            {
                CalcSize();
            }
            using (SolidBrush brush = new SolidBrush(FBackGround))
            {
                using (new Pen(brush))
                {
                    Rectangle rect = new Rectangle(0, 0, base.Width, base.Height);
                    if (base.Visible)
                    {
                        e.Graphics.FillRectangle(brush, rect);
                        if (base.Enabled)
                        {
                            DrawCharacters(e.Graphics);
                        }
                    }
                    else
                    {
                        brush.Color = SystemColors.ButtonFace;
                        e.Graphics.FillRectangle(brush, rect);
                    }
                }
            }
        }

        public bool AutoPad
        {
            get
            {
                return _autoPad;
            }
            set
            {
                _autoPad = value;
            }
        }

        public Color BackGround
        {
            get
            {
                return FBackGround;
            }
            set
            {
                if (value != FBackGround)
                {
                    FBackGround = value;
                    base.Invalidate();
                }
            }
        }

        public Color BorderColor
        {
            get
            {
                return FBorderColor;
            }
            set
            {
                if (value != FBorderColor)
                {
                    FBorderColor = value;
                    base.Invalidate();
                }
            }
        }

        public int BorderSpace
        {
            get
            {
                return FBorderSpace;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Border spacing can't be less than zero");
                }
                if (value != FBorderSpace)
                {
                    FBorderSpace = value;
                    base.Invalidate();
                }
            }
        }

        public int CharSpacing
        {
            get
            {
                return FCharSpacing;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Character spacing can't be less than zero");
                }
                if (value != FCharSpacing)
                {
                    FCharSpacing = value;
                    base.Invalidate();
                }
            }
        }

        public LCDLabel.DotMatrix DotMatrix
        {
            get
            {
                return FDotMatrix;
            }
            set
            {
                FDotMatrix = value;
                base.Invalidate();
            }
        }

        public int LineSpacing
        {
            get
            {
                return FLineSpacing;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Line spacing can't be less than zero");
                }
                if (value != FLineSpacing)
                {
                    FLineSpacing = value;
                    base.Invalidate();
                }
            }
        }

        public int NumberOfCharacters
        {
            get
            {
                return FNoOfChars;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Display needs at least one character");
                }
                if (value != FNoOfChars)
                {
                    FNoOfChars = value;
                    base.Invalidate();
                }
            }
        }

        public int PixelHeight
        {
            get
            {
                return FPixHeight;
            }
            set
            {
                if ((FPixelSize == LCDLabel.PixelSize.pixCustom) && (value != FPixHeight))
                {
                    if (value < 1)
                    {
                        throw new ArgumentException("Display pixel height must be 1 or greater");
                    }
                    FPixHeight = value;
                    base.Invalidate();
                }
            }
        }

        public Color PixelOff
        {
            get
            {
                return FPixOffColor;
            }
            set
            {
                if (value != FPixOffColor)
                {
                    FPixOffColor = value;
                    base.Invalidate();
                }
            }
        }

        public Color PixelOn
        {
            get
            {
                return FPixOnColor;
            }
            set
            {
                if (value != FPixOnColor)
                {
                    FPixOnColor = value;
                    CalcHalfColor();
                    base.Invalidate();
                }
            }
        }

        public LCDLabel.PixelShape PixelShape
        {
            get
            {
                return FPixelShape;
            }
            set
            {
                if (value != FPixelShape)
                {
                    FPixelShape = value;
                    base.Invalidate();
                }
            }
        }

        public LCDLabel.PixelSize PixelSize
        {
            get
            {
                return FPixelSize;
            }
            set
            {
                if (value != FPixelSize)
                {
                    FPixelSize = value;
                    base.Invalidate();
                }
            }
        }

        public int PixelSpacing
        {
            get
            {
                return FPixelSpacing;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Pixel spacing can't be less than zero");
                }
                if (value != FPixelSpacing)
                {
                    FPixelSpacing = value;
                    base.Invalidate();
                }
            }
        }

        public int PixelWidth
        {
            get
            {
                return FPixWidth;
            }
            set
            {
                if ((FPixelSize == LCDLabel.PixelSize.pixCustom) && (value != FPixWidth))
                {
                    if (value < 1)
                    {
                        throw new ArgumentException("Display pixel width must be 1 or greater");
                    }
                    FPixWidth = value;
                    base.Invalidate();
                }
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (Text != value)
                {
                    base.Text = value;
                    countLines = col = 0;
                    base.Invalidate();
                }
            }
        }

        public int TextLines
        {
            get
            {
                return FTextLines;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Display needs at least one line");
                }
                if (value != FTextLines)
                {
                    FTextLines = value;
                    base.Invalidate();
                }
            }
        }
    }
}

