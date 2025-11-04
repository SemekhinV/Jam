using System.Collections;
using UnityEngine;

namespace CutScene
{
    public class CutScenes : MonoBehaviour
    {
        [SerializeField] private GameObject _fractals;
        [SerializeField] private GameObject _scene1;
        [SerializeField] private GameObject _video1;
        [SerializeField] private GameObject _video2;
        
        [SerializeField] private GameObject _audio1;
        [SerializeField] private GameObject _thx4Game;
        private void OnEnable()
        {
            StartCoroutine(StartCutScene());
        }

        private IEnumerator StartCutScene()
        {
            _audio1.SetActive(true);
            
            _fractals.SetActive(true);
            yield return new WaitForSecondsRealtime(2f);
            _scene1.SetActive(true);
            
            yield return new WaitForSecondsRealtime(2f);
            _fractals.SetActive(false);
            
            yield return new WaitForSecondsRealtime(4f);
            _scene1.SetActive(false);
            
            _video1.SetActive(true);
            yield return new WaitForSecondsRealtime(4f);
            _video1.SetActive(false);
            
            _video2.SetActive(true);
            yield return new WaitForSecondsRealtime(4f);
            _thx4Game.SetActive(true);
        }
    }
}