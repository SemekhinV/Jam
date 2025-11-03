// Scripts/TimingMinigame.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimingMinigame : MonoBehaviour, IInteractable
{
    [Header("UI Elements")]
    public GameObject uiRoot; // Canvas object with slider, target zone, and text
    public Slider timingSlider;
    public Image targetZone;
    public TextMeshProUGUI resultText;

    [Header("Game Settings")]
    public float speed = 2f;         // Speed of slider movement
    public float targetStart = 1f; // Start of target zone (0–1)
    public float targetEnd = 10f;   // End of target zone (0–1)
    public float winPosition = 5f;   // End of target zone (0–1)

    [Header("Audio (optional)")]
    public AudioClip successSound;
    public AudioClip failSound;
    private AudioSource audioSource;

    private bool isActive = false;
    private bool movingForward = true;

    private void Awake()
    {
        if (uiRoot != null) uiRoot.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isActive || timingSlider == null) return;

        // движение бегунка туда-сюда
        float delta = speed * Time.deltaTime * (movingForward ? 1 : -1);
        timingSlider.value = Mathf.Clamp01(timingSlider.value + delta);

        if (timingSlider.value >= 1f) movingForward = false;
        else if (timingSlider.value <= 0f) movingForward = true;

        // нажатие мыши
        if (Input.GetMouseButtonDown(0))
        {
            CheckResult();
        }
    }

    private void CheckResult()
    {
        float val = timingSlider.value;
        bool success = val == winPosition;

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
    }

    // ========== IInteractable реализация ==========

    public void Interact()
    {
        if (uiRoot == null) return;

        uiRoot.SetActive(true);
        timingSlider.value = 0f;
        movingForward = true;
        isActive = true;
    }

    public string GetInteractionText() => "Играть в мини-игру";

    public Vector3 GetInteractionPosition() => transform.position;
}
