using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton<SceneTransitionManager> {

    string destinationTile;
    public string DestinationTile
    {
        get { return destinationTile; }
    }

    // Use this for initialization
    void Start () {

    }
    
    // Update is called once per frame
    void Update () {
    
    }

    public void LoadScene(string sceneToLoad, string destinationTile)
    {
        TransitionFxManager.Fade(2f, true);

        this.destinationTile = destinationTile;

        SceneManager.LoadScene(sceneToLoad);
    }

    public void SpawnPlayer(Vector3 spawnTilePosition, Vector2 directionToFace)
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = spawnTilePosition;

        PlayerMovementController pmc =
            (PlayerMovementController)player.GetComponent("PlayerMovementController");
        pmc.OnSpawnPlayer(directionToFace);

        TransitionFxManager.Fade(2f, false);
    }
}
