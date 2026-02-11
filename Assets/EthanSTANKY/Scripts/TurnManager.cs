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
        StartTurn();
    }

    void StartTurn()
    {
       
        if (turnsTakenThisRound == 0)
        {
            activePlayersAtRoundStart = 0;
            foreach (var p in players)
                if (!p.hasFinished)
                    activePlayersAtRoundStart++;
        }

        followCamera.SetTarget(players[currentPlayerIndex].transform, followCamera.offset);

        players[currentPlayerIndex].StartPlayerTurn();
    }

    public void EndTurn()
    {
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
