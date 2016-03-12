using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BattleManager : MonoBehaviour {

    enum BattlePhase
    {
        BattleStart,
        ChooseAction,
        DoAction,
        BattleEnd
    }
    BattlePhase currentBattlePhase = BattlePhase.BattleStart;

    // Stores battlers from greatest to least speed (determines order at start of battle)
    // Any action that changes the speed of a battler should resort
    GameObject[] battlers;

    class SpeedComparer : IComparer
    {
        int IComparer.Compare(System.Object x, System.Object y)
        {
            GameObject gameObj = (GameObject)x;
            int xSpeed = gameObj.GetComponent<Battler>().battleState.speed;

            gameObj = (GameObject)y;
            int ySpeed = gameObj.GetComponent<Battler>().battleState.speed;

            if (xSpeed > ySpeed) return -1;
            else if (xSpeed == ySpeed) return 0;
            else return 1;
        }
    }

    Battler activeBattler;
    int activeBattlerIndex = 0;

    bool choosingAction = false;
    bool doingAction = false;

    // Use this for initialization
    void Start () {
        GameObject player = GameObject.FindGameObjectWithTag("Player").transform.parent.gameObject;
        player.GetComponent<PlayerBattleController>().battleState =
            PlayerStateManager.Instance.PlayerBattleState;

        battlers = GameObject.FindGameObjectsWithTag("Battler");
        IComparer speedComparer = new SpeedComparer();
        Array.Sort(battlers, speedComparer);

        activeBattler = battlers[activeBattlerIndex].GetComponent<Battler>();
    }
    
    // Update is called once per frame
    void Update () {
        switch (currentBattlePhase)
        {
            case BattlePhase.BattleStart:
                Debug.Log("The battle has started.");
                currentBattlePhase = BattlePhase.ChooseAction;
                break;
            case BattlePhase.ChooseAction:
                if(!choosingAction)
                {
                    choosingAction = true;
                    StartCoroutine(activeBattler.ChooseAction(chooseActionCallback));
                }
                break;
            case BattlePhase.DoAction:
                if(!doingAction)
                {
                    doingAction = true;
                    StartCoroutine(activeBattler.DoAction(doActionCallback));
                }
                break;
            case BattlePhase.BattleEnd:
                SceneTransitionManager.Instance.ExitBattle();
                break;
            default:
                break;
        }
    }

    void chooseActionCallback()
    {
        choosingAction = false;
        activeBattlerIndex++;
        if (activeBattlerIndex < battlers.Length)
        {
            activeBattler = battlers[activeBattlerIndex].GetComponent<Battler>();
        }
        else
        {
            activeBattlerIndex = 0;
            activeBattler = battlers[activeBattlerIndex].GetComponent<Battler>();
            currentBattlePhase = BattlePhase.DoAction;
        }
    }

    void doActionCallback(bool deathOccurred, bool speedChanged)
    {
        doingAction = false;

        if (deathOccurred)
        {
            currentBattlePhase = BattlePhase.BattleEnd;

            GameObject player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                Debug.Log("You've been wrecked...");
                return;
            }

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                Debug.Log("You've wonnerino!");
                return;
            }
        }

        activeBattlerIndex++;
        if (activeBattlerIndex < battlers.Length)
        {
            activeBattler = battlers[activeBattlerIndex].GetComponent<Battler>();
        }
        else
        {
            activeBattlerIndex = 0;
            activeBattler = battlers[activeBattlerIndex].GetComponent<Battler>();
            currentBattlePhase = BattlePhase.ChooseAction;
        }
    }
}
