using UnityEngine;

public class AIPlayer : IPlayer
{
    public CellState State { get; set; }
    public void MakeTurn(IPlayerReceiver model)
    {
        AiSelectCell(model, out var x, out var y);

        model.MakeTurn(x,y);
    }
    
    private void AiSelectCell(IPlayerReceiver model, out int x, out int y)
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
