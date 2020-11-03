using System;
using Random = UnityEngine.Random;

namespace BattleShip
{
    
    public class LocalPlayer : IPlayer
    {

        private int _healthPoint;

        private readonly IInputController _inputController;

        public string Name { get; }
        public IPlayerReceiver Model { get; set; }

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

        public CellState State { get; set; }

        public LocalPlayer(IInputController inputController, string name)
        {
            Name = name;
            _inputController = inputController ?? throw new NullReferenceException(nameof(inputController));
        }

        public void MakeTurn()
        {
            _inputController.CellSelected += SelectCell;
        }

        public void SetShips()
        {
            int shipCells = 0;

            shipCells += SetRandomShips(4, 1);
            shipCells += SetRandomShips(3, 2);
            shipCells += SetRandomShips(2, 3);
            shipCells += SetRandomShips(1, 4);

            _healthPoint = shipCells;
        }

        private int SetRandomShips(int size, int count)
        {
            for (int i = 0; i < count; i++)
            {
                int massLength = Model.Grid1.GetLength(1);

                bool shipPlaced = false;

                while (!shipPlaced)
                {
                    var x = Random.Range(0, massLength);
                    var y = Random.Range(0, massLength);
                    var isVertical = Random.Range(0, 2) != 0;

                    shipPlaced = Model.CreateShip(Model.Grid1, size, isVertical, x, y);
                }
            }
            return size * count;
        }

        private void SelectCell(int coordinateX, int coordinateY)
        {
            Model.MakeTurn(coordinateX, coordinateY);

            if (_inputController.CellSelected != null) _inputController.CellSelected -= SelectCell;
        }
    }
}