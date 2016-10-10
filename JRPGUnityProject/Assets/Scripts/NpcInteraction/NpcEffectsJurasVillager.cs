using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DialogueController))]
public class NpcEffectsJurasVillager : NpcEffectsBase
{
    public override IEnumerator activateNpcEffect(int effect)
    {
        switch (effect)
        {
            case 1:
                StartCoroutine(SceneTransitionManager.Instance.GoToGameOver());
                break;
            case 2:
                this.gameObject.GetComponent<EnterBattle>().StartBattle();
                break;
            default:
                break;
        }

        yield break;
    }
}