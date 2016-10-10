using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class EnterBattle : MonoBehaviour {

    public GameObject enemyInBattle;
    public BattleState enemyBattleState;
    public bool destroyAfterBattle = true;
    public int afterBattleEffect = 0;

    void Start()
    {
        enemyBattleState.statusEffects = new List<statusEffect>();
    }

    //commented out because not used in demo
    //void OnTriggerEnter2D(Collider2D collider)
    //{
    //    if (collider.gameObject.tag == "Player")
    //    {
    //        StartBattle();
    //    }
    //}

    public void StartBattle()
    {
        StartCoroutine(SceneTransitionManager.Instance.EnterBattle(this.gameObject, 
            destroyAfterBattle, afterBattleEffect));
    }
}
