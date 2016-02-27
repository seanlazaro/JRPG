using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

    public string sceneToLoad;
    public string destinationTile;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            SceneTransitionManager.Instance.LoadScene(sceneToLoad, destinationTile);
        }
    }
}
