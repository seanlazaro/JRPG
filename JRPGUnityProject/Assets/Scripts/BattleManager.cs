using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {

    enum BattleState
    {
        BattleStart,
        PlayerAction,
        EnemyAction,
        ActionResult,
        BattleEnd
    }
    Dictionary<int, BattleState> battleStateDict = new Dictionary<int, BattleState>();
    BattleState currentBattleState;
    Animator battleStateManager;

    // Use this for initialization
    void Start () {
        battleStateManager = GetComponent<Animator>();
        BattleState[] battleStates = (BattleState[]) System.Enum.GetValues(typeof(BattleState));

        foreach (BattleState battleState in battleStates)
        {
            int battleStateHash = Animator.StringToHash(battleState.ToString());
            battleStateDict.Add(battleStateHash, battleState);
        }
    }
    
    // Update is called once per frame
    void Update () {
        // GetCurrentAnimatorStateInfo(0) gets info from Base Layer, which has a layer index of 0
        int currentBattleStateHash = battleStateManager.GetCurrentAnimatorStateInfo(0).shortNameHash;
        currentBattleState = battleStateDict[currentBattleStateHash];

        switch (currentBattleState)
        {
            case BattleState.BattleStart:
                break;
            case BattleState.PlayerAction:
                break;
            case BattleState.EnemyAction:
                break;
            case BattleState.ActionResult:
                break;
            case BattleState.BattleEnd:
                break;
            default:
                break;
        }
    }
}
