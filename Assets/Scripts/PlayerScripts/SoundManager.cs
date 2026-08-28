using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName;
        public AudioClip music;
    }

    private static SoundManager instance;

    public AudioMixer audioMixer;
    public AudioClip defaultMusic;
    public SceneMusic[] sceneMusic;

    private AudioSource musicSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        musicSource = GetComponent<AudioSource>();
        musicSource.loop = true;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            instance = null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        AudioClip music = defaultMusic;

        if (sceneMusic != null)
        {
            foreach (SceneMusic sceneTrack in sceneMusic)
            {
                if (sceneTrack.sceneName == sceneName)
                {
                    music = sceneTrack.music;
                    break;
                }
            }
        }

        if (musicSource.clip == music)
        {
            return;
        }

        musicSource.clip = music;

        if (music != null)
        {
            musicSource.Play();
        }
        else
        {
            musicSource.Stop();
        }
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
}
