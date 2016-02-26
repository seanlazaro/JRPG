using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class testBattleScene : MonoBehaviour {

    public GameObject[] testEnemies;

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
            GameStateManager.possibleEnemies = testEnemies;
            SceneManager.LoadScene("Battle");
        }
    }
}
