using System;
using UnityEngine;

public class BattleShipModel : IPlayerReceiver
{
    private IPlayer _player1;
    private IPlayer _player2;
    
    private IPlayer _activePlayer;
    
    private bool _isGameRunning;

    public CellState[,] Grid1 { get; } = new CellState[10, 10];
    public CellState[,] Grid2 { get; } = new CellState[10, 10];

    public bool IsPlayer1 => _activePlayer == _player1;

    public bool IsGameStarted
    {
        get => _isGameRunning;
        private set
        {
            if (_isGameRunning == value) return;
            _isGameRunning = value;
            GameStatusChanged?.Invoke();
        }
    }
    
    public event Action GameStatusChanged;
    public event Action<CellState, int, int> PlayerMadeTurn;
    public event Action<CellState> WinnerFound;
    
    public void StartBattle(IPlayer player1, IPlayer player2)
    {
        if (IsGameStarted) return;

        _player1 = player1 ?? throw new NullReferenceException(nameof(player1));
        _player2 = player2 ?? throw new NullReferenceException(nameof(player2));
        
        _activePlayer = _player1;
        IsGameStarted = true;

        GenerateShip(Grid2,3,true,0,0);
        //
        
        _activePlayer.MakeTurn(this);
    }

    public void MakeTurn(int coordinateX, int coordinateY)
    {
        SetState(coordinateX, coordinateY, IsPlayer1 ? Grid2 : Grid1);

        _activePlayer = _activePlayer == _player1 ? _player2 : _player1;
        
        if (_isGameRunning) _activePlayer.MakeTurn(this);
    }
    
    private void SetState(int coordinateX, int coordinateY, CellState[,] grid)
    {
        var state = grid[coordinateX, coordinateY] == CellState.Ship ? CellState.Hit : CellState.Miss;

        grid[coordinateX, coordinateY] = state;

        PlayerMadeTurn?.Invoke(state, coordinateX, coordinateY);
    }

    // Строит вниз или в право, относительно начальной точки
    private void GenerateShip(CellState[,] grid, int size, 
        bool isVertical, int coordinateX, int coordinateY)
    {
        for (int i = 0; i < size; i++)
        {
            if (isVertical)
            {
                grid[coordinateX + i, coordinateY] = CellState.Ship;
            }
            else
            {
                grid[coordinateX, coordinateY + i] = CellState.Ship;
            }
        }
    }
    
    
    
}
