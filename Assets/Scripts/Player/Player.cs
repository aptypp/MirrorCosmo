using Game.Game.Player;
using Mirror;
using TMPro;
using UnityEngine;

namespace MomoCoop
{
    public class Player : NetworkBehaviour
    {
        [SerializeField]
        private Camera _camera;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private Transform _cameraTransform;
        [SerializeField]
        private GameObject _viewGameObject;
        [SerializeField]
        private PlayerBase _playerBase;
        [SerializeField]
        private TextMeshProUGUI _nicknameText;
        [SerializeField]
        private SkinnedMeshRenderer _meshRenderer;
        [SerializeField]
        private CharacterController _characterController;

        [SyncVar(hook = nameof(OnMoveDirectionChanged))]
        private float _moveAnimationDirection;
        [SyncVar(hook = nameof(OnStrafeDirectionChanged))]
        private float _strafeAnimationDirection;
        [SyncVar(hook = nameof(OnMoveSpeedChanged))]
        private float _moveAnimationSpeed;
        [SyncVar(hook = nameof(OnNicknameChanged))]
        private string _nickname;
        [SyncVar(hook = nameof(OnColorChanged))]
        private Color32 _color;

        private bool _isRun;
        private float _speed;
        private float _runSpeed;
        private float _sensitivity;
        private Vector2 _lookRotation;
        private Vector3 _moveDirection;
        private PlayerControls _playerControls;

        private static readonly int _animatorMoveSpeedHash = Animator.StringToHash("moveSpeed");
        private static readonly int _animatorMoveDirectionHash = Animator.StringToHash("moveDirection");
        private static readonly int _animatorStrafeDirectionHash = Animator.StringToHash("strafeDirection");

        private void Awake()
        {
            _meshRenderer.materials[0] = new Material(_meshRenderer.materials[0]);

            _speed = _playerBase.speed;
            _runSpeed = _playerBase.runSpeed;
            _sensitivity = _playerBase.sensitivity;
        }

        private void Update()
        {
            if (!isLocalPlayer) return;

            if (!_characterController.isGrounded) _moveDirection.y = -9.81f;

            if (_moveDirection != Vector3.zero) Move(_moveDirection);
        }

        public override void OnStartLocalPlayer()
        {
            _camera.enabled = true;
            _viewGameObject.SetActive(false);

            _playerControls = FindObjectOfType<PlayerControls>();

            _playerControls.SetPlayerMovement(SetMoveDirection);
            _playerControls.SetPlayerChangeColor(ChangeColor);
            _playerControls.SetPlayerLookRotation(ChangeLookRotation);

            SetNickName(LocalSettings.nickname);

            Cursor.lockState = CursorLockMode.Locked;
        }

        public override void OnStopLocalPlayer()
        {
            _playerControls.RemovePlayerMovement();
            _playerControls.RemoveChangeColor();
        }

        [Command]
        private void ChangeColor()
        {
            _color = new Color32((byte)Random.Range(100, 200), (byte)Random.Range(100, 200), (byte)Random.Range(100, 200), 255);
        }

        [Command]
        private void SetNickName(string nickname)
        {
            _nickname = nickname;
        }

        [Command]
        private void SetMotionAnimationParameters(Vector3 direction)
        {
            _moveAnimationDirection = direction.z;
            _strafeAnimationDirection = direction.x;
            _moveAnimationSpeed = Mathf.Clamp01(Mathf.Abs(direction.z)  + Mathf.Abs(direction.x));
        }

        public void SetMoveDirection(Vector2 moveDirection)
        {
            _moveDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
        }

        public void ChangeLookRotation(Vector2 delta)
        {
            _lookRotation.x += delta.x * _sensitivity;
            _lookRotation.y = Mathf.Clamp(_lookRotation.y + delta.y * _sensitivity, -90, 90);

            Quaternion bodyRotation = Quaternion.AngleAxis(_lookRotation.x, Vector3.up);
            Quaternion cameraRotation = bodyRotation * Quaternion.AngleAxis(-_lookRotation.y, Vector3.right);

            SetRotations(bodyRotation, cameraRotation);
        }

        private void Move(Vector3 direction)
        {
            SetMotionAnimationParameters(direction);

            Vector3 forward = transform.forward;
            Vector3 right = _cameraTransform.right;

            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = direction.x * right.x + direction.z * forward.x;
            moveDirection.y = direction.y;
            moveDirection.z = direction.x * right.z + direction.z * forward.z;

            SetMotion(moveDirection * ((_isRun ? _runSpeed : _speed) * Time.deltaTime));
        }

        private void OnColorChanged(Color32 oldValue, Color32 newValue)
        {
            _meshRenderer.material.color = newValue;
        }

        private void OnNicknameChanged(string oldValue, string newValue)
        {
            _nicknameText.text = newValue;
        }

        private void OnMoveSpeedChanged(float oldValue, float newValue)
        {
            _animator.SetFloat(_animatorMoveSpeedHash, newValue);
        }

        private void OnMoveDirectionChanged(float oldValue, float newValue)
        {
            _animator.SetFloat(_animatorMoveDirectionHash, newValue);
        }

        private void OnStrafeDirectionChanged(float oldValue, float newValue)
        {
            _animator.SetFloat(_animatorStrafeDirectionHash, newValue);
        }

        private void SetMotion(Vector3 motion)
        {
            _characterController.Move(motion);
        }

        private void SetRotations(Quaternion bodyRotation, Quaternion cameraRotation)
        {
            transform.rotation = bodyRotation;
            _cameraTransform.rotation = cameraRotation;
        }
    }
}