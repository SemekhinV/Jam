// Scripts/Pigeon.cs
using UnityEngine;

public class Pigeon : MonoBehaviour, IInteractable
{
    public enum State { Idle, Alert, Flee }
    public State currentState = State.Idle;
    public Animator animator;
    public float alertDistance = 3f;
    public AudioClip cooSfx;
    AudioSource audioSource;
    Transform player;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player")?.transform;
        //InteractionSystem.OnPlayerAction += OnPlayerAction;
    }

    void OnDestroy()
    {
        //InteractionSystem.OnPlayerAction -= OnPlayerAction;
    }

    void Update()
    {
        // простой пример реакции: если игрок рядом — alert
        if (player != null)
        {
            float d = Vector2.Distance(transform.position, player.position);
            if (d < alertDistance) SetState(State.Alert);
            else SetState(State.Idle);
        }
        animator?.SetInteger("State", (int)currentState);
    }

    public void Interact()
    {
        // игрок взаимодействовал напрямую с голубем
        SetState(State.Flee);
        PlaySfx(cooSfx);
    }

    public Vector2 GetInteractPosition() => transform.position;

    void OnPlayerAction(GameObject playerObj, string action)
    {
        // базовая логика: реагирует на плевок или открытие двери
        if (action == "spit") SetState(State.Alert);
        if (action == "door_open") SetState(State.Alert);
    }

    void SetState(State s)
    {
        if (currentState == s) return;
        currentState = s;
        // animation handled by Animator via integer param
    }

    void PlaySfx(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.PlayOneShot(clip);
    }

    public string GetInteractionText()
    {
        return "Погладить голубя";
    }

    public Vector3 GetInteractionPosition()
    {
        return transform.position;
    }
}
