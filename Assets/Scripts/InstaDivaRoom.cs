using UnityEngine;

public class InstaDivaRoom : Room
{
    // [SerializeField] private TimingMinigame _timingMiniGame;
    
    private void OnEnable()
    {
        // _timingMiniGame.OnComlpete += OnMiniGameComplete;
    }
    
    private void Disable()
    {
        // _timingMiniGame.OnComlpete -= OnMiniGameComplete;
    }

    protected override void OnEnterRoomCompeted()
    {
        base.OnEnterRoomCompeted();
        // Disable PlayerController
        // _timingMiniGame.gameObject.SetActive(true);
    }

    private void OnMiniGameComplete()
    {
        // Enable PlayerController
    }
}