using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class CellController : MonoBehaviour
{
    
    [SerializeField] public Vector2 coordinate;
    
    [SerializeField] private Sprite hit = default;
    [SerializeField] private Sprite miss = default;
    
    public event Action<Vector2> CellSelected;
    
    private Image _image;
    private Button _button;
    
    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();

        _button.onClick.AddListener(OnButtonClick);
    }
    
    private void OnButtonClick()
    {
        CellSelected?.Invoke(coordinate);
    }
    
    public void CellStateChange(CellState state)
    {
        if (state == CellState.Hit)
        {
            _image.sprite = hit;
            _button.enabled = false;
        }
        else
        {
            _image.sprite = miss;
            _button.enabled = false;
        }
    }

    public void SetActive(bool active)
    {
        _button.interactable = active;
    }
}
