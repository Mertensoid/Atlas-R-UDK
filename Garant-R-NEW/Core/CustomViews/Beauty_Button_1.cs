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
    public class Beauty_Button_1 : Control
    {
        private StringFormat SF = new StringFormat();

        //private bool MouseEntered = false;
        //private bool MousePressed = false;
        public Color leftColor = new Color();
        public Color rightColor = new Color();
        

        public Beauty_Button_1()
        {
            //Набор настроек, чтобы полностью избавиться от мерцаний           
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;

            //Первоначальные параметры кнопки
            Size = new System.Drawing.Size(180, 25);
            BackColor = Color.Tomato;
            ForeColor = Color.Black;
            leftColor = Color.LightGray;
            rightColor = Color.Gainsboro;

            //Параметры выравнивания текста 
            SF.Alignment = StringAlignment.Center;
            SF.LineAlignment = StringAlignment.Center;

        }

        public void UseHorizontalLinearGradients(PaintEventArgs e)
        {
            
            LinearGradientBrush linGrBrush = new LinearGradientBrush(
               new Point(0, 0),
               new Point(200, 60),
               leftColor,
               rightColor);

            Pen pen = new Pen(linGrBrush);

            e.Graphics.DrawRectangle(pen, 0, 0, 200, 60);
            e.Graphics.FillRectangle(linGrBrush, 0, 0, 200, 60);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;
            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
             
            graph.Clear(Parent.BackColor);

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            UseHorizontalLinearGradients(e);

            /*
            if (MouseEntered)
            {
                graph.DrawRectangle(new Pen(Color.FromArgb(60, Color.White)), rect);
                graph.FillRectangle(new SolidBrush(Color.FromArgb(60, Color.White)), rect);
            }

            if (MousePressed)
            {
                graph.DrawRectangle(new Pen(Color.FromArgb(60, Color.Black)), rect);
                graph.FillRectangle(new SolidBrush(Color.FromArgb(60, Color.Black)), rect);
            }
            */
            graph.DrawString(Text, Font, new SolidBrush(ForeColor), rect, SF);
        }
        /*
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            MouseEntered = true;

            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            MouseEntered = false;

            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            MousePressed = true;

            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            MousePressed = false;

            Invalidate();
        }
        */
    }
}
