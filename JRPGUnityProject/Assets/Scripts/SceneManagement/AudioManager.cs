using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

	// Stores music player.
    public AudioSource musicSource;
    public static AudioManager Instance;

	// Stores music and corresponding scenes.
	[Header("Same Length, Clip Corresponds To Scene")]
	public string[] scenesString;
	public AudioClip[] clips;

	// Used to prevent the clip from restarting even if the clip didn't change.
    private bool changingMusic = true;

    void Awake()
    {
		// Singleton code.
        if (Instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }

		// Prevents awkward behavior.
        musicSource.playOnAwake = false;
        musicSource.rolloffMode = AudioRolloffMode.Logarithmic;
        musicSource.loop = true;
    }

    void Start () 
    {
		// Plays initial scene music.
		StartAudio(SceneManager.GetActiveScene().name);

		// Displays warning if arrays are not configured properly.
		if (clips.Length != scenesString.Length)
			Debug.LogWarning ("Audio Manager: Length of 'clips' array does not match the length of the 'scenes' array.");
    }


	void StartAudio(string newScene)
	{
		// Resets changingMusic bool.
		changingMusic = false;

		// If the current scene contains the clip, find scene and play corresponding clip.
		if ((scenesString.Contains (newScene))) {
			for (int i = 0; i < clips.Length; i++) {
				if (scenesString [i] == newScene) {

					// Prevents clip from restarting if the music doesn't change.
					if (musicSource.clip != clips [i]) {
						musicSource.clip = clips [i];
						changingMusic = true;
					}
					break;
				}
			}
		} else // Triggered when there is no music on given scene.
			musicSource.Stop ();
        if (changingMusic)
        {
            musicSource.Play();
        }
    }

    IEnumerator OnLevelWasLoaded()
    {
		// Updates audio.
		if (scenesString.Contains (SceneManager.GetActiveScene ().name)) {
			StartAudio (SceneManager.GetActiveScene ().name);
		}
        yield break;
    }
		
	public IEnumerator AudioFade(float fadeTime, bool fadeOut)
	{
		// Based on the fadescreen code, but uses musicSource.volume
		// instead of alpha values.

		bool increasing;
		float totalChange = 0;

		if (fadeOut)
			increasing = false;
		else 
			increasing = true;
		
		while(!(totalChange >= 1))
		{
			if (increasing) {
				musicSource.volume = 0;
				if (musicSource.isPlaying) {
					musicSource.Pause ();
				}
			}
			else
				musicSource.volume -= Time.deltaTime / fadeTime;
			totalChange += Time.deltaTime / fadeTime;
			yield return new WaitForEndOfFrame();
		}
		if (increasing) {
			musicSource.volume = 1;
			musicSource.Play ();
		}
		yield break;
	}

}
