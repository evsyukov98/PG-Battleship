
public interface IPlayerReceiver
{
    CellState[,] Grid1 { get; }
    CellState[,] Grid2 { get; }
    
    void MakeTurn(int coordinateX, int coordinateY);

}
