using GameOfLife.Model;
using GameOfLife.View;

namespace GameOfLife.Controller;

public class CanvasController
{
    private Canvas _canvasModel = new Canvas();
    private CanvasView _canvasView = new CanvasView();

    public void Ready()
    {
        
    }
    
    public void InitializeCanvasModel(int windowWidth, int windowHeight)
    {
        _canvasModel.windowHeight = windowHeight;
        _canvasModel.windowWidth = windowWidth;
    }
    
    private char[,] InitializeCanvas(int windowWidth, int windowHeight)
    {
        var canvas = new char[windowWidth, windowHeight];
        
        for (var i = 0; i < windowHeight; i++)
        {
            for (var j = 0; j < windowWidth; j++)
            {
                canvas[i, j] = ' ';
            }
        }
        return canvas;
    }
}