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
        private bool _isResizing;

        public Minesweeper2()
        {
            InitializeComponent();
            CurrentState = State.Init;
            InitializeCellList();
            this.SizeChanged += Minesweeper2_SizeChanged;
            this.ResizeBegin += Minesweeper2_ResizeBegin;
            this.ResizeEnd += Minesweeper2_ResizeEnd;
        }

        private void Minesweeper2_Load(object sender, EventArgs e)
        {
            FillMainPanel();
        }

        private void InitializeCellList()
        {
            int rows = Properties.Settings.Default.NumberOfRows;
            int columns = Properties.Settings.Default.NumberOfColumns;
            int cellSize = CalculateCellSize(rows, columns);

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

        private int CalculateCellSize(int rows, int columns)
        {
            int panelSize = Math.Min(MainPanel.Width, MainPanel.Height);
            return panelSize / Math.Max(rows, columns);
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
            SetGameState(State.Run);
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
            var minePositions = GetRandomMinePositions(mineCount);

            foreach (var minePosition in minePositions)
            {
                _cellList[minePosition].Mine = true;
                UpdateCellDegree(minePosition);
            }
        }

        private IEnumerable<Point> GetRandomMinePositions(int mineCount)
        {
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                return _cellList.Keys.OrderBy(_ =>
                {
                    byte[] randomNumber = new byte[4];
                    rng.GetBytes(randomNumber);
                    return BitConverter.ToInt32(randomNumber, 0);
                }).Take(mineCount).ToList();
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
            if (cell == null) throw new ArgumentNullException(nameof(cell));

            int cellX = cell.Location.X / cell.Width;
            int cellY = cell.Location.Y / cell.Height;

            Point[] directions = new Point[]
            {
                new Point(-1, -1), new Point(0, -1), new Point(1, -1),
                new Point(-1, 0), new Point(1, 0),
                new Point(-1, 1), new Point(0, 1), new Point(1, 1)
            };

            foreach (var direction in directions)
            {
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

            SetGameState(State.End);
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

            bool isGameClear = _cellList.Values.All(cell =>
                (cell.Mine && cell.CurrentMode == Cell.Mode.BtnFlag) ||
                (!cell.Mine && cell.CurrentMode != Cell.Mode.BtnBlank)
            );

            if (isGameClear)
            {
                EndGame(false);
            }
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            if (CurrentState == State.Run)
            {
                SetGameState(State.Pausd);
            }
            else if (CurrentState == State.Pausd)
            {
                SetGameState(State.Run);
            }
        }

        private void SetGameState(State newState)
        {
            CurrentState = newState;
            BtnPause.Visible = newState switch
            {
                State.Init => false,
                State.Run => true,
                State.Pausd => true,
                State.End => false,
                _ => BtnPause.Visible
            };
            BtnPause.Text = newState switch
            {
                State.Pausd => "Resume",
                State.Run => "Pause",
                _ => BtnPause.Text
            };
        }

        private void Minesweeper2_SizeChanged(object? sender, EventArgs e)
        {
            if (!_isResizing)
            {
                ResizeCells();
            }
        }

        private void Minesweeper2_ResizeBegin(object? sender, EventArgs e)
        {
            _isResizing = true;
        }

        private void Minesweeper2_ResizeEnd(object? sender, EventArgs e)
        {
            _isResizing = false;
            ResizeCells();
        }

        private void ResizeCells()
        {
            int rows = Properties.Settings.Default.NumberOfRows;
            int columns = Properties.Settings.Default.NumberOfColumns;
            int cellSize = CalculateCellSize(rows, columns);

            foreach (var kvp in _cellList)
            {
                Point cellPosition = kvp.Key;
                Cell cell = kvp.Value;
                cell.Size = new Size(cellSize, cellSize);
                cell.Location = new Point(cellPosition.X * cellSize, cellPosition.Y * cellSize);
            }

            // パネルの右辺の余分なスペースを削除
            MainPanel.Width = cellSize * columns;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }
    }
}
