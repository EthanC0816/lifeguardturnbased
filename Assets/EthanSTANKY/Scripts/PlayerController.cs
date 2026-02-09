using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int currentSpaceIndex = 0;
    public int money = 0;
    public List<Transform> boardSpaces;
    public float moveSpeed = 3f;

    private TurnManager turnManager;
    public bool isMyTurn = false;


    private Rigidbody rb;

    [Header("InputSystem")]

    public InputActionReference spacebar;

    private void OnEnable()
    {
        spacebar.action.performed += OnRoll;
        spacebar.action.Enable();
    }

    private void OnDisable()
    {
        spacebar.action.performed -= OnRoll;
        spacebar.action.Disable();
    }

    void OnRoll(InputAction.CallbackContext ctx)
    {
        if (!isMyTurn) return;
        RollDice();

    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        turnManager = FindFirstObjectByType<TurnManager>();
        rb.MovePosition(boardSpaces[currentSpaceIndex].position);
    }

    public void StartPlayerTurn()
    {
        isMyTurn = true;
    }

    public void RollDice()
    {
        int roll = Random.Range(1, 7);
        Debug.Log($"Player {turnManager.currentPlayerIndex + 1} rolled: {roll}");
        StartCoroutine(MoveSpaces(roll));
    }

    IEnumerator MoveSpaces(int spaces)
    {
        for (int i = 0; i < spaces; i++)
        {
            
            currentSpaceIndex++;

            
            if (currentSpaceIndex >= boardSpaces.Count - 1)
            {
                currentSpaceIndex = boardSpaces.Count - 1;
            }

            Vector3 target = boardSpaces[currentSpaceIndex].position;

            
            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                Vector3 direction = (target - transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
                }

                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                yield return null;
            }

           
            if (currentSpaceIndex == boardSpaces.Count - 1)
                break;
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

            case "Start":
                Debug.Log($"Player is on the start space");
                break;

            case "End":
                Debug.Log($"Player reached the end!");
                break;

        }

        if (currentSpaceIndex == boardSpaces.Count - 1)
        {
            Debug.Log("Player reached the end!");
            turnManager.EndTurn();
            isMyTurn = false;
            return;
        }


      
    }
}
