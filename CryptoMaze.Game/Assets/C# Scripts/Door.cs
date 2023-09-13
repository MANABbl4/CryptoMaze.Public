using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private bool _initiallyOpened;

    [SerializeField]
    private Vector3 _changeRotation;

    [SerializeField]
    private Vector3 _changePosition;

    [SerializeField]
    private bool _opened;
    private Vector3 _initialRotation;
    private Vector3 _initialPosition;

    public void Switch()
    {
        _opened = !_opened;

        UpdateState();
    }

    private void Start()
    {
        _opened = _initiallyOpened;
        _initialRotation = gameObject.transform.rotation.eulerAngles;
        _initialPosition = gameObject.transform.position;

        UpdateState();
    }

    private void UpdateState()
    {
        if ((_initiallyOpened && _opened) ||
            (!_initiallyOpened && !_opened))
        {
            gameObject.transform.rotation = Quaternion.Euler(_initialRotation);
            gameObject.transform.position = _initialPosition;
        }
        else
        {
            gameObject.transform.Rotate(_changeRotation);
            gameObject.transform.position += _changePosition;
        }
    }
}
