using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour {

    public string spawnPlayerTileName;
    public Vector2 directionToFace;

    // Use this for initialization
    void Start () {
        if (spawnPlayerTileName == SceneTransitionManager.Instance.DestinationTile)
        {
            SceneTransitionManager.Instance.SpawnPlayer(transform.position, directionToFace);
        }
    }

    // Update is called once per frame
    void Update () {
    
    }
}
