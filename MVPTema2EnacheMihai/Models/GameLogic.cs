using MVPTema2EnacheMihai.Commands;
using MVPTema2EnacheMihai.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVPTema2EnacheMihai.Models
{
    public class GameLogic : INotifyPropertyChanged
    {
        public ICommand CellClickedCommand { get; }
        public ObservableCollection<CheckerCell> Cells { get; } = new ObservableCollection<CheckerCell>();
        private bool _isPlayerOneTurn = true;
        public bool IsPlayerOneTurn
        {
            get { return _isPlayerOneTurn; }
            set
            {
                if (_isPlayerOneTurn != value)
                {
                    _isPlayerOneTurn = value;
                    OnPropertyChanged(nameof(IsPlayerOneTurn));
                }
            }
        }

        public GameLogic()
        {
            InitializeBoard();
            CellClickedCommand = new RelayCommand<CheckerCell>(CellClicked);
        }


         private void InitializeBoard()
         {
             Console.WriteLine("Board Initialized");
             for (int row = 0; row < 8; row++)
             {
                 for (int col = 0; col < 8; col++)
                 {
                     bool isDark = (row + col) % 2 == 0;
                     var cell = new CheckerCell { IsDark = isDark, Row = row, Column = col };

                     // Place pieces for player one on the first three rows of dark cells
                     if (isDark && row < 3)
                     {
                         cell.Pieces = new Pieces { IsPlayerOne = true };
                     }

                     // Place pieces for player two on the last three rows of dark cells
                     if (isDark && row > 4)
                     {
                         cell.Pieces = new Pieces { IsPlayerOne = false };
                     }

                     Cells.Add(cell);
                 }
             }
         }
       /* private void InitializeBoard()
        {
            Console.WriteLine("Board Initialized");

            // Clear existing cells
            Cells.Clear();

            // Create a new board with two pieces, one for each player
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var cell = new CheckerCell { IsDark = (row + col) % 2 == 0, Row = row, Column = col };

                    // Place a piece for player one at a specific location
                    if (row == 0 && col == 0)
                    {
                        cell.Pieces = new Pieces { IsPlayerOne = true };
                    }

                    // Place a piece for player two at a specific location
                    if (row == 7 && col == 7)
                    {
                        cell.Pieces = new Pieces { IsPlayerOne = false };
                    }

                    Cells.Add(cell);
                }
            }
        }*/





        private CheckerCell _selectedCell;

       
        private void HighlightAvailableMoves(CheckerCell cell)
        {
            Console.WriteLine("Calculating highlight");
            // Calculate the positions of the diagonally adjacent cells
            int[,] adjacentPositions = new int[,]
            {
        { cell.Row - 1, cell.Column - 1 }, // Up-left
        { cell.Row - 1, cell.Column + 1 }, // Up-right
        { cell.Row + 1, cell.Column - 1 }, // Down-left
        { cell.Row + 1, cell.Column + 1 }  // Down-right
            };

            for (int i = 0; i < 4; i++)
            {
                int row = adjacentPositions[i, 0];
                int col = adjacentPositions[i, 1];

                // Check if the position is within the bounds of the board
                if (row >= 0 && row < 8 && col >= 0 && col < 8)
                {
                    var adjacentCell = Cells[row * 8 + col];

                    // Check if the cell is available for a move
                    if (adjacentCell.Pieces == null)
                    {
                        // Check the direction of movement
                        if (cell.Pieces.IsKing || (cell.Pieces.IsPlayerOne && row > cell.Row) || (!cell.Pieces.IsPlayerOne && row < cell.Row))
                        {
                            // Highlight the cell as available for a regular move
                            adjacentCell.IsAvailable = true;
                        }
                    }
                    else if (adjacentCell.Pieces.IsPlayerOne != cell.Pieces.IsPlayerOne)
                    {
                        // Check if there's an enemy piece that can be jumped over
                        int destRow = row + (row - cell.Row);
                        int destCol = col + (col - cell.Column);

                        // Check if the destination cell is within the bounds of the board
                        if (destRow >= 0 && destRow < 8 && destCol >= 0 && destCol < 8)
                        {
                            var destCell = Cells[destRow * 8 + destCol];

                            // Check if the destination cell is empty
                            if (destCell.Pieces == null)
                            {
                                // Check the direction of movement
                                if (cell.Pieces.IsKing || (cell.Pieces.IsPlayerOne && destRow > cell.Row) || (!cell.Pieces.IsPlayerOne && destRow < cell.Row))
                                {
                                    // Highlight the destination cell as available for a jump
                                    destCell.IsAvailable = true;
                                }
                            }
                        }
                    }
                }
            }
        }





        public void CellClicked(CheckerCell cell)
        {
            Console.WriteLine("Cell clicked");
            if (_selectedCell != null && cell.Pieces == null && cell.IsAvailable)
            {
                MovePiece(_selectedCell, cell);
                _selectedCell = null;

             
                foreach (var c in Cells)
                {
                    c.IsAvailable = false;
                }

                IsPlayerOneTurn = !IsPlayerOneTurn;
            }
            else if (cell.Pieces != null)
            {
                if (cell.Pieces.IsPlayerOne == IsPlayerOneTurn)
                {
                    foreach (var c in Cells)
                    {
                        c.IsAvailable = false;
                    }

                    _selectedCell = cell;

                    HighlightAvailableMoves(_selectedCell);
                }
            }

        }




        private void MovePiece(CheckerCell fromCell, CheckerCell toCell)
        {
            Console.WriteLine("Piece moved");
            if (toCell.IsAvailable)
            {
                toCell.Pieces = fromCell.Pieces;
                fromCell.Pieces = null;

                if (Math.Abs(fromCell.Row - toCell.Row) == 2)
                {
                    int jumpedRow = (fromCell.Row + toCell.Row) / 2;
                    int jumpedCol = (fromCell.Column + toCell.Column) / 2;
                    var jumpedCell = Cells[jumpedRow * 8 + jumpedCol];

                    if (jumpedCell.Pieces.IsPlayerOne)
                    {
                        PlayerOnePiecesCount--;
                    }
                    else
                    {
                        PlayerTwoPiecesCount--;
                    }
                    jumpedCell.Pieces = null;
                    CheckForWinCondition();
                }
            }
            else
            {
                Console.WriteLine("Invalid move. Please select an available cell.");
            }
        }


        public string CheckForWinCondition()
        {
            if (PlayerOnePiecesCount == 0)
            {
                Console.WriteLine("Player Two (Black) wins!");
                return "Player Two (Black) wins!";
            }
            else if (PlayerTwoPiecesCount == 0)
            {
                Console.WriteLine("Player One (Red) wins!");
                return "Player One (Red) wins!";
            }
            else
            {
                return null;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private int _playerOnePiecesCount = 12;
        public int PlayerOnePiecesCount
        {
            get { return _playerOnePiecesCount; }
            set
            {
                _playerOnePiecesCount = value;
                OnPropertyChanged(nameof(PlayerOnePiecesCount));
            }
        }

        private int _playerTwoPiecesCount = 12;
        public int PlayerTwoPiecesCount
        {
            get { return _playerTwoPiecesCount; }
            set
            {
                _playerTwoPiecesCount = value;
                OnPropertyChanged(nameof(PlayerTwoPiecesCount));
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


            public string SaveState()
            {
                // Include the cell information in the saved state
                var cellData = Cells.Select(cell => new { cell.Row, cell.Column, Pieces = cell.Pieces }).ToList();
                return JsonConvert.SerializeObject(cellData);
            }

            public static GameLogic LoadState(string savedState)
    {
        var cellStates = JsonConvert.DeserializeObject<List<dynamic>>(savedState);
        var gameLogic = new GameLogic();
        gameLogic.Cells.Clear();

        foreach (var cellState in cellStates)
        {
     
            Pieces pieces = JsonConvert.DeserializeObject<Pieces>(cellState.Pieces.ToString());


            var cell = new CheckerCell { Row = cellState.Row, Column = cellState.Column, Pieces = pieces };
            gameLogic.Cells.Add(cell);
        }

        return gameLogic;
    }


    }

}
