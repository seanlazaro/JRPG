using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleScreenController : MonoBehaviour {

    void Start()
    {
        // Create SceneTransitionManager singleton
        SceneTransitionManager stm = SceneTransitionManager.Instance;
    }
    
    public void OnStartClicked()
    {
        // Create PlayerStateManager and GameStateManager singletons
        PlayerStateManager psm = PlayerStateManager.Instance;
        GameStateManager gsm = GameStateManager.Instance;

        GameStateManager.Instance.defeatedBruiser = false;
        GameStateManager.Instance.defeatedTank = false;
        GameStateManager.Instance.defeatedBoss = false;
        
        StartCoroutine (SceneTransitionManager.Instance.LoadFromTitleScreen());
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
