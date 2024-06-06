using GameOfLife.Model;

namespace GameOfLife.View;

public class MapView()
{
    public delegate void InitializeMapModelAction(int lengthAbscissa, int lengthOrdinate);
    public delegate void InitializeInitialMapAction(int state);
    public delegate void UpdateCellMapAction(int generations);
    public delegate void InitializeCustomStateAction(int abscissaPosition, int ordinatePosition);
    public event InitializeMapModelAction? OnInitializeMapModel;
    public event InitializeInitialMapAction? OnInitializeInitialMap;
    public event UpdateCellMapAction? OnUpdateCellMap;
    public event InitializeCustomStateAction? OnInitializeCustomState;
    public void Initialization()
    {
        var windowWidth = Console.WindowWidth;
        var windowHeight = Console.WindowHeight;
        var state = MenuView();
        Console.Write($"Введите значение длины карты по абсциссе({windowWidth}): ");
        var lengthAbscissa = Convert.ToInt32(Console.ReadLine());
        Console.Write($"Введите значение длины карты по ординате({windowHeight}): ");
        var lengthOrdinate = Convert.ToInt32(Console.ReadLine());
        Console.Write("Введите значение колличества поколений: ");
        var generations = Convert.ToInt32(Console.ReadLine());
        OnInitializeMapModel?.Invoke(lengthAbscissa, lengthOrdinate);
        if (state == 0)
        {
            OnInitializeInitialMap?.Invoke(state);
            OnUpdateCellMap?.Invoke(generations);
        }
        if (state == 1)
        {
            OnInitializeInitialMap?.Invoke(state);
            CustomCellMapView();
            OnUpdateCellMap?.Invoke(generations); 
        }
    }

    public void UpdateMapView(Cell[,] cellMap)
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

    private void CustomCellMapView()
    {
        var abscissaPosition = 0;
        var ordinatePosition = 0;

        while (true)
        {
            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
            if (consoleKeyInfo.Key == ConsoleKey.UpArrow)
            {
                abscissaPosition++;
                OnInitializeCustomState?.Invoke(ordinatePosition, abscissaPosition);
            }
            if (consoleKeyInfo.Key == ConsoleKey.DownArrow)
            {
                abscissaPosition--;
                OnInitializeCustomState?.Invoke(ordinatePosition, abscissaPosition);
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