using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<PlayerController> players;
    public int currentPlayerIndex = 0;
    void Start()
    {
        StartTurn();
    }

   void StartTurn()
    {
        players[currentPlayerIndex].StartPlayerTurn();
    }

    public void EndTurn()
    {
        players[currentPlayerIndex].isMyTurn = false;

        currentPlayerIndex = (currentPlayerIndex + 1 ) % players.Count;
        StartTurn();
    }

}
