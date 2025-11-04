using System.Collections;
using UnityEngine;

public class LobbyRoom : Room
{
    [SerializeField] private LobbyTutor _lobbyTutor;

    private PlayerController _playerController;

    private bool _disablingRightTutorial;
    private bool _disablingLeftTutorial;
    private bool _leftTutorStarted;
    
    private void Awake()
    {
        _playerController = FindAnyObjectByType<PlayerController>();
        
    }

    private void Update()
    {
        if (_playerController.IsMovingRight && _disablingRightTutorial == false)
        {
            _disablingRightTutorial = true;
            StartCoroutine(DisableRightTutorial());
        }

        if (_playerController.IsMovingRight && _leftTutorStarted && _disablingLeftTutorial == false)
        {
            _disablingLeftTutorial = true;
            StartCoroutine(DisableLeftTutorial());
        }
    }

    private IEnumerator DisableRightTutorial()
    {
        yield return new WaitForSeconds(1f);
        _lobbyTutor.ShowRight(false);
        _lobbyTutor.ShowLeft(true);
        _leftTutorStarted = true;
    }
    
    private IEnumerator DisableLeftTutorial()
    {
        yield return new WaitForSeconds(1f);
        _lobbyTutor.ShowLeft(false);
    }

    private void OnEnable()
    {
        if (_lobbyTutor != null)
            _lobbyTutor.ShowRight(true);
        if (_minigame != null)
            _minigame.OnComplete += OnMiniGameComplete;
    }
    
    private void Disable()
    {
        if (_minigame != null)
            _minigame.OnComplete -= OnMiniGameComplete;
    }

    // protected override void OnEnterRoomCompeted()
    // {
    //     base.OnEnterRoomCompeted();
    //     // Disable PlayerController
    //     // _timingMiniGame.gameObject.SetActive(true);
    // }

    private void OnMiniGameComplete()
    {
        
        _minigame.StartMinigame();
    }
}