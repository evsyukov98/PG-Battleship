using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleShipPresenter : MonoBehaviour, IInputController
{
    [SerializeField] private Button hotSeatButton = default;
    [SerializeField] private Button withAIButton = default;

    [SerializeField] private GameObject grid1;
    [SerializeField] private GameObject grid2;
    
    private Dictionary<Vector2, CellController> _cellControllers1;
    private Dictionary<Vector2, CellController> _cellControllers2;
    
    private readonly BattleShipModel _model = new BattleShipModel();
    
    public Action<int, int> CellSelected { get; set; }
    
    private void Awake()
    {
        _cellControllers1 = new Dictionary<Vector2, CellController>();
        _cellControllers2 = new Dictionary<Vector2, CellController>();
    }

    private void Start()
    {
        _model.PlayerMadeTurn += OnPlayerMadeTurn;

        hotSeatButton.onClick.AddListener(StartHotSeatGame);
        
        SetupCells();
    }

    private void StartHotSeatGame()
    {
        IPlayer player1 = new LocalPlayer(this);
        IPlayer player2 = new LocalPlayer(this);

        _model.StartBattle(player1,player2);
        
    }
    
    private void OnPlayerMadeTurn(bool isPLayer1, CellState state, int coordinateX, int coordinateY)
    {
        if (isPLayer1)
        {
            _cellControllers1[new Vector2(coordinateX,coordinateY)].CellStateChange(state);
        }
        else
        {
            _cellControllers2[new Vector2(coordinateX,coordinateY)].CellStateChange(state);
        }
    }

    private void SetupCells()
    {
        var cellControllersMass1 = grid1.GetComponentsInChildren<CellController>();
        var cellControllersMass2 = grid2.GetComponentsInChildren<CellController>();

        foreach (var cell in cellControllersMass1)
        {
            _cellControllers1.Add(cell.coordinate, cell);
            cell.CellSelected += OnCellSelected;
        }
        foreach (var cell in cellControllersMass2)
        {
            _cellControllers2.Add(cell.coordinate, cell);
            cell.CellSelected += OnCellSelected;
        }
    }
    
    private void OnCellSelected(Vector2 coordinate)
    {
        CellSelected?.Invoke((int)coordinate.x ,(int)coordinate.y);
    }

}
