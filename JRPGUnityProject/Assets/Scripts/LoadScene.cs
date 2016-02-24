using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {

    public string sceneLoadTileName;

    // Use this for initialization
    void Start () {
        if (sceneLoadTileName == PlayerStateController.spawnPoint)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = transform.position;
            PlayerMovementController pmc = 
                (PlayerMovementController) player.GetComponent("PlayerMovementController");
            pmc.LastMove = PlayerStateController.spawnDirection;
        }
    }

    // Update is called once per frame
    void Update () {
    
    }
}
