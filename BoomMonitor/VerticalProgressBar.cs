using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoomMonitor
{
    public class VerticalProgressBar : ProgressBar
    {
        protected override CreateParams CreateParams
        {
            get
            {
                // Avoid CA2122
                //new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();

                CreateParams cp = base.CreateParams;
                cp.Style |= 0x04;
                
                return cp;
            }
        }

        public VerticalProgressBar()
        {
            // Enable OnPaint overriding
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (ProgressBarRenderer.IsSupported)
            {
                
                ProgressBarRenderer.DrawVerticalBar(e.Graphics, ClientRectangle);

                const int HORIZ_OFFSET = 0;
                const int VERT_OFFSET = 0;

                


                if (this.Minimum == this.Maximum || (this.Value - Minimum) == 0 ||
                        this.Height < 2 * VERT_OFFSET || this.Width < 2 * VERT_OFFSET)
                    return;

                int barHeight = (this.Value - this.Minimum) * this.Height / (this.Maximum - this.Minimum);
                barHeight = Math.Min(barHeight, this.Height - 2 * VERT_OFFSET);
                int barWidth = this.Width - 2 * HORIZ_OFFSET;

                if (this.RightToLeftLayout && this.RightToLeft == System.Windows.Forms.RightToLeft.No)
                {
                    
                    SolidBrush brush = new SolidBrush(BackColor);
                    e.Graphics.FillRectangle(brush, HORIZ_OFFSET, VERT_OFFSET, barWidth, barHeight);
                }
                else
                {
                    int blockHeight = 1;
                    int wholeBarHeight = Convert.ToInt32(barHeight / blockHeight) * blockHeight;
                    int wholeBarY = this.Height - wholeBarHeight - VERT_OFFSET;
                    int restBarHeight = barHeight % blockHeight;
                    int restBarY = this.Height - barHeight - VERT_OFFSET;
                    SolidBrush brush = new SolidBrush(BackColor);
                    e.Graphics.FillRectangle(brush, HORIZ_OFFSET, wholeBarY, barWidth, wholeBarHeight);
                    e.Graphics.FillRectangle(brush, HORIZ_OFFSET, restBarY, barWidth, restBarHeight);
                    //ProgressBarRenderer.DrawVerticalChunks(e.Graphics,
                    //  new Rectangle(HORIZ_OFFSET, wholeBarY, barWidth, wholeBarHeight));
                    
                }

            }

            string text = Text;
            using (Font f = new Font(FontFamily.GenericSansSerif, 10))
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
                SizeF len = e.Graphics.MeasureString(text, f);
                // Calculate the location of the text (the middle of progress bar)
                // Point location = new Point(Convert.ToInt32((rect.Width / 2) - (len.Width / 2)), Convert.ToInt32((rect.Height / 2) - (len.Height / 2)));
                Point location = new Point(Convert.ToInt32((Width / 2) - len.Height / 2), Convert.ToInt32((Height / 2) - len.Width / 2));
                // The commented-out code will centre the text into the highlighted area only. This will centre the text regardless of the highlighted area.
                // Draw the custom text
                e.Graphics.DrawString(text, f, new SolidBrush(ForeColor), location, stringFormat);
            }

            base.OnPaint(e);
        }
    }
}
