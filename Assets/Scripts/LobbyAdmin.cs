using System.Collections.Generic;
using minigames;
using UI;
using UnityEngine;

public class LobbyAdmin : SpriteInteraction
{
    [SerializeField] private LobbyTutor _tutor;
    [SerializeField] private List<string> _dialogs;
    
    [Header("MiniGame (optional)")]
    [SerializeField] protected MonoBehaviour _minigameBehaviour; // любой компонент, реализующий IMinigame
    protected IMinigame _minigame;
    
    private DialogSystem _dialogSystem;

    private bool _completed = false;
    private void Awake()
    {
        _dialogSystem = FindAnyObjectByType<DialogSystem>();
        if (_minigameBehaviour != null)
            _minigame = _minigameBehaviour as IMinigame;
    }

    protected override void OnSpriteClicked()
    {
        base.OnSpriteClicked();

        if (_completed)
        {
            return;
        }

        _completed = true;
        _tutor.ShowMouse(false);
        _tutor.CompleteMouseTutor();
        _dialogSystem.ShowDialog(_dialogs, OnEnterRoomCompeted);
        // Если миниигра назначена — запустить её
    }
    
    protected virtual void OnEnterRoomCompeted()
    {
        // Debug.Log($"[Room] Starting minigame in room '{name}'");
        
        // Если миниигра назначена — запустить её
        if (_minigame != null)
        {
            _minigame.OnComplete += OnMinigameComplete;
            _minigame.StartMinigame();
            return; // ждём завершения миниигры
        }

        // Если миниигры нет — открываем дверь сразу
        TryOpenExitDoor();
    }
    
    private void OnMinigameComplete()
    {
        Debug.Log($"[Room] Minigame in room '{name}' completed");
        _minigame.OnComplete -= OnMinigameComplete;
        TryOpenExitDoor();
    }

    private void TryOpenExitDoor()
    {
        // if (_exitDoor != null)
        //     _exitDoor.Opened += RoomCompleted;
        // else
        //     RoomCompleted?.Invoke();
    }
}