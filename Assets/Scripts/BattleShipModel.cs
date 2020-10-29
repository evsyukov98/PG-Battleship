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
    public event Action<bool, CellState, int, int> PlayerMadeTurn;
    public event Action<CellState> WinnerFound;
    
    public void StartBattle(IPlayer player1, IPlayer player2)
    {
        if (IsGameStarted) return;

        _player1 = player1 ?? throw new NullReferenceException(nameof(player1));
        _player2 = player2 ?? throw new NullReferenceException(nameof(player2));
        
        _activePlayer = _player1;
        IsGameStarted = true;

        _activePlayer.MakeTurn(this);
    }

    public void MakeTurn(int coordinateX, int coordinateY)
    {
        Debug.Log(coordinateX+""+coordinateY);
        SetState(coordinateX, coordinateY);
        
        _activePlayer = _activePlayer == _player1 ? _player2 : _player1;
        
        if (_isGameRunning) _activePlayer.MakeTurn(this);
    }
    
    
    private void SetState(int coordinateX, int coordinateY)
    {
        bool isPlayer1;
        
        if (_activePlayer == _player1)
        {
            isPlayer1 = true;
            Grid1[coordinateX,coordinateY] = CellState.Hit;
            // TODO: CellState.hit надо поменять на попытаться уничтожить корабль
            // TODO: (короче логика должна быть)
        }
        else
        {
            isPlayer1 = false;
            Grid2[coordinateX, coordinateY] = CellState.Hit;
        }
        
        PlayerMadeTurn?.Invoke(isPlayer1, CellState.Hit, coordinateX, coordinateY);

    }
    
}
