using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite pressedSprite;

    private Image _image;
    private RectTransform _icon;

    void Awake()
    {
        _image = GetComponent<Image>();
        foreach (Transform child in transform)
        {
            _icon = child.GetComponent<RectTransform>();
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_image != null && pressedSprite != null)
            _image.sprite = pressedSprite;

        Vector2 anchoredPos = _icon.anchoredPosition;
        anchoredPos.y = -10f;
        _icon.anchoredPosition = anchoredPos;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_image != null && normalSprite != null)
            _image.sprite = normalSprite;

        Vector2 anchoredPos = _icon.anchoredPosition;
        anchoredPos.y = 11.7f;
        _icon.anchoredPosition = anchoredPos;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_image != null && normalSprite != null)
            _image.sprite = normalSprite;
    }
}
