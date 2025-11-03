// Scripts/RoomManager.cs
using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public List<Room> rooms = new List<Room>();
    public int currentRoomIndex = 0;
    public Transform roomsParent;
    public float roomWidth = 10f; // расстояние между центрами комнат

    void Start()
    {
        LayoutRooms();
        DiscoverRoom(currentRoomIndex);
    }

    public void LayoutRooms()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].index = i;
            if (roomsParent != null)
                rooms[i].transform.SetParent(roomsParent, true);
            rooms[i].transform.position = new Vector3(i * roomWidth, 0, 0);
        }
    }

    public void DiscoverRoom(int index)
    {
        if (index < 0 || index >= rooms.Count) return;
        rooms[index].discovered = true;
        // можно вызвать обновление мини-карты:
        //MinimapController.Instance?.MarkDiscovered(index);
    }

    public Room GetRoom(int index) { if (index < 0 || index >= rooms.Count) return null; return rooms[index]; }
}
