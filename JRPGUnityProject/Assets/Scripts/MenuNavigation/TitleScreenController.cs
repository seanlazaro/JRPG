using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour {

    public void OnStartClicked()
    {
        SceneManager.LoadScene("main");
    }

    public void OnCreditsClicked()
    {
        SceneManager.LoadScene("Credits");
    }
}
