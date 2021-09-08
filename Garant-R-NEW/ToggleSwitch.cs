using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Garant_R_NEW
{
    class ToggleSwitch : Control
    {
        Rectangle rect;
        Rectangle rectToggle;

        int TogglePosX_ON;
        int TogglePosX_OFF;
        public bool Checked { get; set; } = false;
        //public Color BackColorON { get; set; } = Color.Tomato;

        public ToggleSwitch()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;

            Size = new Size(60, 23);

            Font = new Font("Verdana", 16, FontStyle.Regular);
            BackColor = Color.White;

            rect = new Rectangle(1, 1, Width - 3, Height - 3);
            rectToggle = new Rectangle(rect.X, rect.Y, rect.Height, rect.Height);

            TogglePosX_ON = rect.Width - rect.Height; 
            TogglePosX_OFF = rect.X;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;
            graph.SmoothingMode = SmoothingMode.HighQuality;
            graph.Clear(Parent.BackColor);

            Pen TSPen = new Pen(Color.DarkGray, 3);
            Pen TSPenToggle = new Pen(Color.DarkGray, 3);

            graph.DrawRectangle(TSPen, rect);

            if (Checked == true)
            {
                rectToggle.Location = new Point(TogglePosX_ON, rect.Y);
                graph.FillRectangle(new SolidBrush(Color.LimeGreen), rect);
            }
            else
            {
                rectToggle.Location = new Point(TogglePosX_OFF, rect.Y);
                graph.FillRectangle(new SolidBrush(Color.Tomato), rect);
            }

            //graph.FillRectangle(new SolidBrush(Color.WhiteSmoke), rect);

            graph.DrawRectangle(TSPenToggle, rectToggle);
            graph.FillRectangle(new SolidBrush(Color.White), rectToggle);

            
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            rect = new Rectangle(1, 1, Width - 3, Height - 3);
            rectToggle = new Rectangle(rect.X, rect.Y, rect.Height, rect.Height);

            TogglePosX_ON = rect.Width - rect.Height;
            TogglePosX_OFF = rect.X;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            SwitchToggle();
        }

        private void SwitchToggle()
        {
            Checked = !Checked;
            Invalidate();
        }
    }
}
