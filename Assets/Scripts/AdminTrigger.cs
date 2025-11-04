using UnityEngine;

public class AdminTrigger : MonoBehaviour
{
    [SerializeField] private LobbyTutor _tutor;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        _tutor.ShowMouse(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _tutor.ShowMouse(false);
    }
}