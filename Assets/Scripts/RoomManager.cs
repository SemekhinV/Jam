// Scripts/RoomManager.cs

using System;
using UnityEngine;
using System.Collections.Generic;
using CutScene;
using UI;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Hider _hider;
    [SerializeField] private List<Room> _roomSchedule;
    [SerializeField] private CutScenes _cutScene;
    [SerializeField] private Transform _playerSpawnPosition;
    
    
    public int _currentRoomIndex = -1;
    
    private Room _currentRoom;
    
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = FindAnyObjectByType<PlayerController>();
    }

    public void StartRoomSchedule()
    {
        _currentRoomIndex = 0;
        if (_roomSchedule.Count < _currentRoomIndex)
        {
            Debug.Log("Room Schedule is empty");
            _cutScene.gameObject.SetActive(true);
            _playerController.gameObject.SetActive(false);
            // TODO final cutscene
            return;
        }

        _currentRoom = _roomSchedule[_currentRoomIndex];
        _currentRoom.gameObject.SetActive(true);
        _currentRoom.Enter();
        _currentRoom.RoomCompleted += NextRoom;
    }
    
    public void NextRoom()
    {
        _currentRoom.RoomCompleted -= NextRoom;
        _hider.HideAndShow(HideCurrentRoomAndShowNextRoom);
    }

    private void HideCurrentRoomAndShowNextRoom()
    {
        _currentRoom.gameObject.SetActive(false);
        _currentRoomIndex++;
        if (_roomSchedule.Count < _currentRoomIndex)
        {
            Debug.Log("Room Schedule is empty");
            _cutScene.gameObject.SetActive(true);
            _playerController.gameObject.SetActive(false);
            return;
        }
        Room nextRoom = _roomSchedule[_currentRoomIndex];
        nextRoom.gameObject.SetActive(true);
        nextRoom.Enter();
        _currentRoom = nextRoom;
        _playerController.transform.position = _playerSpawnPosition.position;
        
    }
    
    
    
    // private void Start()
    // {
    //     LayoutRooms();
    //     DiscoverRoom(currentRoomIndex);
    // }
    //
    // public void LayoutRooms()
    // {
    //     for (int i = 0; i < rooms.Count; i++)
    //     {
    //         rooms[i].index = i;
    //         if (roomsParent != null)
    //             rooms[i].transform.SetParent(roomsParent, true);
    //         rooms[i].transform.position = new Vector3(i * roomWidth, 0, 0);
    //     }
    // }
    //
    // public void DiscoverRoom(int index)
    // {
    //     if (index < 0 || index >= rooms.Count) return;
    //     rooms[index].discovered = true;
    //     // ????? ??????? ?????????? ????-?????:
    //     //MinimapController.Instance?.MarkDiscovered(index);
    // }
    //
    // public Room GetRoom(int index) { if (index < 0 || index >= rooms.Count) return null; return rooms[index]; }
}
