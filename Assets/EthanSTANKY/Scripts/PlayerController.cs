using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int currentSpaceIndex = 0;
    public int money = 0;
    public List<Transform> boardSpaces;
    public float moveSpeed = 3f;

    private TurnManager turnManager;

    void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
    }

    public void StartPlayerTurn()
    {
        // show Roll Button
    }

    public void RollDice()
    {
        int roll = Random.Range(1, 7);
        StartCoroutine(MoveSpaces(roll));
    }

    IEnumerator MoveSpaces(int spaces)
    {
        for (int i = 0; i < spaces; i++)
        {
            currentSpaceIndex = (currentSpaceIndex + 1) % boardSpaces.Count;
            Vector3 target = boardSpaces[currentSpaceIndex].position;

            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        ResolveSpace();
    }

    void ResolveSpace()
    {
        Transform space = boardSpaces[currentSpaceIndex];

        switch (space.tag)
        {
            case "Money":
                money += 5;
                break;

            case "LoseMoney":
                money -= 5;
                break;

            case "Explosion":
                money -= 10;
                currentSpaceIndex = Mathf.Max(0, currentSpaceIndex - 5);
                transform.position = boardSpaces[currentSpaceIndex].position;
                break;
        }

        turnManager.EndTurn();
    }
}
