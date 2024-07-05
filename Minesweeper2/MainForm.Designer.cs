namespace Minesweeper2
{
    partial class Minesweeper2
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            MainPanel = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            BtnStart = new Button();
            BtnPause = new Button();
            tableLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(MainPanel, 0, 1);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5.81717443F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 94.18282F));
            tableLayoutPanel1.Size = new Size(974, 1029);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // MainPanel
            // 
            MainPanel.BorderStyle = BorderStyle.Fixed3D;
            MainPanel.Dock = DockStyle.Fill;
            MainPanel.Location = new Point(3, 62);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(968, 964);
            MainPanel.TabIndex = 4;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(BtnStart);
            flowLayoutPanel1.Controls.Add(BtnPause);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(3, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(968, 53);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // BtnStart
            // 
            BtnStart.Location = new Point(3, 3);
            BtnStart.Name = "BtnStart";
            BtnStart.Size = new Size(150, 46);
            BtnStart.TabIndex = 6;
            BtnStart.Text = "Start";
            BtnStart.UseVisualStyleBackColor = true;
            BtnStart.Click += BtnStart_Click;
            // 
            // BtnPause
            // 
            BtnPause.Location = new Point(159, 3);
            BtnPause.Name = "BtnPause";
            BtnPause.Size = new Size(150, 46);
            BtnPause.TabIndex = 7;
            BtnPause.Text = "Pause";
            BtnPause.UseVisualStyleBackColor = true;
            BtnPause.Visible = false;
            BtnPause.Click += BtnPause_Click;
            // 
            // Minesweeper2
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(974, 1029);
            Controls.Add(tableLayoutPanel1);
            Name = "Minesweeper2";
            Text = "Minesweeper2";
            Load += Minesweeper2_Load;
            tableLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button BtnPause;
        private Button BtnStart;
        private Panel MainPanel;
    }
}
