namespace BoomMonitor
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.overview = new System.Windows.Forms.TabPage();
            this.panel = new System.Windows.Forms.Panel();
            this.total_accounts = new System.Windows.Forms.Label();
            this.total_bots = new System.Windows.Forms.Label();
            this.tree = new System.Windows.Forms.TreeView();
            this.logs = new System.Windows.Forms.DataGridView();
            this.date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.showAll = new System.Windows.Forms.Button();
            this.hideAll = new System.Windows.Forms.Button();
            this.update = new System.Windows.Forms.Button();
            this.tabs = new System.Windows.Forms.TabControl();
            this.LostTimer = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.overview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logs)).BeginInit();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // overview
            // 
            this.overview.BackColor = System.Drawing.Color.White;
            this.overview.Controls.Add(this.panel);
            this.overview.Controls.Add(this.total_accounts);
            this.overview.Controls.Add(this.total_bots);
            this.overview.Controls.Add(this.tree);
            this.overview.Controls.Add(this.logs);
            this.overview.Controls.Add(this.showAll);
            this.overview.Controls.Add(this.hideAll);
            this.overview.Controls.Add(this.update);
            resources.ApplyResources(this.overview, "overview");
            this.overview.Name = "overview";
            // 
            // panel
            // 
            resources.ApplyResources(this.panel, "panel");
            this.panel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel.Name = "panel";
            // 
            // total_accounts
            // 
            resources.ApplyResources(this.total_accounts, "total_accounts");
            this.total_accounts.Name = "total_accounts";
            // 
            // total_bots
            // 
            resources.ApplyResources(this.total_bots, "total_bots");
            this.total_bots.Name = "total_bots";
            // 
            // tree
            // 
            resources.ApplyResources(this.tree, "tree");
            this.tree.Name = "tree";
            // 
            // logs
            // 
            this.logs.AllowUserToAddRows = false;
            this.logs.AllowUserToDeleteRows = false;
            this.logs.AllowUserToResizeColumns = false;
            this.logs.AllowUserToResizeRows = false;
            resources.ApplyResources(this.logs, "logs");
            this.logs.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.logs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.logs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.date,
            this.bot,
            this.message});
            this.logs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.logs.Name = "logs";
            this.logs.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.logs.RowHeadersVisible = false;
            this.logs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // date
            // 
            resources.ApplyResources(this.date, "date");
            this.date.Name = "date";
            // 
            // bot
            // 
            resources.ApplyResources(this.bot, "bot");
            this.bot.Name = "bot";
            // 
            // message
            // 
            this.message.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.message, "message");
            this.message.Name = "message";
            // 
            // showAll
            // 
            resources.ApplyResources(this.showAll, "showAll");
            this.showAll.Name = "showAll";
            this.showAll.UseVisualStyleBackColor = true;
            this.showAll.Click += new System.EventHandler(this.ShowAll_Click);
            // 
            // hideAll
            // 
            resources.ApplyResources(this.hideAll, "hideAll");
            this.hideAll.Name = "hideAll";
            this.hideAll.UseVisualStyleBackColor = true;
            this.hideAll.Click += new System.EventHandler(this.HideAll_Click);
            // 
            // update
            // 
            resources.ApplyResources(this.update, "update");
            this.update.Name = "update";
            this.update.UseVisualStyleBackColor = true;
            this.update.Click += new System.EventHandler(this.Button1_Click);
            // 
            // tabs
            // 
            resources.ApplyResources(this.tabs, "tabs");
            this.tabs.Controls.Add(this.overview);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            // 
            // LostTimer
            // 
            this.LostTimer.Enabled = true;
            this.LostTimer.Interval = 300000;
            this.LostTimer.Tick += new System.EventHandler(this.LostTimer_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            resources.ApplyResources(this.notifyIcon1, "notifyIcon1");
            this.notifyIcon1.BalloonTipClicked += new System.EventHandler(this.notifyIcon1_BalloonTipClicked);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabs);
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.overview.ResumeLayout(false);
            this.overview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logs)).EndInit();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Timer timer;
        private System.Windows.Forms.TabPage overview;
        public System.Windows.Forms.DataGridView logs;
        private System.Windows.Forms.Button showAll;
        private System.Windows.Forms.Button hideAll;
        private System.Windows.Forms.Button update;
        public System.Windows.Forms.TabControl tabs;
        public System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.DataGridViewTextBoxColumn date;
        private System.Windows.Forms.DataGridViewTextBoxColumn bot;
        private System.Windows.Forms.DataGridViewTextBoxColumn message;
        private System.Windows.Forms.Label total_bots;
        private System.Windows.Forms.Label total_accounts;
        private System.Windows.Forms.Timer LostTimer;
        public System.Windows.Forms.NotifyIcon notifyIcon1;
        public System.Windows.Forms.Panel panel;
    }
}

