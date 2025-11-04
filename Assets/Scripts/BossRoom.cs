using System.Collections;
using System.Collections.Generic;
using CutScene;
using UnityEngine;

public class BossRoom : Room
{
    [SerializeField]
    private CutScenes _cutScenes;

    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = FindAnyObjectByType<PlayerController>();
    }
    protected override void OnEnterRoomCompeted()
    {
        base.OnEnterRoomCompeted();
        _cutScenes.gameObject.SetActive(true);
        _playerController.gameObject.SetActive(false);
        StartCoroutine(DisableRoom());
    }

    private IEnumerator DisableRoom()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}