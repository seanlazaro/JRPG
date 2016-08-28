using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleScreenController : MonoBehaviour {

    public void OnStartClicked()
    {
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
