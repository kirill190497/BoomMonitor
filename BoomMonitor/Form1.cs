using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BoomMonitor
{
    public partial class Form1 : Form
    {
        public static Form1 Instance { get; private set; }
        public List<BotItem> bots = new List<BotItem>();


        public Form1()
        {
            Instance = this;
            InitializeComponent();
            MonitorServer.Start();
            timer.Start();
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Text += " " + Resource.Version;
            notifyIcon1.Text = Text;

            
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            bots.Clear();
            panel.Controls.Clear();
            tree.Nodes.Clear();
            logs.Rows.Clear();
            foreach (TabPage i in tabs.TabPages)
            {
                var name = i.Text.ToString();
                if (name != "Overview")
                {
                    tabs.TabPages.Remove(i);
                }
            }
            Log.Add("Bot list updated");
            SendHello();
        }
        public void CreateBot(JToken bot)
        {
            List<string> list = new List<string>();

            foreach (var i in bots)
            {

                list.Add(i.Name);
                if (i.Port == Convert.ToInt32(bot["port"]) && i.Name == bot["folder"].ToString())
                {
                    // обновление ботов
                    var status = Convert.ToBoolean(bot["status"]);
                    var window = Convert.ToBoolean(bot["window"]);
                    var name = bot["folder"].ToString();
                    var port = Convert.ToInt32(bot["port"]);
                    var calculated = Convert.ToDecimal(bot["calculated"].ToString());
                    var unrealized = Convert.ToDecimal(bot["unrealized"].ToString());
                    var close = Convert.ToDecimal(bot["close"].ToString());
                    var long_pnl = Convert.ToDecimal(bot["long-pnl"].ToString());
                    var short_pnl = Convert.ToDecimal(bot["short-pnl"].ToString());
                    var average = Convert.ToDecimal(bot["average-pnl"].ToString());

                    var usdt = Convert.ToDecimal(bot["balances"]["usdt"].ToString());
                    var bnb = Convert.ToDecimal(bot["balances"]["bnb"].ToString());
                    var margin = Convert.ToDecimal(bot["balances"]["margin"].ToString());
                    var available = Convert.ToDecimal(bot["balances"]["available"].ToString());

                    i.Last = DateTime.Now;
                    i.Calculated = calculated;
                    i.Usdt = usdt;
                    i.Bnb = bnb;
                    i.Magrin = margin;
                    i.Available = available;

                    i.AveragePNL = average;
                    i.ShortPNL = short_pnl;
                    i.LongPNL = long_pnl;
                    i.Close_profit = close;
                    i.Unrealized = unrealized;

                    i.ChangeStatus(status);
                    i.ChangeWindow(window);
                    int percent = 0;
                    try
                    {
                        percent = Convert.ToInt32(decimal.Round(100 / (close / calculated)));
                    }
                    catch
                    {
                        percent = 0;
                    }

                    i.SetBarValue(percent);
                    
                    

                    // обновление веток 
                    foreach (TreeNode wallet in tree.Nodes)
                    {
                        if (wallet.Text == i.Wallet)
                        {

                            foreach (TreeNode node in wallet.Nodes)
                            {
                                if (node.Text.StartsWith("Unrealized"))
                                {
                                    node.Text = "Unrealized: " + unrealized;
                                }
                                if (node.Text.StartsWith("Usdt"))
                                {
                                    node.Text = "Usdt balance: " + usdt;
                                }
                                if (node.Text.StartsWith("Bnb"))
                                {
                                    node.Text = "Bnb balance: " + bnb;
                                }
                                if (node.Text.StartsWith("Available"))
                                {
                                    node.Text = "Available: " + available;
                                }
                                if (node.Text.StartsWith("Margin"))
                                {
                                    node.Text = "Margin balance: " + margin;
                                }


                                if (node.Text == i.Name)
                                {

                                    foreach (TreeNode item in node.Nodes)
                                    {
                                        string statustext = status ? "Started" : "Stoped";

                                        if (item.Text.StartsWith("Calculated"))
                                        {
                                            item.Text = "Calculated: " + calculated;
                                        }

                                        if (item.Text.StartsWith("Status"))
                                        {
                                            item.Text = "Status: " + statustext;
                                        }

                                    }
                                }
                            }
                        }
                    }

                    var wallet_count = tree.Nodes.Count;
                    total_bots.Text = "Total run bots: " + bots.Count;
                    total_accounts.Text = "Total accounts: " + wallet_count;

                }

            }
            if (!list.Contains(bot["folder"].ToString()) || bots.Count == 0)
            {
                bots.Add(new BotItem(bot));
                string msg = "Found a new bot from the folder: " + bot["folder"];
                Log.Add(msg);
                notifyIcon1.BalloonTipText = msg;
                notifyIcon1.BalloonTipTitle = "BoomMonitor";
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;

                notifyIcon1.ShowBalloonTip(1000);
            }


        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MonitorServer.Stop();
            Application.Exit();
        }
        private static void SendHello()
        {
            var port = 49001;
            for (int i = 1; i < 20; i++)
            {
                MonitorServer.Send("hello", port);

                port += 1;
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            SendHello();
            int minutes = 1;
            
            foreach (var bot in bots)
            {
                var delta = DateTime.Now - bot.Last;
                if (delta.TotalMinutes > minutes)
                {
                    LostTimer.Start();
                    if (Convert.ToInt32(delta.TotalSeconds) == 61)
                        LostTimer_Tick(null, null);
                    bot.isLaunched(true);
                    bot.Status = false;
                    foreach (TreeNode wallet in tree.Nodes)
                        if (wallet.Text == bot.Wallet)
                            foreach (TreeNode node in wallet.Nodes)
                                if (node.Text == bot.Name)
                                    foreach (TreeNode item in node.Nodes)
                                    {
                                        string statustext = bot.Status ? "Started" : "Stoped";

                                        if (item.Text.StartsWith("Calculated"))
                                        {
                                            item.Text = "Calculated: " + 0;
                                        }

                                        if (item.Text.StartsWith("Status"))
                                        {
                                            item.Text = "Status: " + statustext;
                                        }

                                    }

                }
                else
                {
                    LostTimer.Stop();
                    bot.isLaunched(false);
                }
                
            }

        }



        private void HideAll_Click(object sender, EventArgs e)
        {
            foreach (var bot in bots)
            {
                MonitorServer.Send("hide", bot.Port);
            }
            Log.Add("All bots are hidden");
        }

        private void ShowAll_Click(object sender, EventArgs e)
        {
            foreach (var bot in bots)
            {
                MonitorServer.Send("show", bot.Port);
            }
            Log.Add("All bots are shown");
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                logs.Height = 300;
            }
            if (WindowState == FormWindowState.Normal)
            {
                logs.Height = 160;
            }

            var loc = logs.Location;
            loc.Y = tabs.Height - logs.Height - tabs.Padding.X * 5;
            logs.Location = loc;
            tree.Height = tabs.Height - logs.Height - tabs.Padding.X * 7;
            logs.Width = tabs.Width - tabs.Padding.X * 3;
            total_accounts.Location = new System.Drawing.Point(total_accounts.Location.X, tabs.Height - logs.Height - total_accounts.Height - tabs.Padding.X * 7);
            total_bots.Location = new System.Drawing.Point(total_bots.Location.X, tabs.Height - logs.Height - total_bots.Height - total_accounts.Height - tabs.Padding.X * 7);
            foreach (var bot in bots)
            {
                bot.logs.Height = logs.Height;
                loc = bot.logs.Location;
                loc.Y = tabs.Height - logs.Height - tabs.Padding.X * 5;
                bot.logs.Location = loc;
                bot.info.Height = tabs.Height - bot.logs.Height - tabs.Padding.X * 7;
                bot.logs.Width = tabs.Width - tabs.Padding.X * 3;
                
                bot.progress.Width = Width - bot.info.Width - 3 * 21;
                loc.Y = loc.Y - bot.progress.Height - tabs.Padding.Y * 3;
                loc.X = bot.progress.Location.X;
                bot.progress.Location = loc;
                
            }

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Button1_Click(update, null);
        }

        private void LostTimer_Tick(object sender, EventArgs e)
        {
            int minutes = 1;

            foreach (var bot in bots)
            {
                var delta = DateTime.Now - bot.Last;
                if (delta.TotalMinutes > minutes)
                {
                    string msg = "Bot '" + bot.Name + "' doesn't respond for " + Math.Round(delta.TotalMinutes) + " minutes";
                    Log.Add(msg);
                    notifyIcon1.BalloonTipText = msg;
                    notifyIcon1.BalloonTipTitle = "BoomMonitor: " + bot.Name;
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Warning;

                    notifyIcon1.ShowBalloonTip(3000);
                }
                
            }

        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            this.Activate();
        }
    }
}
