using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public static bool gameStarted = false;

    public void StartGame()
    {
        gameStarted = true;
    }
    
}
