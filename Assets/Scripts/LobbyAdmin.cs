using System.Collections.Generic;
using UI;
using UnityEngine;

public class LobbyAdmin : SpriteInteraction
{
    [SerializeField] private LobbyTutor _tutor;
    [SerializeField] private List<string> _dialogs;
    
    private DialogSystem _dialogSystem;

    private bool _completed = false;
    private void Awake()
    {
        _dialogSystem = FindAnyObjectByType<DialogSystem>();
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
        _dialogSystem.ShowDialog(_dialogs);
    }
}