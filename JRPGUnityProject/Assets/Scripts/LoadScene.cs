using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {

    public string sceneLoadTileName;

    // Use this for initialization
    void Start () {
        if (sceneLoadTileName == GameStateManager.playerSpawnPoint)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = transform.position;
            PlayerMovementController pmc = 
                (PlayerMovementController) player.GetComponent("PlayerMovementController");
            pmc.LastMove = GameStateManager.playerSpawnDirection;
        }
    }

    // Update is called once per frame
    void Update () {
    
    }
}
