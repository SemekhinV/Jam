using UI;
using UnityEngine;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{
    [SerializeField] private Button _startGame;
    [SerializeField] private Hider _hider;
    [SerializeField] private RoomManager _roomManager;

    private void OnEnable()
    {
        _startGame.onClick.AddListener(StartGame);
    }
    
    private void OnDisable()
    {
        _startGame.onClick.RemoveListener(StartGame);
    }

    private void StartGame()
    {
        _hider.HideAndShow(StartRoomsAndDisableMenu, DisableMenu);
    }

    private void StartRoomsAndDisableMenu()
    {
        _roomManager.StartRoomSchedule();
        DisableMenu();
    }

    private void DisableMenu()
    {
        gameObject.SetActive(false);
    }
}