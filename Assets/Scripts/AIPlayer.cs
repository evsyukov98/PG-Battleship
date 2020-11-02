using UnityEngine;

public class AIPlayer : IPlayer
{
    private int _healthPoint;

    public string Name { get; }
    public IPlayerReceiver Model { get; set; }
    
    public CellState State { get; set; }

    public int HealthPoint
    {
        get => _healthPoint;
        set
        {
            _healthPoint = value;
            
            if (_healthPoint <= 0)
            {
                Model.WinnerFound(this);
            }
        }
    }

    public AIPlayer(string name)
    {
        Name = name;
    }
    public void SetShips()
    {
        int shipCells = 0;
        
        shipCells += SetRandomShips(4,1);
        
        shipCells += SetRandomShips(3,2);
        
        shipCells += SetRandomShips(2,3);

        shipCells += SetRandomShips(1,4);

        _healthPoint = shipCells;
    }
    public void MakeTurn()
    {
        AISelectCell(Model, out var x, out var y);

        Model.MakeTurn(x,y);
    }

    private int SetRandomShips(int size, int count)
    {
        for (int i = 0; i < count; i++)
        {
            int massLength = Model.Grid2.GetLength(1);

            bool shipPlaced = false;

            while (!shipPlaced)
            {
                var x = Random.Range(0, massLength);
                var y = Random.Range(0, massLength);
                var isVertical = Random.Range(0, 2) != 0;

                shipPlaced = Model.CreateShip(Model.Grid2, size, isVertical, x, y);
            }
        }

        return size * count;
    }
    
    private void AISelectCell(IPlayerReceiver model, out int x, out int y)
    {
        var massLength = model.Grid1.GetLength(1);
            
        x = Random.Range(0, massLength); 
        y = Random.Range(0, massLength);
            
        while (model.Grid1[x,y] == CellState.Hit ||
               model.Grid1[x,y] == CellState.Miss) 
        { 
            x = Random.Range(0, massLength); 
            y = Random.Range(0, massLength);
        }
    }
}
