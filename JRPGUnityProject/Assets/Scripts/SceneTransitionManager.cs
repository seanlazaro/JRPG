using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton<SceneTransitionManager> {

    // Prevent use of constructor
    protected SceneTransitionManager() { }
    
    string destinationTile;
    public string DestinationTile
    {
        get { return destinationTile; }
    }

    GameObject[] possibleEnemies;

    public void LoadScene(string sceneToLoad, string destinationTile)
    {
        StartCoroutine(LoadSceneCoroutine(sceneToLoad, destinationTile));
    }

    IEnumerator LoadSceneCoroutine(string sceneToLoad, string destinationTile)
    {
        TransitionFxManager.Fade(0.75f, true);
        yield return new WaitForSeconds(0.75f);

        this.destinationTile = destinationTile;

        SceneManager.LoadScene(sceneToLoad);
    }

    public void SpawnPlayer(Vector3 spawnPosition, Vector2 directionToFace)
    {
        StartCoroutine(SpawnPlayerCoroutine(spawnPosition, directionToFace));
    }

    IEnumerator SpawnPlayerCoroutine(Vector3 spawnPosition, Vector2 directionToFace)
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = spawnPosition;

        PlayerMovementController pmc =
            (PlayerMovementController)player.GetComponent("PlayerMovementController");
        pmc.OnSpawnPlayer(directionToFace);

        TransitionFxManager.Fade(0.75f, false);
        yield return null;
    }

    public void LoadBattleScene(GameObject[] possibleEnemies)
    {
        StartCoroutine(LoadBattleSceneCoroutine(possibleEnemies));
    }

    IEnumerator LoadBattleSceneCoroutine(GameObject[] possibleEnemies)
    {
        TransitionFxManager.Fade(0.4f, true);
        yield return new WaitForSeconds(0.4f);

        this.possibleEnemies = possibleEnemies;

        SceneManager.LoadScene("Battle");
    }

    public void SpawnEnemyInBattle(Vector2 spawnPosition)
    {
        StartCoroutine(SpawnEnemyInBattleCoroutine(spawnPosition));
    }

    IEnumerator SpawnEnemyInBattleCoroutine(Vector2 spawnPosition)
    {
        TransitionFxManager.Fade(0.4f, false);
        yield return new WaitForSeconds(0.4f);

        System.Random r = new System.Random();
        int i = r.Next(possibleEnemies.Length);

        GameObject enemy = Instantiate(possibleEnemies[i]);
        enemy.transform.position = spawnPosition;
    }
}
