using System;

public interface IInputController
{
    Action<int, int> CellSelected { get; set; }
}
