using UnityEngine;

public class AIPlayer : IPlayer
{
    
    public CellState State { get; set; }
    public void MakeTurn(IPlayerReceiver model)
    {
        AISelectCell(model, out var x, out var y);

        model.MakeTurn(x,y);
    }

    public void SetShips(IPlayerReceiver model)
    {
        SetRandomShip(model,4);
        
        SetRandomShip(model,3);
        SetRandomShip(model,3);
        
        SetRandomShip(model,2);
        SetRandomShip(model,2);
        SetRandomShip(model,2);
        
        SetRandomShip(model,1);
        SetRandomShip(model,1);
        SetRandomShip(model,1);
        SetRandomShip(model,1);


    }
    
    public void SetRandomShip(IPlayerReceiver model, int size)
    {
        int massLength = model.Grid2.GetLength(1);

        bool shipPlaced = false;
        
        while (!shipPlaced)
        {
            var x = Random.Range(0, massLength); 
            var y = Random.Range(0, massLength);
            var isVertical = Random.Range(0, 2) != 0;
            
            shipPlaced = model.CreateShip(model.Grid2, size, isVertical, x, y);
        }
    }
    
    private void AISelectCell(IPlayerReceiver model, out int x, out int y)
    {
        var massLength = model.Grid1.GetLength(1);
            
        x = Random.Range(0, massLength); 
        y = Random.Range(0, massLength);
            
        while (model.Grid1[x,y] != CellState.None && 
               model.Grid1[x,y] != CellState.Ship) 
        { 
            x = Random.Range(0, massLength); 
            y = Random.Range(0, massLength);
        }
    }
}
