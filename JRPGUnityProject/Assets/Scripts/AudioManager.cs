using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

    public AudioSource musicSource;
    public AudioClip battleBGM;
    public AudioClip normalBGM;
    public AudioClip battleTransition;
    public AudioClip normalTransition;

    public static AudioManager Instance;

    public string[] overWorldScenes;
    public string[] fightingScenes;

    private bool changingMusic = true;

    void Awake()
    {
        if (Instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        musicSource.playOnAwake = false;
        musicSource.rolloffMode = AudioRolloffMode.Logarithmic;
        musicSource.loop = true;
    }

    void Start () 
    {
        StartAudio(SceneManager.GetActiveScene().name);
    }

    public IEnumerator PlayTransition(string newScene)
    {
        changingMusic = false;
        
        musicSource.loop = false;

        if (overWorldScenes.Contains(newScene))
        {
            if (musicSource.clip != normalTransition && musicSource.clip != normalBGM)
            {
                musicSource.clip = normalTransition;
                changingMusic = true;
            }
        }
        if (fightingScenes.Contains(newScene))
        {
            if (musicSource.clip != battleTransition && musicSource.clip != battleBGM)
            {
                musicSource.clip = battleTransition;
                changingMusic = true;
            }
        }

        if (changingMusic)
        {
            musicSource.Play();
        }
        yield return new WaitForSeconds(musicSource.clip.length);
        musicSource.loop = true;
        StartAudio(newScene);
    }

    void StartAudio(string newScene)
    {
        if (overWorldScenes.Contains(newScene))
        {
            musicSource.clip = normalBGM;
        }
        else if (fightingScenes.Contains(newScene))
        {
            musicSource.clip = battleBGM;
        }

        if (changingMusic)
        {
            musicSource.Play();
        }
    }

    IEnumerator OnLevelWasLoaded()
    {
        StartCoroutine(PlayTransition(SceneManager.GetActiveScene().name));
        yield break;
    }
}
