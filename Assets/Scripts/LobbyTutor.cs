using UnityEngine;

public class LobbyTutor : MonoBehaviour
{
    [SerializeField] private GameObject _right;
    [SerializeField] private GameObject _left;
    [SerializeField] private GameObject _mouse;

    private bool _mouseTutorCompleted = false;

    public void ShowMouse(bool isActive)
    {
        if (_mouseTutorCompleted)
        {
            _mouse.SetActive(false);
            return;
        }
        
        _mouse.SetActive(isActive);
    }
    
    public void ShowRight(bool isActive)
    {
        _right.SetActive(isActive);
    }
    
    public void ShowLeft(bool isActive)
    {
        _left.SetActive(isActive);
    }

    public void CompleteMouseTutor()
    {
        _mouseTutorCompleted = true;
        ShowMouse(false);
    }
}