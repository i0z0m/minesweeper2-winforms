namespace Minesweeper2
{
    partial class Minesweeper2
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
            MainPanel = new Panel();
            BtnStart = new Button();
            BtnPause = new Button();
            SuspendLayout();
            // 
            // MainPanel
            // 
            MainPanel.BorderStyle = BorderStyle.Fixed3D;
            MainPanel.Location = new Point(12, 71);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(1006, 1007);
            MainPanel.TabIndex = 0;
            // 
            // BtnStart
            // 
            BtnStart.Location = new Point(12, 12);
            BtnStart.Name = "BtnStart";
            BtnStart.Size = new Size(150, 46);
            BtnStart.TabIndex = 1;
            BtnStart.Text = "Start";
            BtnStart.UseVisualStyleBackColor = true;
            BtnStart.Click += BtnStart_Click;
            // 
            // BtnPause
            // 
            BtnPause.Location = new Point(168, 12);
            BtnPause.Name = "BtnPause";
            BtnPause.Size = new Size(150, 46);
            BtnPause.TabIndex = 2;
            BtnPause.Text = "Pause";
            BtnPause.UseVisualStyleBackColor = true;
            BtnPause.Visible = false;
            BtnPause.Click += BtnPause_Click;
            // 
            // Minesweeper2
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1026, 1083);
            Controls.Add(BtnPause);
            Controls.Add(BtnStart);
            Controls.Add(MainPanel);
            Name = "Minesweeper2";
            Text = "Minesweeper2";
            Load += Minesweeper2_Load;
            ResumeLayout(false);
        }

        #endregion

        private Panel MainPanel;
        private Button BtnStart;
        private Button BtnPause;
    }
}
