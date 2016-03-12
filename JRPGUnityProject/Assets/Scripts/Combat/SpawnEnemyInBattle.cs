using UnityEngine;
using System.Collections;

public class SpawnEnemyInBattle : MonoBehaviour {

    void Start () {
        StartCoroutine(SceneTransitionManager.Instance.SpawnEnemyInBattle(transform.position));
    }
    
    // Update is called once per frame
    void Update () {
    
    }
}
