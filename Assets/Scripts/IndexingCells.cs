using UnityEngine;

public class IndexingCells : MonoBehaviour
{
    private CellController[] _cellController;

    private void Awake()
    {
        _cellController = GetComponentsInChildren<CellController>();
        
        IndexAllCells();
    }

    private void IndexAllCells()
    {
        var l = 0;
        
        for (var i = 0; i < 10; i++)
        {
            for (var j = 0; j < 10; j++)
            {
                _cellController[l].coordinate = new Vector2(i, j);
                l++;
            }
        }
    }
}
