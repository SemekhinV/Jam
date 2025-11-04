using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class DialogSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dialogText;
    [SerializeField] private float _messageDelay = 1f;
    [SerializeField] private float _charDelay = 0.05f;
    
    private PlayerInput _playerInput;
    private InputAction _clickAction;
    private Coroutine _currentDialogCoroutine;
    private Coroutine _currentTypeCoroutine;
    private string _currentText;
    private bool _isTyping = false;
    private bool _waitingForClick = false;
    
    private void Awake()
    {
        // Получаем или создаем PlayerInput компонент
        _playerInput = GetComponent<PlayerInput>();
        if (_playerInput == null)
        {
            _playerInput = gameObject.AddComponent<PlayerInput>();
        }
        
        // Создаем действие для клика
        _clickAction = new InputAction("Click", InputActionType.Button, "<Mouse>/leftButton");
        _clickAction.Enable();
        
        // Или используем существующее действие из Input Action Asset
        // _clickAction = _playerInput.actions["Click"];
    }
    
    private void OnEnable()
    {
        _clickAction?.Enable();
    }
    
    private void OnDisable()
    {
        _clickAction?.Disable();
        StopDialog();
    }
    
    private void OnDestroy()
    {
        _clickAction?.Dispose();
    }
    
    public void ShowDialog(List<string> dialog)
    {
        ShowDialog(dialog, null);
    }
    
    public void ShowDialog(List<string> dialog, System.Action onComplete)
    {
        if (_currentDialogCoroutine != null)
        {
            StopCoroutine(_currentDialogCoroutine);
        }
        
        _currentDialogCoroutine = StartCoroutine(DialogCoroutine(dialog, onComplete));
    }
    
    public void StopDialog()
    {
        if (_currentTypeCoroutine != null)
        {
            StopCoroutine(_currentTypeCoroutine);
            _currentTypeCoroutine = null;
        }
        
        if (_currentDialogCoroutine != null)
        {
            StopCoroutine(_currentDialogCoroutine);
            _currentDialogCoroutine = null;
        }
        
        _isTyping = false;
        _waitingForClick = false;
        _dialogText.text = "";
    }
    
    private IEnumerator DialogCoroutine(List<string> dialog, System.Action onComplete)
    {
        foreach (var text in dialog)
        {
            _dialogText.text = "";
            _currentText = text;
            
            // Запускаем эффект печатания
            _isTyping = true;
            _currentTypeCoroutine = StartCoroutine(TypeText(text));
            
            // Ждем завершения печатания или клика для скипа
            while (_isTyping)
            {
                if (_clickAction.WasPerformedThisFrame())
                {
                    // Скипаем печатание и показываем весь текст сразу
                    if (_currentTypeCoroutine != null)
                    {
                        StopCoroutine(_currentTypeCoroutine);
                        _currentTypeCoroutine = null;
                    }
                    _dialogText.text = text;
                    _isTyping = false;
                }
                yield return null;
            }
            
            // Текст полностью отобразился, ждем клика для продолжения
            _waitingForClick = true;
            yield return WaitForClickOrDelay(_messageDelay);
            _waitingForClick = false;
        }
        
        _dialogText.text = "";
        onComplete?.Invoke();
        _currentDialogCoroutine = null;
    }
    
    private IEnumerator TypeText(string text)
    {
        _dialogText.text = "";
        
        foreach (char character in text)
        {
            _dialogText.text += character;
            yield return new WaitForSeconds(_charDelay);
            
            // Проверка на клик во время печатания
            if (_clickAction.WasPerformedThisFrame())
            {
                // Выходим из цикла, текст будет установлен полностью в основном корутине
                break;
            }
        }
        
        // Убеждаемся, что текст установлен полностью
        _dialogText.text = text;
        _isTyping = false;
        _currentTypeCoroutine = null;
    }
    
    private IEnumerator WaitForClickOrDelay(float delay)
    {
        float timer = 0f;
        bool clicked = false;
        
        while (timer < delay && !clicked)
        {
            timer += Time.deltaTime;
            
            if (_clickAction.WasPerformedThisFrame())
            {
                clicked = true;
            }
            
            yield return null;
        }
    }
    
    // Альтернативная версия с использованием любого действия (например, кнопки взаимодействия)
    public void SetSkipAction(InputAction skipAction)
    {
        _clickAction?.Dispose();
        _clickAction = skipAction;
        _clickAction?.Enable();
    }
    
    public bool IsDialogActive()
    {
        return _currentDialogCoroutine != null;
    }
    
    public bool IsTyping()
    {
        return _isTyping;
    }
    
    public bool IsWaitingForClick()
    {
        return _waitingForClick;
    }
}
}