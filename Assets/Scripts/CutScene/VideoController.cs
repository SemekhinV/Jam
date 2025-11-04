using UnityEngine;
using UnityEngine.Video;

namespace CutScene
{
    public class VideoController : MonoBehaviour
    {
        public VideoPlayer videoPlayer;
        public string videoFileName; // Имя файла в папке StreamingAssets
    
        void Start()
        {
            // Получаем компонент VideoPlayer
            videoPlayer = GetComponent<VideoPlayer>();
        
            // Устанавливаем источник видео
            videoPlayer.source = VideoSource.Url;
        
            // Путь к файлу (файл должен быть в папке StreamingAssets)
            videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
        
            // Настройки воспроизведения
            videoPlayer.isLooping = true;
            videoPlayer.playOnAwake = true;
        
            // Подписываемся на события
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.loopPointReached += OnVideoEnd;
        
            // Подготавливаем видео
            videoPlayer.Prepare();
        }
    
        void OnVideoPrepared(VideoPlayer vp)
        {
            // Видео готово к воспроизведению
            vp.Play();
        }
    
        void OnVideoEnd(VideoPlayer vp)
        {
            // Действия при завершении видео
            Debug.Log("Video finished playing");
        }
    
        public void PlayVideo()
        {
            if (!videoPlayer.isPlaying)
                videoPlayer.Play();
        }
    
        public void PauseVideo()
        {
            if (videoPlayer.isPlaying)
                videoPlayer.Pause();
        }
    
        public void StopVideo()
        {
            videoPlayer.Stop();
        }
    }
}