using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnterBattle : MonoBehaviour {

    public GameObject enemyInBattle;
    public BattleState enemyBattleState;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            // The battleState variable is only used to activate the PlayerStateManager for test purposes
            BattleState battleState = PlayerStateManager.Instance.PlayerBattleState;
            //

            StartCoroutine(SceneTransitionManager.Instance.EnterBattle(this.gameObject));
        }
    }
}
