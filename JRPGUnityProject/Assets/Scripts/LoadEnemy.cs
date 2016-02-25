using UnityEngine;
using System.Collections;

public class LoadEnemy : MonoBehaviour {

    GameObject[] enemies;

    // Use this for initialization
    void Start () {
        enemies = GameStateController.possibleEnemies;

        System.Random r = new System.Random();
        int i = r.Next(enemies.Length);

        GameObject enemy = Instantiate(enemies[i]);
        enemy.transform.position = transform.position;
    }
    
    // Update is called once per frame
    void Update () {
    
    }
}
