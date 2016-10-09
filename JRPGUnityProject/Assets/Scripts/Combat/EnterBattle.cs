using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class EnterBattle : MonoBehaviour {

    public GameObject enemyInBattle;
    public BattleState enemyBattleState;

    void Start()
    {
        enemyBattleState.statusEffects = new List<statusEffect>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            StartCoroutine(SceneTransitionManager.Instance.EnterBattle(this.gameObject));
        }
    }
}
