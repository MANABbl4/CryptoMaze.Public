using System.Collections.Generic;
using UnityEngine;

public class DoorSwitcher : MonoBehaviour
{
    [SerializeField]
    private List<Door> _doors;

    [SerializeField]
    private GameObject _redButton;

    [SerializeField]
    private GameObject _greenButton;

    [SerializeField]
    private bool _buttonPressed;

    [SerializeField]
    private bool _oneTimeSwitcher;

    private bool _switched = false;

    private void Start()
    {
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        _redButton.gameObject.SetActive(!_buttonPressed);
        _greenButton.gameObject.SetActive(_buttonPressed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_oneTimeSwitcher || !_switched)
        {
            if (other.CompareTag("Player"))
            {
                _buttonPressed = !_buttonPressed;
                _switched = true;

                UpdateButtons();

                foreach (var door in _doors)
                {
                    door.Switch();
                }
            }
        }
    }
}
