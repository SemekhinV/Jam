using UnityEngine;
using UnityEngine.InputSystem;

public class SpriteInteraction : MonoBehaviour
{
    private Camera mainCamera;
    [Header("Настройки взаимодействия")]
    public LayerMask interactableLayer = -1;
    private Mouse mouse;

    [SerializeField] private InteractionTrigger _trigger;
    void Start()
    {
        mainCamera = Camera.main;
        mouse = Mouse.current;
    }

    void Update()
    {
        HandleMouseInteraction();
    }

    void HandleMouseInteraction()
    {
        if (mouse == null) return;

        Vector2 mousePosition = mouse.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, interactableLayer);

        bool isThisObject = hit.collider != null && hit.collider.gameObject == gameObject;


        // Обработка клика
        if (isThisObject && mouse.leftButton.wasPressedThisFrame)
        {
            if (_trigger != null && _trigger.InTrigger)
            {
                OnSpriteClicked();
                return;
            }

            if (_trigger == null)
            {
                OnSpriteClicked();
            }
        }
    }


    protected virtual void OnSpriteClicked()
    {
        Debug.Log($"Клик по: {gameObject.name}");

    }
}