using UnityEngine;
using System.Collections;

[RequireComponent(typeof (DialogueController))]
public class NpcEffectsChangeExitTileDestinations : NpcEffectsBase {

	public GameObject[] exitTiles;
	public string[] destination;

	public override IEnumerator activateNpcEffect(int effect)
	{
		// No use for effect variable here because there is only one type of effect

        for (int i = 0; i < exitTiles.Length; i++)
        {
            exitTiles[i].GetComponent<ChangeScene>().sceneToLoad = destination[i];
        }
		yield break;
	}
}
