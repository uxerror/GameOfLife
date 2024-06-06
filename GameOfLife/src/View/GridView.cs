using GameOfLife.Model;

namespace GameOfLife.View;

public class GridView()
{
    public delegate void InitializeGridModelAction(int rows, int cols);
    public delegate void InitializeInitialGridAction(int state);
    public delegate void UpdateCellGridAction();
    public delegate void InitializeCustomStateAction(int rows, int cols);
    public event InitializeGridModelAction? OnInitializeGridModel;
    public event InitializeInitialGridAction? OnInitializeInitialGrid;
    public event UpdateCellGridAction? OnUpdateCellGrid;
    public event InitializeCustomStateAction? OnInitializeCustomState;
    public void Initialization()
    {
        var windowWidth = Console.WindowWidth;
        var windowHeight = Console.WindowHeight;
        var state = MenuView();
        Console.Write($"Введите значение длины карты по оси y({windowHeight}): ");
        var rows = Convert.ToInt32(Console.ReadLine());
        Console.Write($"Введите значение длины карты по оси x({windowWidth % 2}): ");
        var cols = Convert.ToInt32(Console.ReadLine());
        Console.Write("Введите значение колличества поколений: ");
        var generations = Convert.ToInt32(Console.ReadLine());
        OnInitializeGridModel?.Invoke(rows, cols);
        if (state == 0)
        {
            OnInitializeInitialGrid?.Invoke(state);
        }
        if (state == 1)
        {
            OnInitializeInitialGrid?.Invoke(state);
            CustomCellGridView();
        }

        Update();
    }

    public void Update()
    {
        while (true)
        {
            Console.ReadKey();
            OnUpdateCellGrid?.Invoke();
        }
    }

    public void UpdateGridView(Cell[,] cellMap)
    {
        var lengthAbscissa = cellMap.GetLength(0);
        var lengthOrdinate = cellMap.GetLength(1);
        
        Thread.Sleep(500);
        Console.Clear();
        for (int i = 0; i < lengthAbscissa; i++)
        {
            for (int j = 0; j < lengthOrdinate; j++)
            {
                Console.Write(cellMap[i, j].IsAlive ? "\u2593 " : "\u2591 ");
            }
            Console.WriteLine();
        }
    }

    private void CustomCellGridView()
    {
        var r = 0;
        var y = 0;

        while (true)
        {
            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
            if (consoleKeyInfo.Key == ConsoleKey.UpArrow)
            {
                r++;
                OnInitializeCustomState?.Invoke(y, r);
            }
            if (consoleKeyInfo.Key == ConsoleKey.DownArrow)
            {
                r--;
                OnInitializeCustomState?.Invoke(y, r);
            }

            if (consoleKeyInfo.Key == ConsoleKey.Enter)
            {
                break;
            }
        }
    }
    
    private int MenuView()
    {
        var windowWidth = Console.WindowWidth;
        var markerPosition = 0;
        char[] markerList = [' ', ' '];
        string[] lenList =
        [
            "Случайное начальное состояние", "Пользовательское начальное состояние"
        ];
        while (true)
        {
            for (int i = 0; i < markerList.Length - 1; i++)
            {
                if (i == markerPosition)
                {
                    markerList[i] = '>';
                }
                else
                {
                    markerList[i] = ' ';
                }
            }

            for (int i = 0; i < lenList.Length - 1; i++)
            {
                for (int j = 0; j < (windowWidth / 2) - 30; j++)
                {
                    Console.Write(' ');
                }

                Console.WriteLine($"{markerList[i]} {lenList[i]}");
            }

            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
            if (consoleKeyInfo.Key == ConsoleKey.UpArrow)
            {
                if (markerPosition > 0)
                {
                    markerPosition--;
                }
            }

            if (consoleKeyInfo.Key == ConsoleKey.DownArrow)
            {
                if (markerPosition < markerList.Length - 2)
                {
                    markerPosition++;
                }
            }
            
            if (consoleKeyInfo.Key == ConsoleKey.Enter)
            {
                break;
            }
            Console.Clear();
        }
        Console.Clear();
        return markerPosition;
    }
}