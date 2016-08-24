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
            else if (xSpeed == ySpeed)
            {
                //if two battlers have the same speed, their turn order will be randomly chosen
                System.Random r = new System.Random();
                if (r.Next(2) == 1) return 1;
                else return -1;
            }
            else return 1;
        }
    }

    Battler activeBattler;
    int activeBattlerIndex = 0;

    bool startingBattle = false;
    bool choosingAction = false;
    bool doingAction = false;
    bool exitingBattle = false;

    bool blockedByMessage = false;
    public bool blockingMessageReleased = false;

    // Use this for initialization
    void Start () {
        GameObject player = GameObject.FindGameObjectWithTag("Player").transform.parent.gameObject;
        player.GetComponent<PlayerBattleController>().battleState = new BattleState();
        PlayerStateManager.Instance.CopyPlayerBattleState(
            player.GetComponent<PlayerBattleController>().battleState);

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
                if (!startingBattle)
                {
                    startingBattle = true;

                    for (int j = 0; j < battlers.Length; j++)
                    {
                        StartCoroutine(CombatUI.Instance.UpdateHealthBar(
                            (double)battlers[j].GetComponent<Battler>().battleState.currentHealth,
                            (double)battlers[j].GetComponent<Battler>().battleState.maximumHealth,
                            battlers[j].name == "PlayerDuringBattle"));
                    }

                    StartCoroutine(CombatUI.Instance.DisplayBlockingMessage("The battle has started."));

                    blockedByMessage = true;
                }

                if (blockedByMessage)
                {
                    if (blockingMessageReleased)
                    {
                        currentBattlePhase = BattlePhase.ChooseAction;
                        
                        //reset
                        blockedByMessage = false;
                        blockingMessageReleased = false;
                    }
                }
                
                break;
            case BattlePhase.ChooseAction:
                if(!choosingAction)
                {
                    choosingAction = true;
                    StartCoroutine(activeBattler.ChooseAction(FinishChoosingAction));
                }
                break;
            case BattlePhase.DoAction:
                if(!doingAction)
                {
                    doingAction = true;
                    StartCoroutine(activeBattler.DoAction(FinishDoingAction));
                }
                break;
            case BattlePhase.BattleEnd:
                if(!exitingBattle)
                {
                    exitingBattle = true;

                    BattleState playerBattleState = PlayerStateManager.Instance.PlayerBattleState;
                    playerBattleState.statusEffects.RemoveAll(se => se.limitedDuration);
                }

                if (blockedByMessage)
                {
                    if (blockingMessageReleased)
                    {
                        StartCoroutine(SceneTransitionManager.Instance.ExitBattle());

                        //reset
                        blockedByMessage = false;
                        blockingMessageReleased = false;
                    }
                }

                break;
            default:
                break;
        }
    }

    void FinishChoosingAction()
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

    void FinishDoingAction(bool deathOccurred, bool speedChanged)
    {
        doingAction = false;

        //reverse iteration over the list so that elements can be removed while iterating
        for (int i = activeBattler.battleState.statusEffects.Count - 1; i >= 0; i--)
        {
            statusEffect se = activeBattler.battleState.statusEffects[i];

            if (se.limitedDuration)
            {
                if (!se.startedDuration)
                {
                    se.startedDuration = true;
                }
                else
                {
                    se.numberOfTurnsRemaining--;

                    if (se.numberOfTurnsRemaining == 0)
                    {
                        activeBattler.battleState.statusEffects.Remove(se);
                    }
                }
            }
        }

        if (deathOccurred)
        {
            GameObject player = GameObject.FindWithTag("Player");
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (player == null)
            {
                StartCoroutine(CombatUI.Instance.DisplayBlockingMessage("You've been wrecked..."));
            }
            else if (enemies.Length == 0)
            {
                StartCoroutine(CombatUI.Instance.DisplayBlockingMessage("You've wonnerino!"));
            }
            blockedByMessage = true;

            currentBattlePhase = BattlePhase.BattleEnd;
            return;
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
