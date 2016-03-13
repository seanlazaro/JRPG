using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Will contain member fields for abilities, health, status effects, money, inventory, learned techniques, etc.
public class PlayerStateManager : Singleton<PlayerStateManager> {
    BattleState playerBattleState;
    // To do: write a setter for PlayerBattleState which sets only valid values (e.g. abilities are
    // between 1 and 250 inclusive)
    public BattleState PlayerBattleState
    {
        get { return playerBattleState; }
        set { playerBattleState = value; }
    }

    // After loading game from a save file is implemented, remove Start()
    void Start()
    {
        playerBattleState = new BattleState();

        playerBattleState.level = 1;

        playerBattleState.strength = 2;
        playerBattleState.endurance = 2;
        playerBattleState.accuracy = 2;
        playerBattleState.speed = 2;
        playerBattleState.deadliness = 2;

        playerBattleState.attackRating = 1;
        playerBattleState.defenceRating = 1;

        playerBattleState.currentHealth = 20;
        playerBattleState.maximumHealth = 20;
        playerBattleState.statusEffects = new List<bool>();
    }
}
