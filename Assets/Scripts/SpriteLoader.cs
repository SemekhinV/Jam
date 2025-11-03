using UnityEngine;

public static class SpriteLoader
{
    // Ищет спрайт в Resources/CustomSprites/<name>
    public static Sprite Load(string name)
    {
        if (string.IsNullOrEmpty(name)) return null;
        return Resources.Load<Sprite>("CustomSprites/" + name);
    }
}