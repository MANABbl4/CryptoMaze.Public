using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField]
    private Image _backgroundImage;
    [SerializeField]
    private Image _joystickImage;

    private Vector2 _inputVector;// полученные координаты джойстика
    private bool _keyReleased = true;

    public virtual void OnPointerDown(PointerEventData pointerData)
    {
        OnDrag(pointerData);
    }

    public virtual void OnPointerUp(PointerEventData pointerData)
    {
        _inputVector = Vector2.zero;
        _joystickImage.rectTransform.anchoredPosition = Vector2.zero;// возврат джойстика в центр    
    }

    public virtual void OnDrag(PointerEventData pointerData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_backgroundImage.rectTransform, pointerData.position, pointerData.pressEventCamera, out var localPoint))
        {
            var pos = new Vector2(localPoint.x / _backgroundImage.rectTransform.sizeDelta.x, localPoint.y / _backgroundImage.rectTransform.sizeDelta.y);

            _inputVector = pos.normalized;

            if (localPoint.sqrMagnitude > _backgroundImage.rectTransform.sizeDelta.x * _backgroundImage.rectTransform.sizeDelta.x * 0.25f)
            {
                localPoint = localPoint.normalized * _backgroundImage.rectTransform.sizeDelta.x * 0.5f;
            }

            _joystickImage.rectTransform.anchoredPosition = new Vector2(localPoint.x, localPoint.y);
        }
    }

    public Vector2 GetDirection()
    {
        return _inputVector;
    }

    private void Start()
    {
        _backgroundImage = GetComponent<Image>();
        _joystickImage = transform.GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        var userInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.anyKey)
        {
            if (!Input.GetMouseButton(0))
            {
                _keyReleased = false;
                _inputVector = userInput.normalized;
            }
        }
        else if (!_keyReleased)
        {
            _keyReleased = true;
            _inputVector = Vector2.zero;
        }
    }
}
