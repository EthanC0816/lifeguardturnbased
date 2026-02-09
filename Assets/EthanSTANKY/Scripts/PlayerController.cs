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
    public bool hasFinished = false;

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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        turnManager = FindFirstObjectByType<TurnManager>();
        rb.MovePosition(boardSpaces[currentSpaceIndex].position);
    }

    void OnRoll(InputAction.CallbackContext ctx)
    {
        if (!isMyTurn) return;

        spacebar.action.Disable();   // Prevent double rolls
        RollDice();
    }

    public void StartPlayerTurn()
    {
        isMyTurn = true;
        spacebar.action.Enable();
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
                currentSpaceIndex = boardSpaces.Count - 1;

            Vector3 target = boardSpaces[currentSpaceIndex].position;

            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                Vector3 direction = (target - transform.position).normalized;

                // Rotate only when moving forward
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

    IEnumerator MoveBackwards(int spaces)
    {
        int targetIndex = Mathf.Max(0, currentSpaceIndex - spaces);

        while (currentSpaceIndex > targetIndex)
        {
            currentSpaceIndex--;

            Vector3 target = boardSpaces[currentSpaceIndex].position;

            // Move backwards at double speed, no rotation
            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target,
                    moveSpeed * 2f * Time.deltaTime
                );

                yield return null;
            }
        }

        // After backward movement, resolve the new tile
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
                StartCoroutine(MoveBackwards(5));
                return; 

            case "Start":
                Debug.Log("Player is on the start space");
                break;

            case "End":
                Debug.Log("Player reached the end!");
                break;
        }

        // If player reached the end tile
        if (currentSpaceIndex == boardSpaces.Count - 1)
        {
            hasFinished = true;
            isMyTurn = false;
            turnManager.EndTurn();
            return;
        }

        // Normal end of turn
        isMyTurn = false;
        turnManager.EndTurn();
    }
}
