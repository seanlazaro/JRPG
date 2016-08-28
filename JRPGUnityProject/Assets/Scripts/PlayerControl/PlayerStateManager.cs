using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Will contain member fields for abilities, health, status effects, money, inventory, learned techniques, etc.
public class PlayerStateManager : Singleton<PlayerStateManager> {

    public BattleState playerBattleState;

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

        playerBattleState.level = 50;

        playerBattleState.strength = 100;
        playerBattleState.endurance = 100;
        playerBattleState.accuracy = 100;
        playerBattleState.speed = 100;
        playerBattleState.deadliness = 25;

        playerBattleState.attackRating = 50;
        playerBattleState.defenceRating = 50;

        playerBattleState.currentHealth = 2500;
        playerBattleState.maximumHealth = 2500;
        playerBattleState.statusEffects = new List<statusEffect>();
    }

    public void CopyPlayerBattleState(BattleState copy)
    {
        copy.level = playerBattleState.level;

        copy.strength = playerBattleState.strength;
        copy.endurance = playerBattleState.endurance;
        copy.accuracy = playerBattleState.accuracy;
        copy.speed = playerBattleState.speed;
        copy.deadliness = playerBattleState.deadliness;

        copy.attackRating = playerBattleState.attackRating;
        copy.defenceRating = playerBattleState.defenceRating;

        copy.currentHealth = playerBattleState.currentHealth;
        copy.maximumHealth = playerBattleState.maximumHealth;

        // The status effects aren't copied. Instead, the copy receives a reference to the same
        // status effects as playerBattleState. This is currently not an issue because all temporary
        // status effects are removed at the end of battle so the status effects received by the
        // copy don't permanently affect playerBattleState.
        copy.statusEffects = playerBattleState.statusEffects;
    }
}
