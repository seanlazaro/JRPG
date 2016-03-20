using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

    public AudioSource musicSource;
    public GameObject dontDestroy;
    public AudioClip battleBGM;
    public AudioClip normalBGM;
    public AudioClip battleTransition;
    public AudioClip normalTransition;

    public static AudioManager Instance;

    public string[] overWorldScenes;
    public string[] fightingScenes;

    private bool changingMusic = false;

    void Awake()
    {
        if (Instance)
        {
            Destroy(dontDestroy);
        }
        else
        {
            DontDestroyOnLoad(dontDestroy);
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
        Debug.Log(newScene);

        if (overWorldScenes.Contains(newScene))
        {
            Debug.Log("2");
            if (musicSource.clip != normalTransition && musicSource.clip != normalBGM)
            {
                musicSource.clip = normalTransition;
            }
            else
            {
                changingMusic = true;
            }
        }
        if (fightingScenes.Contains(newScene))
        {
            if (musicSource.clip != battleTransition && musicSource.clip != battleBGM)
            {
                musicSource.clip = battleTransition;
            }
            else
            {
                changingMusic = true;
            }
        }
        Debug.Log("3");
        if (!changingMusic)
        {
            musicSource.Play();
        }
        yield return new WaitForSeconds(musicSource.clip.length);
        musicSource.loop = true;
        StartAudio(newScene);
    }

    void StartAudio(string newScene)
    {
        Debug.Log("Starting Audio");
        Debug.Log(newScene);
        if (overWorldScenes.Contains(newScene))
        {
            Debug.Log("Audio Changed");
            musicSource.clip = normalBGM;
        }
        if (fightingScenes.Contains(newScene))
        {
            Debug.Log("Audio Changed");
            musicSource.clip = battleBGM;
        }
        if (!changingMusic)
        {
            musicSource.Play();
            Debug.Log("Audio Loop Playing");
        }
        Debug.Log("Audio Code Done");
    }

    IEnumerator OnLevelWasLoaded()
    {
        Debug.Log("hi");
        StartCoroutine(PlayTransition(SceneManager.GetActiveScene().name));
        yield return new WaitForSeconds(0.25f);
    }
}
