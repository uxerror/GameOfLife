namespace GameOfLife.View;

public class CanvasView
{
    public void Draw()
    {
        Console.Clear();
        var windowWidth = Console.WindowWidth;
        var windowHeight = Console.WindowHeight;
        
        for (int i = 0; i < windowHeight; i++)
        {
            for (int j = 0; j < windowWidth; j++)
            {
                Console.Write(' ');
            }
        }
        Console.ReadKey();
    }

    public void Update()
    {
        while (true)
        {
            Draw();
        }
    }
}