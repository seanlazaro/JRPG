using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

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

    Battler activeBattler;
    Battler playerBattler;
    int activeBattlerIndex = 0;
    bool playerTurnChooseAction = false;

    bool startingBattle = false;
    bool choosingAction = false;
    bool doingAction = false;
    bool endingBattle = false;
    bool exitingBattle = false;

    public bool blockedByMessage = false;

    // Use this for initialization
    void Start () {
        GameObject player = GameObject.FindGameObjectWithTag("Player").transform.parent.gameObject;
        playerBattler = player.GetComponent<PlayerBattleController>();
        playerBattler.battleState = new BattleState();
        PlayerStateManager.Instance.CopyPlayerBattleState(playerBattler.battleState);

        battlers = GameObject.FindGameObjectsWithTag("Battler");
        battlers = battlers.OrderByDescending(go => go.GetComponent<Battler>().battleState.speed).ToArray();

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
                }

                if (!blockedByMessage)
                {
                    currentBattlePhase = BattlePhase.ChooseAction;
                }
                
                break;
            case BattlePhase.ChooseAction: //player always chooses action last
                if(!choosingAction)
                {                  
                    if (!blockedByMessage)
                    {                       
                        choosingAction = true;

                        if (playerTurnChooseAction)
                        {
                            StartCoroutine(playerBattler.ChooseAction(FinishChoosingAction));
                        }
                        else
                        {
                            //skip player until all non-player battlers have chosen their action
                            if (activeBattler == playerBattler)
                            {
                                activeBattlerIndex++;
                                activeBattler = battlers[activeBattlerIndex].GetComponent<Battler>();
                            }

                            StartCoroutine(activeBattler.ChooseAction(FinishChoosingAction));
                        }
                    }
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
                if(!endingBattle)
                {
                    endingBattle = true;

                    BattleState playerBattleState = PlayerStateManager.Instance.PlayerBattleState;
                    playerBattleState.statusEffects.RemoveAll(se => se.limitedDuration);
                }

                if (!blockedByMessage)
                {
                    if (!exitingBattle)
                    {
                        exitingBattle = true;
                        StartCoroutine(SceneTransitionManager.Instance.ExitBattle());
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

        if (playerTurnChooseAction)
        {
            playerTurnChooseAction = false; //reset
            
            activeBattlerIndex = 0;
            activeBattler = battlers[activeBattlerIndex].GetComponent<Battler>();
            currentBattlePhase = BattlePhase.DoAction;

            for (int i = 0; i < battlers.Length; i++)
            {
                battlers[i].GetComponent<Battler>().StartRoundAction();
            }
        }
        else
        {
            activeBattlerIndex++;
            if (activeBattlerIndex < battlers.Length)
            {
                activeBattler = battlers[activeBattlerIndex].GetComponent<Battler>();

                if (activeBattler == playerBattler)
                {
                    playerTurnChooseAction = true;
                }
            }
            else //all non-player battlers have chosen their action
            {
                playerTurnChooseAction = true;                
            }
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
            for (int i = 0; i < battlers.Length; i++)
            {
                battlers[i].GetComponent<Battler>().EndRoundAction();
            }
            
            activeBattlerIndex = 0;
            activeBattler = battlers[activeBattlerIndex].GetComponent<Battler>();
            currentBattlePhase = BattlePhase.ChooseAction;
        }
    }
}
