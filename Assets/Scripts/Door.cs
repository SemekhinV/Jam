using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour, IInteractable
{
    public event Action Opened;

    [SerializeField] private RoomManager _roomManager;
    [SerializeField] private bool _isCanBeOpened;

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
        if (!_isCanBeOpened) return;
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
        StartCoroutine(OpenDoor());
        if (_roomManager != null)
        {
            _roomManager.StartRoomSchedule();
        }
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
        return isOpen ? "������� �����" : "������� �����";
    }

    private IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(0.3f);
        Opened?.Invoke();
    }
}