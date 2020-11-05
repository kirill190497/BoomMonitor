using System;
using System.Windows.Forms;

namespace BoomMonitor
{
    public static class Log
    {

        private static readonly Form1 mf = Form1.Instance;

        private static void Action(string message, string bot)
        {

            var date = DateTime.Now.ToString();
            //var msg = date + " - " + message;
            mf.logs.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            var i = mf.logs.Rows.Add(date, bot, message);
            //mf.logs.Rows[i].Cells[1].Style.ForeColor = color;
            mf.logs.FirstDisplayedCell = mf.logs.Rows[^1].Cells[0];

            // mf.logs.Rows.Count - 1
            //mf.logs.TopIndex = mf.logs.Items.Count - 1;
        }

        public static void Add(string message, string bot = "Monitor")//, System.Drawing.Color color
        {


            var date = DateTime.Now.ToString();
            if (mf.InvokeRequired)
            {
                mf.BeginInvoke((Action)(() =>
                {
                    Action(message, bot);
                }));
            }
            else
            {
                Action(message, bot);
            }

            
        }
    }
}
