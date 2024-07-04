namespace Minesweeper2
{
    public partial class Minesweeper2 : Form
    {
        private Dictionary<Point, Cell> _cellList = new Dictionary<Point, Cell>();

        public Minesweeper2()
        {
            InitializeComponent();
            InitializeCellList();
        }

        private void Minesweeper2_Load(object sender, EventArgs e)
        {
            FillMainPanel();
        }

        private void InitializeCellList()
        {
            int Rows = Properties.Settings.Default.NumberOfRows;
            int Colmns = Properties.Settings.Default.NumberOfColumns;

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Colmns; col++)
                {
                    int cellSize = 50;
                    Point cellPosition = new Point(col * cellSize, row * cellSize);

                    Cell cell = new Cell
                    {
                        Location = cellPosition,
                        Size = new Size(cellSize, cellSize),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        CurrentMode = Cell.Mode.BtnBlank
                    };

                    _cellList.Add(cellPosition, cell);
                }
            }
        }

        private void FillMainPanel()
        {
            MainPanel.Controls.Clear();

            foreach (var cellEntry in _cellList)
            {
                Cell cell = cellEntry.Value;
                MainPanel.Controls.Add(cell);
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            foreach (var cellEntry in _cellList)
            {
                Cell cell = cellEntry.Value;
                cell.CurrentMode = Cell.Mode.BtnBlank;
            }
        }
    }
}
