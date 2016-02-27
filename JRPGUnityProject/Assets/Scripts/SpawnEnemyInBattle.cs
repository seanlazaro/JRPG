using UnityEngine;
using System.Collections;

public class SpawnEnemyInBattle : MonoBehaviour {

    GameObject[] enemies;

    // Use this for initialization
    void Start () {
        SceneTransitionManager.Instance.SpawnEnemyInBattle(transform.position);
    }
    
    // Update is called once per frame
    void Update () {
    
    }
}
