using UnityEngine;
using System.Collections;

// This script can be used to manage information such as total time played, save states, etc.
public class GameStateManager : Singleton<GameStateManager>
{
    public bool defeatedBruiser = false;
    public bool defeatedTank = false;
    public bool defeatedBoss = false;

    public bool fightingBoss = false;
    public bool wonGame = false;
}
