using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public bool gameStarted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameStarted)
        gameStarted = false;
    }

    
    void Update()
    {
        
    }
}
