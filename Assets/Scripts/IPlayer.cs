public interface IPlayer
{
    IPlayerReceiver Model { get; set; }
    string Name { get; }

    int HealthPoint { get; set; }
    CellState State { get; set; }

    void MakeTurn();

    void SetShips();
}
