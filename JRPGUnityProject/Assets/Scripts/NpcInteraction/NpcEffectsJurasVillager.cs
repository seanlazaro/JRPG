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
                dialogueController.EndDialogue();
                this.gameObject.GetComponent<EnterBattle>().StartBattle();
                break;
            case 3:
                dialogueController.Reinitialize();
                StartCoroutine(this.gameObject.GetComponent<DialogueController>().StartDialogue());
                break;
            default:
                break;
        }

        yield break;
    }
}