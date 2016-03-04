using UnityEngine;
using System.Collections;

public class SpawnEnemyInBattle : MonoBehaviour {

    // Use this for initialization
    void Start () {
        StartCoroutine(SceneTransitionManager.Instance.SpawnEnemyInBattle(transform.position));
    }
    
    // Update is called once per frame
    void Update () {
    
    }
}
