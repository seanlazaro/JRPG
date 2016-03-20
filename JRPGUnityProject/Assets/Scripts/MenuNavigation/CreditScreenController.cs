using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreditScreenController : MonoBehaviour {

    public void OnBackClicked()
    {
        SceneManager.LoadScene("TitleMenu");
    }
}
