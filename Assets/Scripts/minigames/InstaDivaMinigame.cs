using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace minigames
{
    public class InstaDivaMinigame : MonoBehaviour, IInteractable, IMinigame
    {
        [Header("UI Elements")] public GameObject uiRoot;
        private AudioSource audioSource;

        [Header("Input Settings")] [SerializeField]
        private InputActionReference _interactActionReference;

        [Header("Audio (optional)")] public AudioClip successSound;
        private InputAction _interactAction;
        private bool isActive = false;

        [SerializeField] private SpriteRenderer _selfiPalka;
        [SerializeField] private SpriteRenderer _light;

        public void Interact()
        {
            _light.enabled = true;
            _light.enabled = false;
        }

        public string GetInteractionText()
        {
            throw new NotImplementedException();
        }

        public Vector3 GetInteractionPosition()
        {
            throw new NotImplementedException();
        }

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
            _selfiPalka.enabled = true;
            _light.enabled = true;
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

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            if (!isActive) return;

            CheckResult();
        }

        private void CheckResult()
        {
            Invoke(nameof(CloseMinigame), 1.2f);
        }

        private void CloseMinigame()
        {
            uiRoot.SetActive(false);
            isActive = false;
            _selfiPalka.enabled = false;
            _light.enabled = false;
        }

        public event Action OnComplete;
        public void StartMinigame()
        {
            _selfiPalka.enabled = true;
            _light.enabled = true;
            _interactAction.Enable();
            _interactAction.performed += OnInteractPerformed;
        }
    }
}