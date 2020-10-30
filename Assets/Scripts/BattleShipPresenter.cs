using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleShipPresenter : MonoBehaviour, IInputController
{
    [SerializeField] private Button withAIButton = default;

    [SerializeField] private GameObject grid1 = default;
    [SerializeField] private GameObject grid2 = default;
    
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

        withAIButton.onClick.AddListener(StartGameWithAi);
        
        SetupCells();
    }

    private void StartGameWithAi()
    {
        IPlayer player1 = new LocalPlayer(this);
        IPlayer player2 = new AIPlayer();

        _model.StartBattle(player1,player2);
        
    }
    
    private void OnPlayerMadeTurn(CellState state, int coordinateX, int coordinateY)
    {
        if (_model.IsPlayer1)
        {
            _cellControllers2[new Vector2(coordinateX,coordinateY)].CellStateChange(state);
        }
        else
        {
            _cellControllers1[new Vector2(coordinateX,coordinateY)].CellStateChange(state);
        }
    }

    private void SetupCells()
    {
        var cellControllersMass1 = grid1.GetComponentsInChildren<CellController>();
        var cellControllersMass2 = grid2.GetComponentsInChildren<CellController>();

        foreach (var cell in cellControllersMass1)
        {
            _cellControllers1.Add(cell.coordinate, cell);
            //cell.CellSelected += OnCellSelected;
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
