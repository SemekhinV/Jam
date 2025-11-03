using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteraction : MonoBehaviour
{
    [Header("Detection")]
    public float detectRadius = 1.5f;
    public LayerMask interactableLayer; // настрой слой Interactable

    [Header("UI Prompt")]
    public RectTransform promptRoot; // ссылка на RectTransform канваса (Canvas)
    public GameObject promptPrefab;  // prefab: simple UI element with Text/Image
    public Vector2 promptScreenOffset = new Vector2(0, 50);

    [Header("Debug")]
    public bool drawGizmos = true;

    // runtime
    private GameObject promptInstance;
    private RectTransform promptRect;
    private GameObject currentGO; // ближайший GameObject интеракта
    private IInteractable currentInteractable;
    private Camera mainCam;

    // Input System
    private PlayerInputActions input;
    private System.Action<InputAction.CallbackContext> interactCallback;

    private void Awake()
    {
        mainCam = Camera.main;
        input = new PlayerInputActions();
        interactCallback = ctx => OnInteract();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Interact.performed += interactCallback;
    }

    private void OnDisable()
    {
        input.Player.Interact.performed -= interactCallback;
        input.Disable();
    }

    private void Update()
    {
        FindNearestInteractable();

        if (currentInteractable != null)
        {
            ShowOrUpdatePrompt(currentInteractable.GetInteractionText(), currentInteractable.GetInteractionPosition());
        }
        else
        {
            HidePrompt();
        }
    }

    private void FindNearestInteractable()
    {
        currentInteractable = null;
        currentGO = null;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectRadius, interactableLayer);
        float bestDist = float.MaxValue;

        for (int i = 0; i < hits.Length; i++)
        {
            var go = hits[i].gameObject;
            var interact = go.GetComponent<IInteractable>();
            if (interact == null) continue;

            float d = Vector2.Distance(transform.position, interact.GetInteractionPosition());
            if (d < bestDist)
            {
                bestDist = d;
                currentInteractable = interact;
                currentGO = go;
            }
        }
    }

    private void ShowOrUpdatePrompt(string text, Vector3 worldPos)
    {
        if (promptPrefab == null || promptRoot == null) return;

        if (promptInstance == null)
        {
            promptInstance = Instantiate(promptPrefab, promptRoot);
            promptRect = promptInstance.GetComponent<RectTransform>();
        }

        // обновляем текст (поддержка Text и TextMeshPro)
        var uiText = promptInstance.GetComponentInChildren<UnityEngine.UI.Text>();
        if (uiText != null) uiText.text = text;
        else
        {
            var tmp = promptInstance.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (tmp != null) tmp.text = text;
        }

        // позиционируем: world -> screen -> rectTransform anchored pos
        Vector3 screenPos = mainCam.WorldToScreenPoint(worldPos);
        // если Canvas Screen Space - Overlay или Screen Space - Camera, screenPos в пикселях экрана
        promptRect.position = screenPos + (Vector3)promptScreenOffset;
    }

    private void HidePrompt()
    {
        if (promptInstance != null)
        {
            Destroy(promptInstance);
            promptInstance = null;
            promptRect = null;
        }
    }

    private void OnInteract()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
