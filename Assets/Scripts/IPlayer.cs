
using UnityEngine;

public interface IPlayer
{

    CellState State { get; set; }

    void MakeTurn(IPlayerReceiver model);
}
