using System;

namespace minigames
{
    public interface IMinigame
    {
        event Action OnComplete;
        void StartMinigame();
    }
}