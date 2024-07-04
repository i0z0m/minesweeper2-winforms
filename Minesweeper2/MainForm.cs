namespace Minesweeper2
{
    public partial class Minesweeper2 : Form
    {
        public Minesweeper2()
        {
            InitializeComponent();
        }

        private void Minesweeper2_Load(object sender, EventArgs e)
        {
            FillCells();
        }

        private void FillCells()
        {
            MainPanel.Controls.Clear(); // 既存のコントロールをクリア

            int numRows = Properties.Settings.Default.NumberOfRows;
            int numCols = Properties.Settings.Default.NumberOfColumns;
            int cellSize = 50; // 各セルのサイズ

            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    Cell cell = new Cell();
                    cell.CurrentMode = Cell.Mode.BtnBlank; // セルの初期モードを設定
                    cell.Location = new Point(col * cellSize, row * cellSize); // セルの位置を設定
                    cell.Size = new Size(cellSize, cellSize); // セルのサイズを設定
                    cell.SizeMode = PictureBoxSizeMode.StretchImage; // 画像のサイズモードを設定
                    MainPanel.Controls.Add(cell); // MainPanelにセルを追加
                }
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            FillCells();
        }
    }
}
