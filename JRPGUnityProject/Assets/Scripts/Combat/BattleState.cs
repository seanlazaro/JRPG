using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// Will be a private member of PlayerStateManager and a public member of ally sprites,
// enemy sprites, PlayerBattleController, allies in battle and enemies in battle.
[System.Serializable]
public class BattleState {

    public int level;

    public int strength;
    public int endurance;
    public int accuracy;
    public int speed;
    public int deadliness;

    public int attackRating;
    public int defenceRating;

    public int currentHealth;
    public int maximumHealth;
    public List<statusEffect> statusEffects;
}

public class statusEffect
{
    public string name;
    public bool limitedDuration;
    public bool startedDuration;
    public int numberOfTurnsRemaining;
    public bool debuff;

    public statusEffect(string name, bool limitedDuration, bool startedDuration, int numberOfTurnsRemaining, bool debuff)
    {
        this.name = name;
        this.limitedDuration = limitedDuration;
        this.startedDuration = startedDuration;
        this.numberOfTurnsRemaining = numberOfTurnsRemaining;
        this.debuff = debuff;
    }
}