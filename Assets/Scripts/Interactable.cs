using UnityEngine;
public interface IInteractable
{
    // Вызывается при взаимодействии
    void Interact();
    // Возвращает позицию интеракта для определения ближайшего
    string GetInteractionText();

    Vector3 GetInteractionPosition();
}