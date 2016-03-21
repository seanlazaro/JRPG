using UnityEngine;
using System.Collections;
using System;

// Battlers include the player, allies and enemies
public abstract class Battler : MonoBehaviour {

    public BattleState battleState;
    public Func<Action<bool, bool>, IEnumerator> DoAction;

    public abstract IEnumerator ChooseAction(Action Finish);

    protected Battler singleAttackTarget;

    protected float CalculateStandardDamage(Battler attackTarget)
    {
        float baseDamage = (float)(Math.Pow(battleState.level, 2d) + 19d) / 5f;

        float strengthFactor = (float)battleState.strength / (battleState.level * 2);

        int equipmentDifference = battleState.attackRating - attackTarget.battleState.defenceRating;
        float equipmentFactor = 3f / (float)Math.PI *
            (float)Math.Atan(0.075d * equipmentDifference - 0.57d) + 1.5f;        

        float criticalHitFactor;
        float criticalHitChance = battleState.deadliness / 250f;
        System.Random r = new System.Random();
        if (criticalHitChance > r.NextDouble())
        {
            criticalHitFactor = 3f;
            Debug.Log("Critical hit!");
        }
        else criticalHitFactor = 1f;

        float randomFactor = (float)(1.1d - 0.2d * r.NextDouble());
        
        return baseDamage * strengthFactor * equipmentFactor * criticalHitFactor * randomFactor;
    }
    
    // Returns true if the battler dies
    // Animations for taking damage will be placed in here
    public bool TakeDamage(int dmg)
    {
        battleState.currentHealth -= dmg;

        if (battleState.currentHealth <= 0) return true;
        else
        {
            string message = string.Format("Remaining health: {0}/{1}", battleState.currentHealth, 
                battleState.maximumHealth);
            Debug.Log(message);
            return false;
        }
    }

    protected IEnumerator BasicAttack(Action<bool, bool> Finish)
    {
        yield return new WaitForSeconds(1);

        float damage = CalculateStandardDamage(singleAttackTarget);


        string attacker = this.gameObject.name;
        string defender = singleAttackTarget.gameObject.name;
        string message = string.Format("{0} dealt {1} damage to {2}.", attacker, (int)damage, defender);
        Debug.Log(message);


        bool killed = singleAttackTarget.TakeDamage((int)damage);
        if (killed) singleAttackTarget.gameObject.SetActive(false);

        Finish(killed, false);
    }
}
