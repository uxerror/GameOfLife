using GameOfLife.Model;
using GameOfLife.View;

namespace GameOfLife.Controller;

public class MapController()
{
    public delegate void UpdateGridViewAction(Cell[,] cellGrid);
    public delegate void InitializationAction();
    public event UpdateGridViewAction? OnUpdateMapView; 
    public event InitializationAction? OnInitialization;
    
    private Grid _gridModel = new Grid();
    private GridView _gridView = new GridView();
    private Random _random = new Random();

    public void Initialization()
    {
        OnUpdateMapView+= _gridView.UpdateGridView;
        OnInitialization += _gridView.Initialization;
        _gridView.OnInitializeGridModel += InitializeGridModel;
        _gridView.OnInitializeInitialGrid += InitializeInitialGrid;
        _gridView.OnUpdateCellGrid += UpdateCellGrid;
    }

    private void InitializeGridModel(int rows, int cols)
    {
        _gridModel.Rows = rows;
        _gridModel.Cols = cols;
    }

    private void InitializeInitialGrid(int state)
    {
        _gridModel.InitialCellGrid = InitializeCellGrid(_gridModel.Rows, _gridModel.Cols);
        if (state == 0)
        {
            InitializeRandomState(_gridModel.InitialCellGrid);
            UpdateCurrentCellGrid(_gridModel.InitialCellGrid);
            OnUpdateMapView?.Invoke(_gridModel.CurrentCellGrid);
        }

        if (state == 1)
        {
            UpdateCurrentCellGrid(_gridModel.InitialCellGrid);
            OnUpdateMapView?.Invoke(_gridModel.CurrentCellGrid);
        }
    }

    public void Start()
    {
        OnInitialization?.Invoke();
        
    }

    private void UpdateCellGrid()
    {
        var nextGenerationCellGrid= NextGeneration(_gridModel.CurrentCellGrid); 
        UpdateCurrentCellGrid(nextGenerationCellGrid); 
        OnUpdateMapView?.Invoke(_gridModel.CurrentCellGrid);
    }

    private Cell[,] NextGeneration(Cell[,] cellGrid)
    {
        var rows = cellGrid.GetLength(0);
        var cols = cellGrid.GetLength(1);
        var newCellGrid = new Cell[rows, cols];

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                var neighbors = CountAliveNeighbors(i, j, cellGrid);

                if (cellGrid[i, j].IsAlive)
                {
                    newCellGrid[i, j] = new Cell
                    {
                        IsAlive = neighbors == 2 || neighbors == 3
                    };
                }
                else
                {
                    newCellGrid[i, j] = new Cell
                    {
                        IsAlive = neighbors == 3
                    };
                }
            }
        }

        return newCellGrid;
    }

    private void PreviousGeneration()
    {
        
    }

    private void UpdateCurrentCellGrid(Cell[,] cellGrid)
    {
        _gridModel.GenerationsCellGridList.Add(cellGrid);
        _gridModel.CurrentCellGrid = _gridModel.GenerationsCellGridList.Last();
    }

    private Cell[,] InitializeCellGrid(int rows, int cols)
    {
        var cellGrid = new Cell[rows, cols];
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                cellGrid[i, j] = new Cell
                {
                    IsAlive = false
                };
            }
        }
        return cellGrid;
    }

    private void InitializeRandomState(Cell[,] cellGrid)
    {
        var rows = cellGrid.GetLength(0);
        var cols = cellGrid.GetLength(1);
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                var randomIndex = _random.Next(2) == 1;
                cellGrid[i, j].IsAlive = randomIndex;
            }
        }
    }

    private void InitializeCustomState(int x, int y)
    {
        _gridModel.InitialCellGrid[x, y].IsAlive = !_gridModel.InitialCellGrid[x, y].IsAlive;
        OnUpdateMapView?.Invoke(_gridModel.InitialCellGrid);
        
    }

    public void SetGridCell(int x, int y, bool isAlive, Cell[,] cellGrid)
    {
        cellGrid[x, y].IsAlive = isAlive;
    }
    
    private int CountAliveNeighbors(int x, int y, Cell[,] cellGrid)
    {
        var count = 0;
        var rows = cellGrid.GetLength(0);
        var cols = cellGrid.GetLength(1);
        
        for (var i = x - 1; i <= x + 1; i++)
        {
            for (var j = y - 1; j <= y + 1; j++)
            {
                var withinLengthAbscissa = i >= 0 && i < rows;
                var withinLengthOrdinate = j >= 0 && j < cols;
                var isCenter = i == x && j == y;
                
                if (withinLengthAbscissa && withinLengthOrdinate && !isCenter)
                {
                    if (cellGrid[i, j].IsAlive)
                    {
                        count++;
                    }
                }
            }
        }

        return count;
    }
    
}

public enum State
{
    Random = 0
}