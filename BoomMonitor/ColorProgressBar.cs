using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoomMonitor
{
    public partial class ColorProgressBar : ProgressBar
    {
        
        

        

        public ColorProgressBar()
        {
            SetStyle(ControlStyles.UserPaint, true);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rec = e.ClipRectangle;

            rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
            rec.Height = rec.Height - 4;
            SolidBrush brush = new SolidBrush(BackColor);
            e.Graphics.FillRectangle(brush, 2, 2, rec.Width, rec.Height);

            string text = Text;
            using (Font f = new Font(FontFamily.GenericSansSerif, 10))
            {

                SizeF len = e.Graphics.MeasureString(text, f);
                // Calculate the location of the text (the middle of progress bar)
                // Point location = new Point(Convert.ToInt32((rect.Width / 2) - (len.Width / 2)), Convert.ToInt32((rect.Height / 2) - (len.Height / 2)));
                Point location = new Point(Convert.ToInt32((Width / 2) - len.Width / 2), Convert.ToInt32((Height / 2) - len.Height / 2));
                // The commented-out code will centre the text into the highlighted area only. This will centre the text regardless of the highlighted area.
                // Draw the custom text
                e.Graphics.DrawString(text, f, new SolidBrush(ForeColor), location);
            }

            //e.Graphics.DrawString(, Font, new SolidBrush(ForeColor), new PointF(Width / 2f, 0));
        }
    }
}
