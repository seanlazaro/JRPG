using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DialogueController))]
public class NpcEffectsJurasVillager : NpcEffectsBase
{
    public override IEnumerator activateNpcEffect(int effect)
    {
        switch (effect)
        {
            //give player option to accuse the npc of being a doppelganger
            case 1:
                AccuseDoppelganger ad = this.gameObject.GetComponent<AccuseDoppelganger>();

                dialogueController.npcName = "Game";
                dialogueController.afterDialogueEffectTriggered = true;
                dialogueController.afterDialogueEffectFunc = 2;

                StartCoroutine(ad.Accuse(dialogueController.npcNameReal));
                break;
            //reset the npc to be accused again next time you talk to him
            case 2:
                dialogueController.npcName = dialogueController.npcNameReal;
                dialogueController.afterDialogueEffectTriggered = true;
                dialogueController.afterDialogueEffectFunc = 1;
                break;
            //player wrongfully accused an innocent villager of being a doppelganger
            case 3:
                dialogueController.afterDialogueEffectTriggered = true;
                dialogueController.afterDialogueEffectFunc = 4;
                break;
            case 4:
                StartCoroutine(SceneTransitionManager.Instance.EndGame(false));
                break;
            //player chose not to accuse a doppelganger npc of being a doppelganger, so reset
            case 5:
                dialogueController.afterDialogueEffectTriggered = true;
                dialogueController.afterDialogueEffectFunc = 2;
                break;
            //start pre-battle conversation with Dwight
            case 6:
                dialogueController.npcName = dialogueController.npcNameReal;
                dialogueController.afterDialogueEffectTriggered = true;
                dialogueController.afterDialogueEffectFunc = 9;

                StartCoroutine(dialogueController.StartDialogue(null, 4));
                break;
            //start pre-battle conversation with LaMarcus
            case 7:
                dialogueController.npcName = dialogueController.npcNameReal;
                dialogueController.afterDialogueEffectTriggered = true;
                dialogueController.afterDialogueEffectFunc = 9;

                StartCoroutine(dialogueController.StartDialogue(null, 7));
                break;
            //start pre-battle conversation with Darko
            case 8:
                dialogueController.npcName = dialogueController.npcNameReal;
                dialogueController.afterDialogueEffectTriggered = true;
                dialogueController.afterDialogueEffectFunc = 9;

                StartCoroutine(dialogueController.StartDialogue(null, 3));
                break;
            //start battle
            case 9:
                this.gameObject.GetComponent<EnterBattle>().StartBattle();
                break;
            //start post-battle conversation with Dwight
            case 10:
                dialogueController.Reinitialize();

                dialogueController.afterDialogueEffectTriggered = true;
                dialogueController.afterDialogueEffectFunc = 13;

                StartCoroutine(this.gameObject.GetComponent<DialogueController>().StartDialogue(null, 6));

                GameStateManager.Instance.defeatedBruiser = true;
                break;
            //start post-battle conversation with LaMarcus
            case 11:
                dialogueController.Reinitialize();

                dialogueController.afterDialogueEffectTriggered = true;
                dialogueController.afterDialogueEffectFunc = 13;

                StartCoroutine(this.gameObject.GetComponent<DialogueController>().StartDialogue(null, 9));

                GameStateManager.Instance.defeatedTank = true;
                break;
            //start post-battle conversation with Darko
            case 12:
                //hide the enemy sprite
                this.gameObject.GetComponent<Renderer>().enabled = false;

                dialogueController.Reinitialize();

                dialogueController.npcName = "Game";
                dialogueController.afterDialogueEffectTriggered = true;
                dialogueController.afterDialogueEffectFunc = 14;

                StartCoroutine(this.gameObject.GetComponent<DialogueController>().StartDialogue(null, 6));
                break;
            //destroy defeated enemy
            case 13:
                GameObject player = GameObject.Find("Player");
                player.GetComponent<PlayerSpriteController>().RemoveFromNpcList(this.gameObject);
                SceneTransitionManager.Instance.RemoveFromEnemySpriteList(this.gameObject);
                Destroy(this.gameObject);
                break;
            //player has defeated final boss, completing the game
            case 14:
                StartCoroutine(SceneTransitionManager.Instance.EndGame(true));
                break;
            default:
                break;
        }

        yield break;
    }
}