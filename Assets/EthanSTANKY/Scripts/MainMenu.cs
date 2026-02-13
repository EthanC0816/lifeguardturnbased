using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public static bool gameStarted = false;
    [SerializeField] public static int playerCount = 0;
    [SerializeField] public static bool playerCountChosen = false;

    public PlayerUI ui;
    public void StartGame()
    {
        if (playerCountChosen)
            gameStarted = true;
        ui.HidePlayerAmountMenu();
        ui.PlayStartButtonAnimation();
    }
    public void TwoPlayers()
    {
        playerCount = 2;
        playerCountChosen = true;
        ui.HidePlayerAmountMenu();
        ui.PlayStartButtonAnimation();
    }
    public void ThreePlayers()
    {
        playerCount = 3;
        playerCountChosen = true;
        ui.HidePlayerAmountMenu();
        ui.PlayStartButtonAnimation();
    }

    public void FourPlayers()
    {
        playerCount = 4;
        playerCountChosen = true;
        ui.HidePlayerAmountMenu();
        ui.PlayStartButtonAnimation();
    }


}
