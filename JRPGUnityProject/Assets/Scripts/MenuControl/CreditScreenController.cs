using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CreditScreenController : MonoBehaviour {

    public void OnBackClicked()
    {
        SceneManager.LoadScene("TitleMenu");
    }
	void Update()
	{
		if (EventSystem.current.currentSelectedGameObject == null)
			EventSystem.current.SetSelectedGameObject (EventSystem.current.firstSelectedGameObject);
	}
}
