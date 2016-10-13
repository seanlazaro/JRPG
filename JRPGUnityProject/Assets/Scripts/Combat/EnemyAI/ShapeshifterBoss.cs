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

        if (battleState.currentHealth < 0.1f * battleState.maximumHealth)
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
                int n = r.Next(20);

                if (n > 16) //15% of time
                {
                    DoAction = Taunt;
                    Taunt();
                }
                else //85% of time
                {
                    DoAction = DoubleAttack;
                }
            }
            else
            {
                int n = r.Next(20);

                if (n > 16) //15% of time
                {
                    DoAction = Toxin;
                }
                else if (n > 13) //15% of time
                {
                    DoAction = Taunt;
                    Taunt();
                }
                else //70% of time
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
                    DoAction = Taunt;
                    Taunt();
                }
                else if (n > 11) //25% of time
                {
                    DoAction = Heal;
                }
                else //60% of time
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
                else if (n > 14) //15% of time
                {
                    DoAction = Taunt;
                    Taunt();
                }
                else if (n > 10) //20% of time
                {
                    DoAction = Heal;
                }
                else //55% of time
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

    //deals 2.5 times as much damage as a regular attack
    IEnumerator Rampage(Action<bool, bool> Finish)
    {
        StartCoroutine(CombatUI.Instance.DisplayMessage("Doppelganger Leader is on a rampage!", 1f));

        StartCoroutine(StandardAttackWithMultiplier(2.5f, Finish));

        yield break;
    }

    //deals 3 times as much damage as a regular attack
    IEnumerator Rush(Action<bool, bool> Finish)
    {
        StartCoroutine(CombatUI.Instance.DisplayMessage("Doppelganger Leader: Man, I'm pumped up!", 1f));

        StartCoroutine(StandardAttackWithMultiplier(3f, Finish));

        yield break;
    }

    //deals 4 times as much damage as a regular attack
    IEnumerator Taunt(Action<bool, bool> Finish)
    {
        if (battleState.currentHealth < startRoundHealth)
        {
            StartCoroutine(CombatUI.Instance.DisplayMessage("Doppelganger Leader launches a brutal counterattack!", 1f));

            StartCoroutine(StandardAttackWithMultiplier(4f, Finish));

            yield break;
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

        StartCoroutine(StandardAttackWithMultiplier(2f, Finish));

        yield break;
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

        battleState.currentHealth += (int)(0.2f * battleState.maximumHealth);

        StartCoroutine(CombatUI.Instance.UpdateHealthBar((double)battleState.currentHealth,
                (double)battleState.maximumHealth, false));

        Finish(false, false);
    }
}