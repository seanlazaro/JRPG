using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

// Battlers include the player, allies and enemies
public abstract class Battler : MonoBehaviour {

    public BattleState battleState;
    protected Battler singleAttackTarget;

    public abstract IEnumerator ChooseAction(Action Finish);

    public virtual void StartRoundAction()
    {
    }

    public Func<Action<bool, bool>, IEnumerator> DoAction;

    public virtual void EndRoundAction()
    {
    }

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
            StartCoroutine(CombatUI.Instance.DisplayMessage("Critical Hit!", 1f));
        }
        else criticalHitFactor = 1f;

        float randomFactor = (float)(1.1d - 0.2d * r.NextDouble());
        
        return baseDamage * strengthFactor * equipmentFactor * criticalHitFactor * randomFactor;
    }
    
    // Returns true if the battler dies
    public bool TakeDamage(int dmg, string defender)
	{
		battleState.currentHealth -= dmg;

		//
		//  Take Damage Animation!
		//

        bool killed = false;

		// The UpdateHealthBar Coroutine is put in both if/else blocks to
		// prevent enemy health from being shown as a negative number.
		if (battleState.currentHealth <= 0) {	
			StartCoroutine (CombatUI.Instance.UpdateHealthBar (0, 
				(double)battleState.maximumHealth, defender == "PlayerDuringBattle"));
            killed = true;
		} 
		else {
			StartCoroutine(CombatUI.Instance.UpdateHealthBar((double)battleState.currentHealth, 
				(double)battleState.maximumHealth, defender == "PlayerDuringBattle"));
		}

        return killed;
    }

    protected IEnumerator DealDamage(float damage, Action<bool, bool> Finish)
    {
        List<statusEffect> statusEffects = singleAttackTarget.battleState.statusEffects;
        float dmgIncrease = 0;
        float dmgDecrease = 0;

        if (statusEffects.Exists(se => se.name == "Reckless"))
        {
            List<statusEffect> recklessStacks = statusEffects.FindAll(se => se.name == "Reckless");
            dmgIncrease += (0.6f * recklessStacks.Count);
        }
        if (statusEffects.Exists(se => se.name == "Cocoon"))
        {
            List<statusEffect> cocoonStacks = statusEffects.FindAll(se => se.name == "Cocoon");
            dmgDecrease += (0.5f * cocoonStacks.Count);    
        }
        if (statusEffects.Exists(se => se.name == "Shapeshifter Vulnerability"))
        {
            dmgIncrease += 1f;
        }

        float multiplier = 1 + dmgIncrease - dmgDecrease;
        if (multiplier < 0) multiplier = 0;
        damage *= multiplier;

        string attacker = this.gameObject.name;
        string defender = singleAttackTarget.gameObject.name;
        string message = string.Format("{0} dealt {1} damage to {2}.", attacker, (int)damage, defender);
        StartCoroutine(CombatUI.Instance.DisplayMessage(message, 1f));

        bool killed = singleAttackTarget.TakeDamage((int)damage, defender);

        //since a damage animation hasn't yet been implemented, wait 1 second instead
        yield return new WaitForSeconds(1f);

        if (statusEffects.Exists(se => se.name == "Charged Up"))
        {
            StartCoroutine(CombatUI.Instance.DisplayMessage("The enemy's preparation was interrupted!", 1f));
            yield return new WaitForSeconds(1f);
            statusEffects.RemoveAll(se => se.name == "Charged Up");
        }

        if (killed) singleAttackTarget.gameObject.SetActive(false);

        Finish(killed, false);
    }

    protected IEnumerator StandardAttackWithMultiplier(float multiplier, Action<bool, bool> Finish)
    {
        float damage = multiplier * CalculateStandardDamage(singleAttackTarget);

        // TODO: Animations for attack.
        // AnimateMethod(DoAction, ref bool)
        // yield return new WaitUntil(()=>bool)

        //in place of animations, there is a 2 second wait
        yield return new WaitForSeconds(2);

        StartCoroutine(DealDamage(damage, Finish));
    }

    protected IEnumerator BasicAttack(Action<bool, bool> Finish)
    {
        if (this.gameObject.name == "PlayerDuringBattle")
        {
            StartCoroutine(CombatUI.Instance.DisplayMessage("You attack the enemy!", 1f));
        }
        else
        {
            StartCoroutine(CombatUI.Instance.DisplayMessage("The enemy attacks you!", 1f));
        }

        StartCoroutine(StandardAttackWithMultiplier(1f, Finish));

        yield break;
    }
}