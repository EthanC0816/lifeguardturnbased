using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<PlayerController> players;
    public int currentPlayerIndex = 0;

    public int totalRounds = 10;
    public int currentRound = 1;

    private int turnsTakenThisRound = 0;
    private int activePlayersAtRoundStart = 0;

    public CameraFollow followCamera;

    void Start()
    {
        if (!MainMenu.gameStarted) return;
        StartTurn();
    }

    void StartTurn()
    {
        if (!MainMenu.gameStarted) return;
        if (turnsTakenThisRound == 0)
        {
            activePlayersAtRoundStart = 0;
            foreach (var p in players)
                if (!p.hasFinished)
                    activePlayersAtRoundStart++;
        }

        followCamera.SetTarget(players[currentPlayerIndex].transform, followCamera.offset);

        Camera faceCam = GameObject.Find("FaceCamera").GetComponent<Camera>();
        faceCam.cullingMask = players[currentPlayerIndex].FaceCamMask;

        FindObjectOfType<PlayerUI>().ShowPlayerTurn(currentPlayerIndex);

        players[currentPlayerIndex].StartPlayerTurn();
    }

    public void EndTurn()
    {
        if (!MainMenu.gameStarted) return;
        players[currentPlayerIndex].isMyTurn = false;
        turnsTakenThisRound++;

     
        int finishedPlayers = 0;
        foreach (var p in players)
            if (p.hasFinished)
                finishedPlayers++;

        
        if (finishedPlayers >= players.Count - 1)
        {
            EndGame();
            return;
        }

        
        if (turnsTakenThisRound >= activePlayersAtRoundStart)
        {
            currentRound++;
            turnsTakenThisRound = 0;

            FindFirstObjectByType<PlayerUI>().UpdateRound(currentRound);


            Debug.Log($"--- ROUND {currentRound} STARTING ---");

            if (currentRound > totalRounds)
            {
                EndGame();
                return;
            }
        }

        
        do
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        }
        while (players[currentPlayerIndex].hasFinished);

        StartTurn();
    }

    void EndGame()
    {
        Debug.Log("---- GAME ENDED ----");

        foreach (var p in players)
            Debug.Log($"{p.name} finished with {p.money} coins");

        foreach (var p in players)
            p.spacebar.action.Disable();
    }
}
