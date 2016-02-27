using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

    public string sceneToLoad;
    public string destinationTile;

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            SceneTransitionManager.Instance.LoadScene(sceneToLoad, destinationTile);
        }
    }
}
