namespace Minesweeper2
{
    public partial class Minesweeper2 : Form
    {
        private GameState _gameState;
        private Dictionary<Point, Cell> _cellList = new Dictionary<Point, Cell>();

        public Minesweeper2()
        {
            InitializeComponent();
            _gameState = new GameState();
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
                    Point cellPosition = new Point(col, row); // 修正: 座標をセルのインデックスとして管理

                    Cell cell = new Cell(_cellList, cellPosition, _gameState)
                    {
                        Location = new Point(col * cellSize, row * cellSize), // 表示位置
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
                cell.Degree = 0; // Degreeをリセット
            }
            PlaceMines();
            _gameState.CurrentState = GameState.State.Running; // ゲームの状態をRunningに設定
        }

        private void PlaceMines()
        {
            foreach (var cell in _cellList.Values)
            {
                cell.Mine = false;
            }

            int mineCount = (int)(_cellList.Count * Properties.Settings.Default.PercentageOfMines / 100.0);
            Random random = new Random();

            var cellKeys = _cellList.Keys.OrderBy(_ => random.Next()).Take(mineCount);

            foreach (var minePosition in cellKeys)
            {
                _cellList[minePosition].Mine = true;
                UpdateCellDegree(minePosition);
            }
        }

        private void UpdateCellDegree(Point minePosition)
        {
            int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };

            for (int i = 0; i < 8; i++)
            {
                Point neighborPosition = new Point(minePosition.X + dx[i], minePosition.Y + dy[i]);
                if (_cellList.TryGetValue(neighborPosition, out Cell? value))
                {
                    value.Degree++;
                }
            }
        }
    }
}
