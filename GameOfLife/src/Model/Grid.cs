using System.Numerics;

namespace GameOfLife.Model;

public class Grid
{
    public int Rows;
    public int Cols;
    public Cell[,] InitialCellGrid;
    public Cell[,] CurrentCellGrid;
    public List<Cell[,]> GenerationsCellGridList = [];
    public List<int[,]> InitialStateList;
}