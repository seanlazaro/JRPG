using UnityEngine;
using System.Collections;

public class SpawnDoppelganger : MonoBehaviour {

    //should have only one game object
    //array used to comply with implementation of scene transition manager's spawn enemy sprite
    public GameObject[] spriteToSpawn; 
    public string spriteToSpawnId;

    void Start()
    {
        switch (spriteToSpawnId)
        {
            case "bruiser":
                if (!GameStateManager.Instance.defeatedBruiser)
                {
                    SceneTransitionManager.Instance.SpawnEnemySprite(spriteToSpawn, transform.position);
                }
                break;
            case "tank":
                if (!GameStateManager.Instance.defeatedTank)
                {
                    SceneTransitionManager.Instance.SpawnEnemySprite(spriteToSpawn, transform.position);
                }
                break;
            case "boss":
                if (!GameStateManager.Instance.defeatedBoss)
                {
                    SceneTransitionManager.Instance.SpawnEnemySprite(spriteToSpawn, transform.position);
                }
                break;
            default:
                //do nothing
                break;
        }
        
    }

}
