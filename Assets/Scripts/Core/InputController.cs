using UnityEngine;
using UnityEngine.InputSystem;

public sealed class InputController : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 2f;

    private Camera _mainCamera;
    private PlayerController _playerController;
    private InputAction _interactAction;

    public static bool IsControlling { get; private set; }

    private void Awake()
    {
        _mainCamera = Camera.main;
        _playerController = FindObjectOfType<PlayerController>();

        _interactAction = new InputAction(binding: InputActionBindings.Interact.MOUSE_LEFT_BUTTON);
    }

    private void OnEnable()
    {
        _interactAction.Enable();
        _interactAction.performed += OnInteractPerformed;
    }

    private void OnDisable()
    {
        _interactAction.Disable();
        _interactAction.performed -= OnInteractPerformed;
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
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
                if (hit.collider.GetComponent<IDrivable>() != null)
                {
#if UNITY_EDITOR
                    Debug.Log($"Object is drivable: {hit.collider.name}. Interacting directly.");
#endif
                    interactable.Interact();
                    return;
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
                ReleaseControl();
            }
        });
    }

    public static void ReleaseControl()
    {
        IsControlling = false;
#if UNITY_EDITOR
        Debug.Log("Control released.");
#endif
    }
}
