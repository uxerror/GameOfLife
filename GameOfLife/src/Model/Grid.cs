using System.Numerics;

namespace GameOfLife.Model;

public class Grid
{
    public int Rows;
    public int Cols;
    public Cell[,] InitialCellMap;
    public Cell[,] CurrentCellMap;
    public List<Cell[,]> GenerationsCellMapList = [];
    public List<int[,]> InitialStateList;
}