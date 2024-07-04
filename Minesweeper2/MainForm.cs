namespace Minesweeper2
{
    public partial class Minesweeper2 : Form
    {
        public enum State
        {
            Init,
            Run,
            Pausd,
            End
        }

        public State CurrentState { get; set; }
        private Dictionary<Point, Cell> _cellList = new Dictionary<Point, Cell>();

        public Minesweeper2()
        {
            InitializeComponent();
            CurrentState = State.Init;
            InitializeCellList();
        }

        private void Minesweeper2_Load(object sender, EventArgs e)
        {
            FillMainPanel();
        }

        private void InitializeCellList()
        {
            int rows = Properties.Settings.Default.NumberOfRows;
            int columns = Properties.Settings.Default.NumberOfColumns;
            int cellSize = 50;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Point cellPosition = new Point(col, row);
                    Cell cell = CreateCell(cellSize);
                    cell.Location = new Point(cellPosition.X * cellSize, cellPosition.Y * cellSize);
                    _cellList.Add(cellPosition, cell);
                }
            }
        }

        private Cell CreateCell(int cellSize)
        {
            return new Cell(this)
            {
                Size = new Size(cellSize, cellSize),
                SizeMode = PictureBoxSizeMode.StretchImage,
                CurrentMode = Cell.Mode.BtnBlank
            };
        }

        private void FillMainPanel()
        {
            MainPanel.Controls.Clear();
            foreach (var cell in _cellList.Values)
            {
                MainPanel.Controls.Add(cell);
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            ResetCells();
            PlaceMines();
            CurrentState = State.Run;
        }

        private void ResetCells()
        {
            foreach (var cell in _cellList.Values)
            {
                cell.CurrentMode = Cell.Mode.BtnBlank;
                cell.Degree = 0;
            }
        }

        private void PlaceMines()
        {
            foreach (var cell in _cellList.Values)
            {
                cell.Mine = false;
            }

            int mineCount = (int)(_cellList.Count * Properties.Settings.Default.PercentageOfMines / 100.0);
            Random random = new Random();
            var minePositions = _cellList.Keys.OrderBy(_ => random.Next()).Take(mineCount);

            foreach (var minePosition in minePositions)
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
                if (_cellList.TryGetValue(neighborPosition, out Cell? neighborCell))
                {
                    neighborCell.Degree++;
                }
            }
        }

        public void RevealAdjacentCells(Cell cell)
        {
            Point[] directions =
            [
                new Point(-1, -1), new Point(0, -1), new Point(1, -1),
                new Point(-1, 0), new Point(1, 0),
                new Point(-1, 1), new Point(0, 1), new Point(1, 1)
            ];

            foreach (var direction in directions)
            {
                int cellX = cell.Location.X / cell.Width;
                int cellY = cell.Location.Y / cell.Height;
                Point neighborPosition = new Point(cellX + direction.X, cellY + direction.Y);
                if (_cellList.TryGetValue(neighborPosition, out Cell? neighborCell) && neighborCell.CurrentMode == Cell.Mode.BtnBlank)
                {
                    neighborCell.RevealCells();
                }
            }
        }


        public void EndGame(bool isGameOver)
        {
            if (CurrentState == State.End) return;

            CurrentState = State.End;
            RevealAllCells();
            ShowEndGameMessage(isGameOver);
        }

        private void RevealAllCells()
        {
            foreach (var cell in _cellList.Values)
            {
                if (cell.CurrentMode == Cell.Mode.BtnBlank || cell.CurrentMode == Cell.Mode.BtnFlag || cell.CurrentMode == Cell.Mode.BtnHold)
                {
                    if (cell.Mine)
                    {
                        cell.CurrentMode = Cell.Mode.AnsMine;
                    }
                    else
                    {
                        cell.CurrentMode = cell.CurrentMode switch
                        {
                            Cell.Mode.BtnFlag => Cell.Mode.BtnFlagX,
                            Cell.Mode.BtnHold => Cell.Mode.BtnHoldX,
                            _ => cell.CurrentMode
                        };
                        if (cell.CurrentMode == Cell.Mode.BtnBlank)
                        {
                            cell.RevealCells();
                        }
                    }
                }
            }
        }

        private static void ShowEndGameMessage(bool isGameOver)
        {
            string message = isGameOver ? "ゲームオーバー！" : "ゲームクリア！";
            string caption = isGameOver ? "残念でした" : "おめでとうございます";
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void CheckGameState()
        {
            if (CurrentState == State.End) return;

            bool isGameClear = _cellList.Values.All(cell => cell.CurrentMode != Cell.Mode.BtnBlank);

            if (isGameClear)
            {
                EndGame(false);
            }
        }
    }
}
