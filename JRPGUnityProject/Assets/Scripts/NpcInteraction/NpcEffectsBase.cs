using UnityEngine;
using System.Collections;

[RequireComponent(typeof (DialogueController))]
public abstract class NpcEffectsBase : MonoBehaviour {

    protected DialogueController dialogueController;

	// Use this for initialization
	public void Start () {
		GameObject parent = this.gameObject;
        dialogueController = parent.GetComponent<DialogueController>();
	}

	// Update is called once per frame
	public void Update () {

		if (dialogueController.effectTriggered) {
            dialogueController.effectTriggered = false;

            int effect = dialogueController.effectFunc;

			StartCoroutine(activateNpcEffect(effect));
		}
	}

	public abstract IEnumerator activateNpcEffect(int effect);
}
