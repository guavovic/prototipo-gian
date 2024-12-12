using Prototype.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototype.Controllers
{
    [RequireComponent(typeof(PlayerInput))]
    public sealed class InputController : MonoBehaviour
    {
        private class InputActionBindings
        {
            public const string MOUSE_INTERACT = "MouseInteract";
            public const string DRIVE = "Drive";
        }

        [Header("Interaction Settings")]
        [SerializeField] private float interactionDistance = 2f;

        private PlayerInput _playerInput;
        private InputAction _mouseInteractAction;
        private InputAction _driveAction;

        private Camera _mainCamera;
        private PlayerController _playerController;
        private VehicleController _currentVehicleSelected;

        public static bool IsControlling { get; private set; }

        public delegate void InteractAction(InputAction inputAction);
        public static event InteractAction OnMouseInteract;
        public static event InteractAction OnDriveInteract;

        public static void BlockMouseInteractControl()
        {
            IsControlling = true;
#if UNITY_EDITOR
            Debug.Log("Mouse Interact Control blocked.");
#endif
        }

        public static void ReleaseMouseInteractControl()
        {
            IsControlling = false;
#if UNITY_EDITOR
            Debug.Log("Mouse Interact Control released.");
#endif
        }

        private void Awake()
        {
            InitializeComponents();
            SetupPlayerInputs();
        }

        private void InitializeComponents()
        {
            _playerInput = GetComponent<PlayerInput>();
            _mainCamera = Camera.main;
            _playerController = FindObjectOfType<PlayerController>();
        }

        private void SetupPlayerInputs()
        {
            _mouseInteractAction = _playerInput.currentActionMap[InputActionBindings.MOUSE_INTERACT];
            _driveAction = _playerInput.currentActionMap[InputActionBindings.DRIVE];
        }

        private void OnEnable()
        {
            OnEnableMouseInteract();

            if (_currentVehicleSelected != null)
            {
                OnEnableDriveInteract();
            }
        }

        private void OnDisable()
        {
            OnDisableMouseInteract();

            if (_currentVehicleSelected != null)
            {
                OnDisableDriveInteract();
            }
        }

        private void OnMouseInteractPerformed(InputAction.CallbackContext context)
        {
            if (Managers.GameManager.GameState.IsPaused)
            {
                return;
            }

            if (IsControlling)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Interaction ignored: Player is currently controlling an object.");
#endif
                return;
            }

            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
#if UNITY_EDITOR
                Debug.Log($"Raycast hit detected: {hit.collider.name}");
#endif
                if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    interactable.InteractionPoint = hit.point;
#if UNITY_EDITOR
                    Debug.Log($"Interactable found: {hit.collider.name} at point {hit.point}");
#endif
                    if (hit.collider.TryGetComponent<VehicleController>(out var vehicle))
                    {
                        if (!vehicle.IsBlocked && !vehicle.IsDriving)
                        {
                            interactable.Interact();
                            _currentVehicleSelected = vehicle;
#if UNITY_EDITOR
                            Debug.Log($"Object is drivable: {hit.collider.name}. Interacting directly.");
#endif
                            SwitchToVehicleControls();
                            return;
                        }
                    }

                    MoveToInteract(interactable);
                }
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarning("No object hit by raycast.");
            }
#endif
        }

        private void SwitchToVehicleControls()
        {
            OnDisableMouseInteract();
            BlockMouseInteractControl();
            OnEnableDriveInteract();
        }

        private void SwitchToMouseTnteractControls()
        {
            OnDisableDriveInteract();
            ReleaseMouseInteractControl();
            OnEnableMouseInteract();
        }

        private void OnEnableMouseInteract()
        {
            _mouseInteractAction.Enable();
            _mouseInteractAction.performed += OnMouseInteractPerformed;
        }

        private void OnDisableMouseInteract()
        {
            _mouseInteractAction.Disable();
            _mouseInteractAction.performed -= OnMouseInteractPerformed;
        }

        private void OnEnableDriveInteract()
        {
            _driveAction.Enable();
            _driveAction.performed += _currentVehicleSelected.OnDrive;
            _driveAction.canceled += _currentVehicleSelected.OnDrive;
        }

        private void OnDisableDriveInteract()
        {
            _driveAction.Disable();
            _driveAction.performed -= _currentVehicleSelected.OnDrive;
            _driveAction.canceled -= _currentVehicleSelected.OnDrive;
        }

        private void MoveToInteract(IInteractable currentInteractable)
        {
            Vector3 targetPosition = currentInteractable.InteractionPoint;
#if UNITY_EDITOR
            Debug.Log($"Moving to position: {targetPosition}");
#endif
            _playerController.MoveToPosition(targetPosition, () =>
            {
                float distance = Vector3.Distance(_playerController.transform.position, targetPosition);

                if (distance <= interactionDistance)
                {
                    currentInteractable.Interact();
                    ReleaseMouseInteractControl();
                }
            });
        }
    }
}