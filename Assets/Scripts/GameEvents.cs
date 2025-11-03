using System;
using UnityEngine;

public static class GameEvents
{
    public static Action<int> OnRoomDiscovered; // room index
    public static Action<int> OnDoorToggled; // door id
    public static Action<Vector2> OnPlayerInteract; // position
    public static Action OnToggleMinimap;
}