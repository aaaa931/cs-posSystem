namespace cs_posSystem
{
    partial class form_mdi
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.form2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.排版ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.水平排版ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.垂直排版ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.開啟WordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.開啟WordToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.開啟ExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.form2ToolStripMenuItem,
            this.排版ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // form2ToolStripMenuItem
            // 
            this.form2ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.開啟WordToolStripMenuItem,
            this.開啟WordToolStripMenuItem1,
            this.開啟ExcelToolStripMenuItem});
            this.form2ToolStripMenuItem.Name = "form2ToolStripMenuItem";
            this.form2ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.form2ToolStripMenuItem.Text = "檔案";
            // 
            // 排版ToolStripMenuItem
            // 
            this.排版ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.水平排版ToolStripMenuItem,
            this.垂直排版ToolStripMenuItem});
            this.排版ToolStripMenuItem.Name = "排版ToolStripMenuItem";
            this.排版ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.排版ToolStripMenuItem.Text = "排版";
            // 
            // 水平排版ToolStripMenuItem
            // 
            this.水平排版ToolStripMenuItem.Name = "水平排版ToolStripMenuItem";
            this.水平排版ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.水平排版ToolStripMenuItem.Text = "水平排版";
            this.水平排版ToolStripMenuItem.Click += new System.EventHandler(this.水平排版ToolStripMenuItem_Click);
            // 
            // 垂直排版ToolStripMenuItem
            // 
            this.垂直排版ToolStripMenuItem.Name = "垂直排版ToolStripMenuItem";
            this.垂直排版ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.垂直排版ToolStripMenuItem.Text = "垂直排版";
            this.垂直排版ToolStripMenuItem.Click += new System.EventHandler(this.垂直排版ToolStripMenuItem_Click);
            // 
            // 開啟WordToolStripMenuItem
            // 
            this.開啟WordToolStripMenuItem.Name = "開啟WordToolStripMenuItem";
            this.開啟WordToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.開啟WordToolStripMenuItem.Text = "開啟 pdf";
            this.開啟WordToolStripMenuItem.Click += new System.EventHandler(this.開啟WordToolStripMenuItem_Click);
            // 
            // 開啟WordToolStripMenuItem1
            // 
            this.開啟WordToolStripMenuItem1.Name = "開啟WordToolStripMenuItem1";
            this.開啟WordToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.開啟WordToolStripMenuItem1.Text = "開啟 word";
            this.開啟WordToolStripMenuItem1.Click += new System.EventHandler(this.開啟WordToolStripMenuItem1_Click);
            // 
            // 開啟ExcelToolStripMenuItem
            // 
            this.開啟ExcelToolStripMenuItem.Name = "開啟ExcelToolStripMenuItem";
            this.開啟ExcelToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.開啟ExcelToolStripMenuItem.Text = "開啟 excel";
            this.開啟ExcelToolStripMenuItem.Click += new System.EventHandler(this.開啟ExcelToolStripMenuItem_Click);
            // 
            // form_mdi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "form_mdi";
            this.Text = "Form3";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem form2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 排版ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 水平排版ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 垂直排版ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 開啟WordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 開啟WordToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 開啟ExcelToolStripMenuItem;
    }
}