using UnityEngine;
using System.Collections;
using System;

public class PlayerBattleController : Battler {

    public override IEnumerator ChooseAction(Action Finish)
    {
        // Show GUI allowing player to choose an action
        // Only option is Basic Attack until techniques are implemented
        // After techniques are implemented, remove Basic Attack

        // Until GUI is implemented, automatically choose Basic Attack
        yield return new WaitForSeconds(3);
        DoAction = BasicAttack;

        GameObject enemy = GameObject.FindWithTag("Enemy");
        singleAttackTarget = enemy.transform.parent.gameObject.GetComponent<Battler>();

        Finish();
    }
}
