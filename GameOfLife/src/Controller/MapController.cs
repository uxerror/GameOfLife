using GameOfLife.Model;
using GameOfLife.View;

namespace GameOfLife.Controller;

public class MapController()
{
    public delegate void UpdateMapViewAction(Cell[,] cellMap);
    public delegate void InitializationAction();
    public event UpdateMapViewAction? OnUpdateMapView; 
    public event InitializationAction? OnInitialization;
    
    private Grid _gridModel = new Grid();
    private MapView _mapView = new MapView();
    private Random _random = new Random();

    public void Initialization()
    {
        OnUpdateMapView+= _mapView.UpdateMapView;
        OnInitialization += _mapView.Initialization;
        _mapView.OnInitializeMapModel += InitializeMapModel;
        _mapView.OnInitializeInitialMap += InitializeInitialMap;
        _mapView.OnUpdateCellMap += UpdateCellMap;
    }

    private void InitializeMapModel(int lengthAbscissa, int lengthOrdinate)
    {
        _gridModel.Rows = lengthAbscissa;
        _gridModel.Cols = lengthOrdinate;
    }

    private void InitializeInitialMap(int state)
    {
        _gridModel.InitialCellMap = InitializeCellMap(_gridModel.Rows, _gridModel.Cols);
        if (state == 0)
        {
            InitializeRandomState(_gridModel.InitialCellMap);
            UpdateGeneralCellMap(_gridModel.InitialCellMap);
            OnUpdateMapView?.Invoke(_gridModel.CurrentCellMap);
        }

        if (state == 1)
        {
            UpdateGeneralCellMap(_gridModel.InitialCellMap);
            OnUpdateMapView?.Invoke(_gridModel.CurrentCellMap);
        }
    }

    public void Start()
    {
        OnInitialization?.Invoke();
        
    }

    private void UpdateCellMap(int generaions)
    {
        for (var i = 0; i < generaions; i++)
        {
            var nextGenerationCellMap= NextGeneration(_gridModel.CurrentCellMap);
            UpdateGeneralCellMap(nextGenerationCellMap);
            OnUpdateMapView?.Invoke(_gridModel.CurrentCellMap);
        }
    }

    private Cell[,] NextGeneration(Cell[,] cellMap)
    {
        var lengthAbscissa = cellMap.GetLength(0);
        var lengthOrdinate = cellMap.GetLength(1);
        var newCellMap = new Cell[lengthAbscissa, lengthOrdinate];

        for (var i = 0; i < lengthAbscissa; i++)
        {
            for (var j = 0; j < lengthOrdinate; j++)
            {
                var neighbors = CountNeighbors(i, j, cellMap);

                if (cellMap[i, j].IsAlive)
                {
                    newCellMap[i, j] = new Cell
                    {
                        IsAlive = neighbors == 2 || neighbors == 3
                    };
                }
                else
                {
                    newCellMap[i, j] = new Cell
                    {
                        IsAlive = neighbors == 3
                    };
                }
            }
        }

        return newCellMap;
    }

    private void UpdateGeneralCellMap(Cell[,] cellMap)
    {
        _gridModel.GenerationsCellMapList.Add(cellMap);
        _gridModel.CurrentCellMap = _gridModel.GenerationsCellMapList.Last();
    }

    private Cell[,] InitializeCellMap(int lengthAbscissa, int lengthOrdinate)
    {
        var cellMap = new Cell[lengthAbscissa, lengthOrdinate];
        for (var i = 0; i < lengthAbscissa; i++)
        {
            for (var j = 0; j < lengthOrdinate; j++)
            {
                cellMap[i, j] = new Cell
                {
                    IsAlive = false
                };
            }
        }
        return cellMap;
    }

    private void InitializeRandomState(Cell[,] cellMap)
    {
        var lengthAbscissa = cellMap.GetLength(0);
        var lengthOrdinate = cellMap.GetLength(1);
        for (var i = 0; i < lengthAbscissa; i++)
        {
            for (var j = 0; j < lengthOrdinate; j++)
            {
                var randomIndex = _random.Next(2) == 1;
                cellMap[i, j].IsAlive = randomIndex;
            }
        }
    }

    private void InitializeCustomState(int abscissaPosition, int ordinatePosition)
    {
        _gridModel.InitialCellMap[abscissaPosition, ordinatePosition].IsAlive = !_gridModel.InitialCellMap[abscissaPosition, ordinatePosition].IsAlive;
        OnUpdateMapView?.Invoke(_gridModel.InitialCellMap);
        
    }
    
    private int CountNeighbors(int x, int y, Cell[,] cellMap)
    {
        var count = 0;
        var lengthAbscissa = cellMap.GetLength(0);
        var lengthOrdinate = cellMap.GetLength(1);
        
        for (var i = x - 1; i <= x + 1; i++)
        {
            for (var j = y - 1; j <= y + 1; j++)
            {
                var withinLengthAbscissa = i >= 0 && i < lengthAbscissa;
                var withinLengthOrdinate = j >= 0 && j < lengthOrdinate;
                var isCenter = i == x && j == y;
                
                if (withinLengthAbscissa && withinLengthOrdinate && !isCenter)
                {
                    if (cellMap[i, j].IsAlive)
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