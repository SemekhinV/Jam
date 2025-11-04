using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace CutScene
{
    public class UIVideoPlayer : MonoBehaviour
    {
        public VideoPlayer videoPlayer;
        public RawImage videoDisplay;
        public AudioSource audioSource;
    
        void Start()
        {
            // Настраиваем VideoPlayer
            videoPlayer.playOnAwake = false;
            audioSource.playOnAwake = false;
        
            // Связываем аудио с VideoPlayer
            videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            videoPlayer.SetTargetAudioSource(0, audioSource);
        
            // Подписываемся на событие подготовки
            videoPlayer.prepareCompleted += OnVideoPrepared;
        }
    
        void OnVideoPrepared(VideoPlayer vp)
        {
            // Получаем текстуру видео и применяем к RawImage
            videoDisplay.texture = vp.texture;
            videoDisplay.SetNativeSize();
        }
    
        public void LoadAndPlayVideo(string videoPath)
        {
            videoPlayer.url = videoPath;
            videoPlayer.Prepare();
        }
    
        void OnDestroy()
        {
            videoPlayer.prepareCompleted -= OnVideoPrepared;
        }
    }
}