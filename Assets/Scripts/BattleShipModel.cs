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
        
        _player1.SetShips(this);
        _player2.SetShips(this);

        _activePlayer.MakeTurn(this);
    }

    void IPlayerReceiver.MakeTurn(int coordinateX, int coordinateY)
    {
        SetState(coordinateX, coordinateY, IsPlayer1 ? Grid2 : Grid1);

        _activePlayer = _activePlayer == _player1 ? _player2 : _player1;
        
        if (_isGameRunning) _activePlayer.MakeTurn(this);
    }

    bool IPlayerReceiver.CreateShip(CellState[,] grid, int size, 
        bool isVertical, int coordinateX, int coordinateY)
    {
        if (isVertical)
        {
            if (coordinateX + size - 1 > 9)
            {
                return false;
            }

            for (int i = 0; i < size; i++)
            {
                if (grid[coordinateX + i, coordinateY] == CellState.NearShip || 
                    grid[coordinateX + i, coordinateY] == CellState.Ship)
                {
                    return false;
                }
            }
                
            MarksNearShip(grid,size,true,coordinateX,coordinateY);
            for (int i = 0; i < size; i++)
            {
                grid[coordinateX + i, coordinateY] = CellState.Ship;
            }
        }
        else
        {
            if (coordinateY + size - 1 > 9)
            {
                return false;
            }
                
            for (int i = 0; i < size; i++)
            {
                if (grid[coordinateX, coordinateY + i] == CellState.NearShip 
                    || grid[coordinateX, coordinateY + i] == CellState.Ship)
                {
                    return false;
                }
            }

            MarksNearShip(grid,size,false,coordinateX,coordinateY);
            for (int i = 0; i < size; i++)
            {
                grid[coordinateX, coordinateY + i] = CellState.Ship;
            }
        }
        return true;
    }
    
    private void MarksNearShip(CellState[,] grid, int size, 
        bool isVertical, int coordinateX, int coordinateY)
    {
        
        var offsetX = 1;
        var offsetY = 1;
                        
        var massSizeY = 0;
        var massSizeX = 0;

        switch (coordinateX)
        {
            case 0:
                offsetX = 0;
                massSizeX = -1;
                break;
            case 9:
                massSizeX = -1;
                break;
        }

        switch (coordinateY)
        {
            case 0:
                offsetY = 0;
                massSizeY = -1;
                break;
            case 9:
                massSizeY = -1;
                break;
        }

        if (isVertical)
        {
            if (coordinateX + size - 1 >= 9) massSizeX = -1;
            
            for (int i = 0; i < size + 2 + massSizeX; i++)
            {
                for (int j = 0; j < 3 + massSizeY; j++)
                {
                    if(grid[coordinateX - offsetX + i, coordinateY - offsetY + j] 
                       == CellState.Ship) continue;
                    
                    grid[coordinateX - offsetX + i, coordinateY - offsetY + j] =
                        CellState.NearShip;
                }
            }
        }
        else
        {
            if (coordinateY + size - 1 >= 9) massSizeY = -1;
            
            for (int i = 0; i < 3 + massSizeX; i++)
            {
                for (int j = 0; j < size + 2 + massSizeY; j++)
                {
                    if(grid[coordinateX - offsetX + i, coordinateY - offsetY + j] 
                       == CellState.Ship) continue;
                    
                    grid[coordinateX - offsetX + i, coordinateY - offsetY + j] =
                        CellState.NearShip;
                }
            }
        }
    }
    
    private void SetState(int coordinateX, int coordinateY, CellState[,] grid)
    {
        var state = grid[coordinateX, coordinateY] == CellState.Ship ? CellState.Hit : CellState.Miss;

        grid[coordinateX, coordinateY] = state;

        PlayerMadeTurn?.Invoke(state, coordinateX, coordinateY);
    }
}
