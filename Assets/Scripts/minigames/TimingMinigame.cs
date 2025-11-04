using System;
using minigames;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class TimingMinigame : MonoBehaviour, IMinigame, IInteractable
{
    public event Action OnComplete;

    [Header("UI Elements")]
    public GameObject uiRoot;
    public Slider timingSlider;
    public Image targetZone;
    public TextMeshProUGUI resultText;

    [Header("Game Settings")]
    public float speed = 6f;
    public float targetStart = 0.3f;
    public float targetEnd = 0.7f;

    [Header("Audio")]
    public AudioClip successSound;
    public AudioClip failSound;

    private AudioSource audioSource;
    private InputAction interactAction;
    private bool isActive;
    private bool movingForward = true;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        uiRoot?.SetActive(false);

        interactAction = new InputAction(type: InputActionType.Button);
        interactAction.AddBinding("<Mouse>/leftButton");
        interactAction.AddBinding("<Keyboard>/space");
    }

    private void OnEnable()
    {
        interactAction.Enable();
        interactAction.performed += OnClick;
    }

    private void OnDisable()
    {
        interactAction.performed -= OnClick;
        interactAction.Disable();
    }

    private void Update()
    {
        if (!isActive || timingSlider == null) return;

        float delta = speed * Time.deltaTime * (movingForward ? 1 : -1);
        timingSlider.value = Mathf.Clamp(timingSlider.value + delta, 0, 10);
        if (timingSlider.value >= 10f) movingForward = false;
        else if (timingSlider.value <= 0f) movingForward = true;
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        if (!isActive) return;
        CheckResult();
    }

    private void CheckResult()
    {
        bool success = timingSlider.value > 4.6 && timingSlider.value < 5.5;

        resultText.text = success ? "✅ Успех!" : "❌ Промах!";
        audioSource.PlayOneShot(success ? successSound : failSound);

        if (!success) return;

        Invoke(nameof(Close), 1.0f);
    }

    private void Close()
    {
        uiRoot.SetActive(false);
        isActive = false;
        OnComplete?.Invoke();
    }

    public void StartMinigame()
    {
        uiRoot.SetActive(true);
        timingSlider.value = 0f;
        movingForward = true;
        isActive = true;
        Debug.Log(isActive);
    }

    // IInteractable реализация (если игрок может активировать вручную)
    public void Interact() => StartMinigame();
    public string GetInteractionText() => "Играть в мини-игру";
    public Vector3 GetInteractionPosition() => transform.position;
}
