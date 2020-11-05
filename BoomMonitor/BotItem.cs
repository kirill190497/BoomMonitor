using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace BoomMonitor
{


    public class BotItem
    {
        private static readonly Form1 mf = Form1.Instance;
        public int Port { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public bool Window { get; set; }

        public decimal Calculated { get; set; }
        public decimal Unrealized { get; set; }
        public decimal LongPNL { get; set; }
        public decimal ShortPNL { get; set; }

        public decimal AveragePNL { get; set; }
        public decimal Close_profit { get; set; }

        public string Path_to_dir { get; set; }
        public decimal Usdt { get; set; }
        public decimal Available { get; set; }
        public int  Percent { get; set; }
        public decimal Magrin { get; set; }
        public decimal Bnb { get; set; }

        public string Wallet { get; set; }
        public DateTime Last { get; set; }

        public ColorProgressBar progress = new ColorProgressBar();

        public TabPage tab = new TabPage();
        public VerticalProgressBar vertical = new VerticalProgressBar();

        private readonly Button stop = new Button();
        public readonly Button start = new Button();
        private readonly Button hide_show = new Button();
        private readonly Button folder = new Button();
        private readonly Button launch = new Button();


        public DataGridView logs = new DataGridView();
        public DataGridView info = new DataGridView();


        public BotItem(JToken bot)
        {
            //для монитора
            this.Port = Convert.ToInt32(bot["port"]);
            this.Name = bot["folder"].ToString();
            this.Status = Convert.ToBoolean(bot["status"]);
            this.Window = Convert.ToBoolean(bot["window"]);
            this.Path_to_dir = bot["path"].ToString();
            this.Wallet = bot["wallet"].ToString();
            this.Last = DateTime.Now;

            //Данные бота - инфо
            this.Calculated = Convert.ToDecimal(bot["calculated"]);
            this.Unrealized = Convert.ToDecimal(bot["unrealized"]);
            this.Close_profit = Convert.ToDecimal(bot["close"]);

            this.LongPNL = Convert.ToDecimal(bot["long-pnl"]);
            this.ShortPNL = Convert.ToDecimal(bot["short-pnl"]);
            this.AveragePNL = Convert.ToDecimal(bot["average-pnl"]);

            //балансы

            this.Usdt = Convert.ToDecimal(bot["balances"]["usdt"]);
            this.Available = Convert.ToDecimal(bot["balances"]["available"]);
            this.Magrin = Convert.ToDecimal(bot["balances"]["margin"]);
            this.Bnb = Convert.ToDecimal(bot["balances"]["bnb"]);
            

            
            AddNode();
            AddInfoTable();
            AddFilter();
            AddToPanel();
            CreateTab();

        }

        public void SetBarValue(int percent)
        {
           
            if (percent < 0)
            {
                progress.BackColor = Color.IndianRed;
                vertical.BackColor = Color.IndianRed;

                vertical.RightToLeftLayout = true;
                

                if (Math.Abs(percent) <= 100)
                {
                    progress.Value = Math.Abs(percent);
                    vertical.Value = Math.Abs(percent);
                }  
                else if (Math.Abs(percent) <= 200)
                {
                    progress.BackColor = Color.Red;
                    vertical.BackColor = Color.Red;
                }
                else
                {
                    progress.BackColor = Color.DarkRed;
                    vertical.BackColor = Color.DarkRed;
                    
                }
                int temp = Math.Abs(percent);
                while (temp > 100)
                    temp -= 100;
                progress.Value = temp;
                vertical.Value = temp;
            }
            else if (percent >= 100)
            {
                progress.BackColor = Color.Green;
                vertical.BackColor = Color.Green;
                int temp = percent;
                while (temp > 100)
                    temp -= 100;
                progress.Value = temp;
                vertical.Value = temp;
            }
            else
            {
                vertical.RightToLeftLayout = false;
                
                progress.BackColor = Color.Orange;
                vertical.BackColor = Color.Orange;
                progress.Value = percent;
                vertical.Value = percent;
            }

            progress.Text = percent + "%";
            vertical.Text = Name + ": " + percent + "%";
        }

        private void AddToPanel()
        {
            vertical.Width = 15;
            vertical.Minimum = 0;
            vertical.Maximum = 100;
            vertical.Location = new Point(mf.panel.Controls.Count*18+3, 3);

            vertical.Height = mf.panel.Height - 6;

            vertical.Value = 0;
            
            
            vertical.ForeColor = Color.Black;
            mf.panel.Controls.Add(vertical);
        }

        private void AddInfoTable()
        {
            var dcol = new DataGridViewTextBoxColumn
            {
                Width = 120,
                HeaderText = "Parameter",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                Name = "parameter"
            };



            var mcol = new DataGridViewTextBoxColumn
            {
                Width = 120,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                HeaderText = "Value",
                Name = "value"
            };


            info.AllowUserToAddRows = false;
            info.AllowUserToDeleteRows = false;
            info.AllowUserToResizeColumns = false;
            info.AllowUserToResizeRows = false;
            info.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            info.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            info.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            info.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            dcol,
            mcol});
            //resources.ApplyResources(logs, "logs");
            info.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            info.Name = "info";
            info.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            info.RowHeadersVisible = false;
            info.Location = new Point(3, 3);
            info.Size = new Size(240, mf.Height - logs.Height - 112);

            tab.Controls.Add(info);
        }

        private void AddFilter()
        {
            
        }
        private void AddNode()
        {
            if (mf.tree.Nodes.Count > 0)
            {

                var wallets = new List<string>();
                foreach (TreeNode tr in mf.tree.Nodes)
                {
                    wallets.Add(tr.Text.ToString());
                }


                if (!wallets.Contains(Wallet))
                {
                    var wnode = new TreeNode(Wallet);

                    wnode.Nodes.Add("Unrealized: " + Unrealized);
                    wnode.Nodes.Add("Usdt balance: " + Usdt);
                    wnode.Nodes.Add("Available: " + Available);
                    wnode.Nodes.Add("Margin balance: " + Magrin);
                    wnode.Nodes.Add("Bnb balance: " + Bnb);

                    string text = Status ? "Started" : "Stoped";

                    var node = new TreeNode(Name);

                    node.Nodes.Add("Status: " + text);
                    node.Nodes.Add("Calculated: " + Calculated);



                    wnode.Nodes.Add(node);
                    mf.tree.BeginUpdate();
                    mf.tree.Nodes.Add(wnode);
                    mf.tree.EndUpdate();
                }
                else
                {
                    foreach (TreeNode tr in mf.tree.Nodes)
                    {
                        if (tr.Text == Wallet)
                        {
                            string text = Status ? "Started" : "Stoped";

                            var node = new TreeNode(Name);

                            node.Nodes.Add("Status: " + text);
                            node.Nodes.Add("Calculated: " + Calculated);

                            mf.tree.BeginUpdate();
                            tr.Nodes.Add(node);
                            mf.tree.EndUpdate();
                        }
                    }
                }

            }
            else
            {
                var wnode = new TreeNode(Wallet);
                wnode.Nodes.Add("Unrealized: " + Unrealized);
                wnode.Nodes.Add("Usdt balance: " + Usdt);
                wnode.Nodes.Add("Available: " + Available);
                wnode.Nodes.Add("Margin balance: " + Magrin);
                wnode.Nodes.Add("Bnb balance: " + Bnb);

                string text = Status ? "Started" : "Stoped";

                var node = new TreeNode(Name);

                node.Nodes.Add("Status: " + text);
                node.Nodes.Add("Calculated: " + Calculated);
                wnode.Nodes.Add(node);
                mf.tree.BeginUpdate();
                mf.tree.Nodes.Add(wnode);
                mf.tree.EndUpdate();
            }






        }

        private void CreateTab()
        {

            tab.Text = Name;
            tab.UseVisualStyleBackColor = true;



            start.Text = "Start";
            start.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            start.Location = new Point(100, 10);
            start.Width = 90;

            start.Click += (sender, args) =>
            {
                MonitorServer.Send("start", Port);
            };


            stop.Text = "Stop";
            stop.Width = 90;
            stop.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            stop.Location = new Point(100, 40);

            stop.Click += (sender, args) =>
            {
                MonitorServer.Send("stop", Port);
            };

            


            if (Window)
                hide_show.Text = "Hide";
            else
                hide_show.Text = "Show";
            hide_show.Width = 90;
            hide_show.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            hide_show.Location = new Point(100, 70);

            hide_show.Click += (sender, args) =>
            {
                string msg;
                if (Window)
                    msg = "hide";
                else
                    msg = "show";
                MonitorServer.Send(msg, Port);

                hide_show.Text = Window ? "Hide" : "Show";
            };



            folder.Text = "Logs folder";
            folder.Width = 90;
            folder.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            folder.Location = new Point(100, 100);

            folder.Click += (sender, args) =>
            {
                try
                {

                    Process.Start("explorer.exe", Path_to_dir + "\\logs");
                }
                catch (Exception)
                {

                }
            };



            launch.Text = "Launch bot";
            launch.Width = 90;
            launch.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            launch.Location = new Point(100, 130);

            launch.Click += (sender, args) =>
            {
                try
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(Path_to_dir + "\\BoomTrader_2.exe")
                    {
                        WorkingDirectory = Path_to_dir
                    };

                    Process.Start(processStartInfo);

                }
                catch (Exception ex)
                {
                    Log.Add(ex.Message);
                }
            };

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));

            var dcol = new DataGridViewTextBoxColumn
            {
                HeaderText = "Date",
                Name = "date"
            };
            resources.ApplyResources(dcol, "date");


            var mcol = new DataGridViewTextBoxColumn
            {
                Width = 150,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                HeaderText = "Message",
                Name = "message"
            };
            resources.ApplyResources(mcol, "message");

            //progress.Anchor = (AnchorStyles.Top| AnchorStyles.Left | AnchorStyles.Right);
            


            logs.AllowUserToAddRows = false;
            logs.AllowUserToDeleteRows = false;
            logs.AllowUserToResizeColumns = false;
            logs.AllowUserToResizeRows = false;
            logs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            logs.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            logs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            logs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            dcol,
            mcol});
            //resources.ApplyResources(logs, "logs");
            logs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            logs.Name = "logs";
            logs.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            logs.RowHeadersVisible = false;
            logs.Location = new Point(3, 269);
            logs.Size = new Size(mf.Width - 54, 160);

            progress.ForeColor = Color.Black;
            progress.Height = 18;
            progress.Location = new Point(info.Width + 9, logs.Location.Y - progress.Height - mf.tabs.Padding.Y*2 );
            progress.Value = 0;
            progress.Width = mf.Width - info.Width - 3 * 20;
           

            tab.Controls.Add(start);
            tab.Controls.Add(stop);
            tab.Controls.Add(hide_show);
            tab.Controls.Add(launch);
            tab.Controls.Add(folder);
            tab.Controls.Add(logs);
            tab.Controls.Add(progress);
            mf.tabs.TabPages.Add(tab);
        }

        private void Action(string message)
        {

            var date = DateTime.Now.ToString();
            logs.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            var i = logs.Rows.Add(date, message);
            logs.FirstDisplayedCell = logs.Rows[^1].Cells[0];

        }

        public void AddLog(string message)//, System.Drawing.Color color
        {


            var date = DateTime.Now.ToString();
            if (mf.InvokeRequired)
            {
                mf.BeginInvoke((Action)(() =>
                {
                    Action(message);
                }));
            }
            else
            {
                Action(message);
            }
        }

        public void ChangeStatus(bool status)//, System.Drawing.Color color
        {
            start.Enabled = !status;
            stop.Enabled = status;
            UpdateInfo(status);
        }

        public void isLaunched(bool launched)//, System.Drawing.Color color
        {
            launch.Enabled = launched;
        }

        public void ChangeWindow(bool window)//, System.Drawing.Color color
        {
            if (window)
                hide_show.Text = "Hide";
            else
                hide_show.Text = "Show";
            this.Window = window;
        }

        public void UpdateInfo(bool status)
        {
            if (status)
            {
                info.Rows.Clear();
                info.Rows.Add("Average PnL", this.AveragePNL);
                info.Rows.Add("Long PnL", this.LongPNL);
                info.Rows.Add("Short PnL", this.ShortPNL);
                info.Rows.Add("Calculated", this.Calculated);
                info.Rows.Add("Close profit", this.Close_profit);
            }
            else
            {
                info.Rows.Clear();
            }
        }

    }
}
