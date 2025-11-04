using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SpriteInteractionWithShader : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    private bool isMouseOver = false;
    private Material originalMaterial;
    private Material outlineMaterial;

    [Header("Настройки обводки")]
    public Color outlineColor = Color.yellow;
    public float outlineWidth = 0.02f;

    [Header("Настройки взаимодействия")]
    public LayerMask interactableLayer = -1;

    private Mouse mouse;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        mouse = Mouse.current;
        
        // Сохраняем оригинальный материал и создаем материал с обводкой
        originalMaterial = spriteRenderer.material;
        outlineMaterial = new Material(Shader.Find("Custom/OutlineShader"));
        outlineMaterial.CopyPropertiesFromMaterial(originalMaterial);
        
        // Настраиваем параметры обводки
        outlineMaterial.SetColor("_OutlineColor", outlineColor);
        outlineMaterial.SetFloat("_OutlineWidth", outlineWidth);
        outlineMaterial.SetFloat("_OutlineEnabled", 0);
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

        // Обработка входа/выхода мыши
        if (isThisObject && !isMouseOver)
        {
            OnMouseEnterSprite();
            isMouseOver = true;
        }
        else if (!isThisObject && isMouseOver)
        {
            OnMouseExitSprite();
            isMouseOver = false;
        }

        // Обработка клика
        if (isThisObject && mouse.leftButton.wasPressedThisFrame)
        {
            OnSpriteClicked();
        }
    }

    void OnMouseEnterSprite()
    {
        if (outlineMaterial != null)
        {
            spriteRenderer.material = outlineMaterial;
            outlineMaterial.SetFloat("_OutlineEnabled", 1);
        }
    }

    void OnMouseExitSprite()
    {
        if (originalMaterial != null)
        {
            spriteRenderer.material = originalMaterial;
            if (outlineMaterial != null)
                outlineMaterial.SetFloat("_OutlineEnabled", 0);
        }
    }

    protected virtual void OnSpriteClicked()
    {
        Debug.Log($"Клик по: {gameObject.name}");
        // Ваша логика здесь
    }

    bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    // Методы для изменения параметров обводки
    public void SetOutlineColor(Color color)
    {
        outlineColor = color;
        if (outlineMaterial != null)
            outlineMaterial.SetColor("_OutlineColor", color);
    }

    public void SetOutlineWidth(float width)
    {
        outlineWidth = width;
        if (outlineMaterial != null)
            outlineMaterial.SetFloat("_OutlineWidth", width);
    }

    void OnDestroy()
    {
        // Очистка материалов
        if (outlineMaterial != null)
            DestroyImmediate(outlineMaterial);
    }
}