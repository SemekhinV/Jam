using UnityEngine;

public class Room5 : Room
{
    [SerializeField] private ReversibleYog _reversibleYog1;
    [SerializeField] private ReversibleYog _reversibleYog2;
    [SerializeField] private ReversibleYog _reversibleYog3;

    private bool _roomCompleted;

    private void Update()
    {
        if (_roomCompleted)
            return;

        if (_reversibleYog1.IsStandingOnHead && _reversibleYog2.IsStandingOnHead && _reversibleYog3.IsStandingOnHead)
        {
            _roomCompleted = true;
            _reversibleYog1.Complete();
            _reversibleYog2.Complete();
            _reversibleYog3.Complete();
            Debug.Log($"Room Completed");
        }
    }
}