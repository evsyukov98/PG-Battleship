namespace BattleShip
{
    
    public interface IPlayerReceiver
    {

        CellState[,] Grid1 { get; }
        CellState[,] Grid2 { get; }

        void MakeTurn(int coordinateX, int coordinateY);

        bool CreateShip(CellState[,] grid, int size,
            bool isVertical, int coordinateX, int coordinateY);

        void WinnerFound(IPlayer player);
    }
}