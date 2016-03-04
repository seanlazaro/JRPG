using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton<SceneTransitionManager> {

    // Prevent use of constructor
    protected SceneTransitionManager() { }
    
    const float fadeTime = 1f;
    const float battleFadeTime = 1f;
    
    string destinationTile;
    public string DestinationTile
    {
        get { return destinationTile; }
    }

    GameObject[] possibleEnemies;

    public IEnumerator LoadScene(string sceneToLoad, string destinationTile)
    {
        StartCoroutine(TransitionEffects.Instance.Fade(fadeTime, true));
        yield return new WaitForSeconds(fadeTime);

        this.destinationTile = destinationTile;

        SceneManager.LoadScene(sceneToLoad);
    }

    public IEnumerator SpawnPlayer(Vector3 spawnPosition, Vector2 directionToFace)
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = spawnPosition;

        PlayerMovementController pmc =
            (PlayerMovementController)player.GetComponent("PlayerMovementController");
        pmc.OnSpawnPlayer(directionToFace);

        StartCoroutine(TransitionEffects.Instance.Fade(fadeTime, false));
        yield return null;
    }

    public IEnumerator LoadBattleScene(GameObject[] possibleEnemies)
    {
        StartCoroutine(TransitionEffects.Instance.Fade(battleFadeTime, true));
        yield return new WaitForSeconds(battleFadeTime);

        this.possibleEnemies = possibleEnemies;

        SceneManager.LoadScene("Battle");
    }

    public IEnumerator SpawnEnemyInBattle(Vector2 spawnPosition)
    {
        System.Random r = new System.Random();
        int i = r.Next(possibleEnemies.Length);

        GameObject enemy = Instantiate(possibleEnemies[i]);
        enemy.transform.position = spawnPosition;

        StartCoroutine(TransitionEffects.Instance.Fade(battleFadeTime, false));
        yield return null;
    }
}
