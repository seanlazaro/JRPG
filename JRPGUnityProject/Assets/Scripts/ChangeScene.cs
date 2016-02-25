using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public string sceneToLoad;
    public string sceneLoadTile;
    public Vector2 directionToFace;

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
            GameStateController.playerSpawnPoint = sceneLoadTile;
            GameStateController.playerSpawnDirection = directionToFace;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
