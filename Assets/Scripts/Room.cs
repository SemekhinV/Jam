// Scripts/Room.cs
using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public SpriteRenderer background;
    public List<GameObject> interactables = new List<GameObject>();
    public bool discovered = false;
    public int index; // позиция в линии
}
