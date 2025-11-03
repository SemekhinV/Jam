using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour, IInteractable
{
    public event Action? Opened; 
    
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
        StartCoroutine(OpenDoor());
        Debug.Log(isOpen ? "����� �������" : "����� �������");
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
