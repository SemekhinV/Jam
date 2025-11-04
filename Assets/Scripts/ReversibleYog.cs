using System.Collections.Generic;
using UnityEngine;

public class ReversibleYog : SpriteInteraction
{
    [SerializeField] private List<ReversibleYog> _anotherToReverse;
    [SerializeField] private SpriteRenderer _sprite;

    public bool IsStandingOnHead => _sprite.flipY;

    private bool _isCompleted;
    
    public void Complete()
    {
        _isCompleted = true;
    }

    protected override void OnSpriteClicked()
    {
        base.OnSpriteClicked();
        ReverseWithAnother();
    }

    public void ReverseWithAnother()
    {
        if (_isCompleted == true)
        {
            return;
        }
        
        Reverse();
        foreach (ReversibleYog reversibleYog in _anotherToReverse)
        {
            reversibleYog.Reverse();
        }
    }

    private void Reverse()
    {
        _sprite.flipY = !_sprite.flipY;
    }
}