using UnityEngine;
using System.Collections;
using System;

public class BasicAttackEnemy : Battler {

    public override IEnumerator ChooseAction(Action Finish)
    {
        yield return null;
        DoAction = BasicAttack;

        GameObject player = GameObject.FindWithTag("Player");
        singleAttackTarget = player.transform.parent.gameObject.GetComponent<Battler>();

        Finish();
    }
}
