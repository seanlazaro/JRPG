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
            // The battleState variable is only used to activate the PlayerStateManager for test purposes
            BattleState battleState = PlayerStateManager.Instance.PlayerBattleState;

            StartCoroutine(SceneTransitionManager.Instance.EnterBattle(this.gameObject));
        }
    }
}
