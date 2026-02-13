using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static bool gameStarted = false;
    public static int playerCount = 0;
    public static bool playerCountChosen = false;
    public static int chosenRounds = 10;

    public DungeonGenerator dungeon;

    public PlayerUI ui;
    public void StartGame()
    {
        if (playerCountChosen)
        {
            gameStarted = true;

            FindFirstObjectByType<TurnManager>().ApplyPlayerCount();
        }
        ui.HidePlayerAmountMenu();
        ui.PlayStartButtonAnimation();
    }
    public void TwoPlayers()
    {
        playerCount = 2;
        playerCountChosen = true;
        ui.HidePlayerAmountMenu();
        dungeon.ActivatePlayersAndGhosts();
        ui.PlayStartButtonAnimation();
    }
    public void ThreePlayers()
    {
        playerCount = 3;
        playerCountChosen = true;
        ui.HidePlayerAmountMenu();
        dungeon.ActivatePlayersAndGhosts();
        ui.PlayStartButtonAnimation();
    }

    public void FourPlayers()
    {
        playerCount = 4;
        playerCountChosen = true;
        ui.HidePlayerAmountMenu();
        dungeon.ActivatePlayersAndGhosts();
        ui.PlayStartButtonAnimation();
    }

    public void Choose5Rounds()
    {
        chosenRounds = 5;
        dungeon.RegenerateDungeon();
    }

    public void Choose10Rounds()
    {
        chosenRounds = 10;
        dungeon.RegenerateDungeon();
    }

    public void Choose20Rounds()
    {
        chosenRounds = 20;
        dungeon.RegenerateDungeon();
    }



}
