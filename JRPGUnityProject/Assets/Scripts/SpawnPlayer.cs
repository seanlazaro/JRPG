using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour {

    public string spawnPlayerTileName;
    public Vector2 directionToFace;

    // Use this for initialization
    void Start () {
        if (spawnPlayerTileName == SceneTransitionManager.Instance.DestinationTile)
        {
            StartCoroutine(SceneTransitionManager.Instance.SpawnPlayer(transform.position, directionToFace));
        }
    }
}
