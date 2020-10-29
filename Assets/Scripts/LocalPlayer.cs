using System;

public class LocalPlayer : IPlayer
{

    private IPlayerReceiver _model;
    private IInputController _inputController;
    
    public CellState State { get; set; }
    
    public LocalPlayer(IInputController inputController)
    {
        _inputController = inputController ?? throw new NullReferenceException(nameof(inputController));
    }
    public void MakeTurn(IPlayerReceiver model)
    {
        _model = model;
        _inputController.CellSelected += SelectCell;
    }
    
    private void SelectCell(int coordinateX, int coordinateY)
    {
        _model.MakeTurn(coordinateX, coordinateY);

        if (_inputController.CellSelected != null) _inputController.CellSelected -= SelectCell;
    }
}
