using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ShapeshifterBoss : Battler
{
    int startRoundHealth;
    int numConsecutiveTurnsWhereHpDecreased = 0;

    public override IEnumerator ChooseAction(Action Finish)
    {
        System.Random r = new System.Random();

        GameObject player = GameObject.FindWithTag("Player");
        singleAttackTarget = player.transform.parent.gameObject.GetComponent<Battler>();
        List<statusEffect> playerStatusEffects = singleAttackTarget.battleState.statusEffects;

        if (battleState.statusEffects.Exists(se => se.name == "Charged Up"))
        {
            DoAction = ChargedAttack;
        }
        else if (battleState.currentHealth < 0.1f * battleState.maximumHealth)
        {
            DoAction = Rampage;
        }
        else if (numConsecutiveTurnsWhereHpDecreased > 2)
        {
            DoAction = Rush;
        }
        else if (battleState.currentHealth > 0.75f * battleState.maximumHealth)
        {
            if (playerStatusEffects.Exists(se => se.name == "Shapeshifter Toxin"))
            {
                int n = r.Next(4);

                if (n == 3) //25% of time
                {
                    DoAction = Charge;
                }
                else if (n == 2) //25% of time
                {
                    DoAction = Taunt;
                    Taunt();
                }
                else //50% of time
                {
                    DoAction = DoubleAttack;
                }
            }
            else
            {
                int n = r.Next(20);

                if (n > 16) //15% of time
                {
                    DoAction = Charge;
                }
                else if (n > 13) //15% of time
                {
                    DoAction = Toxin;
                }
                else if (n > 8) //25% of time
                {
                    DoAction = Taunt;
                    Taunt();
                }
                else //45% of time
                {
                    DoAction = BasicAttack;
                }
            }
        }
        else
        {
            if (playerStatusEffects.Exists(se => se.name == "Shapeshifter Toxin"))
            {
                int n = r.Next(20);

                if (n > 16) //15% of time
                {
                    DoAction = Charge;
                }
                else if (n > 13) //15% of time
                {
                    DoAction = Heal;
                }
                else if (n > 9) //20% of time
                {
                    DoAction = Taunt;
                    Taunt();
                }
                else //50% of time
                {
                    DoAction = DoubleAttack;
                }
            }
            else
            {
                int n = r.Next(20);

                if (n > 17) //10% of time
                {
                    DoAction = Toxin;
                }
                else if (n > 15) //10% of time
                {
                    DoAction = Charge;
                }
                else if (n > 12) //15% of time
                {
                    DoAction = Heal;
                }
                else if (n > 8) //20% of time
                {
                    DoAction = Taunt;
                    Taunt();
                }
                else //45% of time
                {
                    DoAction = BasicAttack;
                }
            }
        }

        Finish();

        yield break;
    }

    public override void StartRoundAction()
    {
        startRoundHealth = battleState.currentHealth;
    }

    public override void EndRoundAction()
    {
        if (battleState.currentHealth < startRoundHealth)
        {
            numConsecutiveTurnsWhereHpDecreased++;
        }
        else
        {
            numConsecutiveTurnsWhereHpDecreased = 0;
        }
    }

    void Taunt()
    {
        string[] taunts = new string[3]
        {
            "[name]: This isn't even my final form. Attack me and I'll show you my true power.",
            "[name]: You've got no chance of beating me unless you come at me with everything you've got!",
            "[name]: Show me your strongest attack! Or else I'll get bored and end this fight quickly."
        };
        
        System.Random r = new System.Random();
        int n = r.Next(taunts.Length);
        StartCoroutine(CombatUI.Instance.DisplayBlockingMessage(taunts[n]));
    }

    // Charges for one turn then attacks for three times standard damage the next turn. If attacked 
    // during the charging turn, the attack is cancelled.

    IEnumerator Charge(Action<bool, bool> Finish)
    {
        StartCoroutine(CombatUI.Instance.DisplayMessage("The enemy begins setting up an attack.", 1f));

        statusEffect se = new statusEffect("Charged Up", true, false, 1, false);
        battleState.statusEffects.Add(se);

        //in place of animations, there is a 2 second wait
        yield return new WaitForSeconds(2);

        Finish(false, false);
    }

    IEnumerator ChargedAttack(Action<bool, bool> Finish)
    {
        if (battleState.statusEffects.Exists(se => se.name == "Charged Up"))
        {
            StartCoroutine(CombatUI.Instance.DisplayMessage("[name]: Take this!", 1f));

            battleState.statusEffects.RemoveAll(se => se.name == "Charged Up");

            float damage = 3f * CalculateStandardDamage(singleAttackTarget);

            // TODO: Animations for attack.
            // AnimateMethod(DoAction, ref bool)
            // yield return new WaitUntil(()=>bool)

            //in place of animations, there is a 2 second wait
            yield return new WaitForSeconds(2);

            StartCoroutine(DealDamage(damage, Finish));
        }
        else
        {
            Finish(false, false);
        }
    }

    //deals 2.5 times as much damage as a regular attack
    IEnumerator Rampage(Action<bool, bool> Finish)
    {
        StartCoroutine(CombatUI.Instance.DisplayMessage("[name] is on a rampage!", 1f));

        float damage = 2.5f * CalculateStandardDamage(singleAttackTarget);

        // TODO: Animations for attack.
        // AnimateMethod(DoAction, ref bool)
        // yield return new WaitUntil(()=>bool)

        //in place of animations, there is a 2 second wait
        yield return new WaitForSeconds(2);

        StartCoroutine(DealDamage(damage, Finish));
    }

    //deals 3 times as much damage as a regular attack
    IEnumerator Rush(Action<bool, bool> Finish)
    {
        StartCoroutine(CombatUI.Instance.DisplayMessage("[name]: Man, I'm pumped up!", 1f));

        float damage = 3f * CalculateStandardDamage(singleAttackTarget);

        // TODO: Animations for attack.
        // AnimateMethod(DoAction, ref bool)
        // yield return new WaitUntil(()=>bool)

        //in place of animations, there is a 2 second wait
        yield return new WaitForSeconds(2);

        StartCoroutine(DealDamage(damage, Finish));
    }

    //deals 4 times as much damage as a regular attack
    IEnumerator Taunt(Action<bool, bool> Finish)
    {
        if (battleState.currentHealth < startRoundHealth)
        {
            StartCoroutine(CombatUI.Instance.DisplayMessage("[name] launches a brutal counterattack!", 1f));

            float damage = 4f * CalculateStandardDamage(singleAttackTarget);

            // TODO: Animations for attack.
            // AnimateMethod(DoAction, ref bool)
            // yield return new WaitUntil(()=>bool)

            //in place of animations, there is a 2 second wait
            yield return new WaitForSeconds(2);

            StartCoroutine(DealDamage(damage, Finish));
        }
        else
        {
            Finish(false, false);
        }
    }

    //deals twice as much damage as a regular attack
    IEnumerator DoubleAttack(Action<bool, bool> Finish)
    {
        StartCoroutine(CombatUI.Instance.DisplayMessage("The enemy strikes twice at lightning speed!", 1f));

        float damage = 2f * CalculateStandardDamage(singleAttackTarget);

        // TODO: Animations for attack.
        // AnimateMethod(DoAction, ref bool)
        // yield return new WaitUntil(()=>bool)

        //in place of animations, there is a 2 second wait
        yield return new WaitForSeconds(2);

        StartCoroutine(DealDamage(damage, Finish));
    }

    //applies a permanent debuff to the player, which allows the use of DoubleAttack
    IEnumerator Toxin(Action<bool, bool> Finish)
    {
        StartCoroutine(CombatUI.Instance.DisplayMessage(
            "[name]: Let's see if you can keep up with me after you breathe in my poison. Mwuhahaha!", 2f));

        statusEffect se = new statusEffect("Shapeshifter Toxin", false, false, 0, true);

        GameObject player = GameObject.FindWithTag("Player");
        singleAttackTarget = player.transform.parent.gameObject.GetComponent<Battler>();
        singleAttackTarget.battleState.statusEffects.Add(se);

        //in place of animations, there is a 2 second wait
        yield return new WaitForSeconds(2);

        Finish(false, false);
    }

    //increases hp by 25% of max hp, but causes double damage to be taken next turn
    IEnumerator Heal(Action<bool, bool> Finish)
    {
        StartCoroutine(CombatUI.Instance.DisplayMessage(
            "The enemy momentarily lowers their guard to heal.", 1f));

        statusEffect se = new statusEffect("Shapeshifter Vulnerability", true, false, 1, true);
        battleState.statusEffects.Add(se);

        //in place of animations, there is a 2 second wait
        yield return new WaitForSeconds(2);

        battleState.currentHealth += (int)(0.25f * battleState.maximumHealth);

        StartCoroutine(CombatUI.Instance.UpdateHealthBar((double)battleState.currentHealth,
                (double)battleState.maximumHealth, false));

        Finish(false, false);
    }
}