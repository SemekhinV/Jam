// Scripts/TimingMinigame.cs

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class TimingMinigame : MonoBehaviour, IInteractable
{
    public event Action OnComlpete;

    [Header("UI Elements")] public GameObject uiRoot; // Canvas object with slider, target zone, and text
    public Slider timingSlider;
    public Image targetZone;
    public TextMeshProUGUI resultText;

    [Header("Game Settings")] public float speed = 2f; // Speed of slider movement
    public float targetStart = 0.2f; // Start of target zone (0–1)
    public float targetEnd = 0.8f; // End of target zone (0–1)
    public float winPosition = 0.5f; // Target position to hit (0–1)

    [Header("Input Settings")] [SerializeField]
    private InputActionReference _interactActionReference;

    [Header("Audio (optional)")] public AudioClip successSound;
    public AudioClip failSound;
    private AudioSource audioSource;

    private InputAction _interactAction;
    private bool isActive = false;
    private bool movingForward = true;

    private void Awake()
    {
        // Настройка аудио
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        // Настройка Input System
        SetupInputActions();

        // Скрываем UI при старте
        if (uiRoot != null) uiRoot.SetActive(false);
    }

    private void SetupInputActions()
    {
        // Используем Input Action Reference или создаем действие по умолчанию
        if (_interactActionReference != null)
        {
            _interactAction = _interactActionReference.action;
        }
        else
        {
            // Создаем действие по умолчанию (мышь и пробел)
            _interactAction = new InputAction("MinigameInteract", InputActionType.Button);
            _interactAction.AddBinding("<Mouse>/leftButton");
            _interactAction.AddBinding("<Keyboard>/space");
        }
    }

    private void OnEnable()
    {
        _interactAction.Enable();
        _interactAction.performed += OnInteractPerformed;
    }

    private void OnDisable()
    {
        _interactAction.performed -= OnInteractPerformed;
        _interactAction.Disable();

        // Останавливаем миниигру при отключении
        if (isActive)
        {
            CloseMinigame();
        }
    }

    private void OnDestroy()
    {
        // Освобождаем ресурсы если действие создавали вручную
        if (_interactActionReference == null)
        {
            _interactAction?.Dispose();
        }
    }

    private void Update()
    {
        if (!isActive || timingSlider == null) return;

        // Движение бегунка туда-сюда
        float delta = speed * Time.deltaTime * (movingForward ? 1 : -1);
        timingSlider.value = Mathf.Clamp01(timingSlider.value + delta);

        if (timingSlider.value >= 1f) movingForward = false;
        else if (timingSlider.value <= 0f) movingForward = true;
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (!isActive) return;

        CheckResult();
    }

    private void CheckResult()
    {
        float val = timingSlider.value;

        // Проверяем попадание в целевую зону
        bool success = val >= targetStart && val <= targetEnd;
        // Или для точного попадания в конкретную позицию:
        // bool success = Mathf.Abs(val - winPosition) < 0.1f;

        if (success)
        {
            resultText.text = "✅ Успех!";
            if (successSound) audioSource.PlayOneShot(successSound);
        }
        else
        {
            resultText.text = "❌ Промах!";
            if (failSound) audioSource.PlayOneShot(failSound);
        }

        Invoke(nameof(CloseMinigame), 1.2f);
    }

    private void CloseMinigame()
    {
        uiRoot.SetActive(false);
        isActive = false;
        resultText.text = "";
        OnComlpete?.Invoke();
    }

    // ========== IInteractable реализация ==========

    public void Interact()
    {
        if (uiRoot == null) return;

        uiRoot.SetActive(true);
        timingSlider.value = 0f;
        movingForward = true;
        isActive = true;

        // Включаем действие только когда миниигра активна
        _interactAction?.Enable();
    }

    public string GetInteractionText() => "Играть в мини-игру";

    public Vector3 GetInteractionPosition() => transform.position;

    // Дополнительные методы для управления вводом
    public void SetInteractAction(InputActionReference newAction)
    {
        // Отписываемся от старого действия
        _interactAction.performed -= OnInteractPerformed;
        _interactAction.Disable();

        // Устанавливаем новое действие
        _interactActionReference = newAction;
        if (_interactActionReference != null)
        {
            _interactAction = _interactActionReference.action;
            if (isActive)
            {
                _interactAction.Enable();
                _interactAction.performed += OnInteractPerformed;
            }
        }
    }

    // Метод для проверки активности миниигры
    public bool IsActive() => isActive;
}