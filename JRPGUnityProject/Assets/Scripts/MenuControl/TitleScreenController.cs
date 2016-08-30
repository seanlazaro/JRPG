using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleScreenController : MonoBehaviour {

    public void OnStartClicked()
    {
		StartCoroutine (StartGame ());
    }

	IEnumerator StartGame()
	{
		StartCoroutine(AudioManager.Instance.AudioFade(1,true));
		yield return new WaitForSeconds (1);
		StartCoroutine(AudioManager.Instance.AudioFade(0.01f,false));
		SceneManager.LoadScene("Prototype Town");
	}

	public void OnCreditsClicked()
	{
		SceneManager.LoadScene("Credits");
	}

	public void OnInstructionsClicked()
	{
		SceneManager.LoadScene("Instructions");
	}

	void Update()
	{
		if (EventSystem.current.currentSelectedGameObject == null)
			EventSystem.current.SetSelectedGameObject (EventSystem.current.firstSelectedGameObject);
	}
}
