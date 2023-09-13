using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Action<GameObject> OnBlockCollected;
    public Action<GameObject> OnCryptoKeyFragmentCollected;
    public Action<GameObject> OnEnergyCollected;
    public Action<GameObject> OnStorage;
    public Action<GameObject> OnExit;
    public Action<GameObject> OnButtonRed;

    [SerializeField]
    private float _baseSpeed = 10f;

    [SerializeField]
    private float _rocketSpeed = 17f;

    [SerializeField]
    private MobileController _inputController;

    [SerializeField]
    private Animator _animator;

    private float _curSpeed = 0f;
    private float _gravitySpeed = 0f;
    private CharacterController _characterController;
    private bool _moving = true;

    private const float _gravity = -10f;

    public void ActivateRocketSpeed()
    {
        _curSpeed = _rocketSpeed;
    }

    public void StopMoving()
    {
        _moving = false;
        _animator.SetBool("Move", false);
    }

    private void Start()
    {
        _curSpeed = _baseSpeed;
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_moving)
        {
            var inputDirection = _inputController.GetDirection();
            var moveDirection = new Vector3(inputDirection.x, 0f, inputDirection.y);
            var speedVector = moveDirection * _curSpeed;

            _animator.SetBool("Move", moveDirection.sqrMagnitude > 0.0001f);

            if (moveDirection.sqrMagnitude > 0.0001f)
            {
                var rotateDirection = Vector3.RotateTowards(transform.forward, moveDirection, _curSpeed, 0.0f);
                transform.rotation = Quaternion.LookRotation(rotateDirection);
            }

            if (!_characterController.isGrounded)
            {
                _gravitySpeed += _gravity * Time.deltaTime;
            }
            else
            {
                _gravitySpeed = 0f;
            }

            speedVector.y = _gravitySpeed;

            _characterController.Move(speedVector * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block"))
        {
            OnBlockCollected?.Invoke(other.gameObject);
        }
        else if (other.CompareTag("FragmentKey"))
        {
            OnCryptoKeyFragmentCollected?.Invoke(other.gameObject);
        }
        else if (other.CompareTag("Battary"))
        {
            OnEnergyCollected?.Invoke(other.gameObject);
        }
        else if (other.CompareTag("Storage"))
        {
            OnStorage?.Invoke(other.gameObject);
        }
        else if (other.CompareTag("Exit"))
        {
            OnExit?.Invoke(other.gameObject);
        }
        else if (other.CompareTag("ButtonRed"))
        {
            OnButtonRed?.Invoke(other.gameObject);
        }
    }
}
