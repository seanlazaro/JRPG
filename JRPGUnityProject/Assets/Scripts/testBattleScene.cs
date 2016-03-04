using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class testBattleScene : MonoBehaviour {

    public GameObject[] testEnemies;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            StartCoroutine(SceneTransitionManager.Instance.LoadBattleScene(testEnemies));
        }
    }
}
