// Scripts/Door.cs
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour, IInteractable
{
    public Animator animator; // door animator (open/close)
    public bool isOpen = false;
    public AudioClip openSfx;
    public AudioClip closeSfx;
    AudioSource audioSource;

    void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void Interact()
    {
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
        Debug.Log(isOpen ? "Дверь открыта" : "Дверь закрыта");
        //PlaySfx(isOpen ? openSfx : closeSfx);
    }

    public Vector3 GetInteractionPosition()
    {
        return transform.position;
    }

    void PlaySfx(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.PlayOneShot(clip);
    }

    public string GetInteractionText()
    {
        return isOpen ? "Закрыть дверь" : "Открыть дверь";
    }
}
