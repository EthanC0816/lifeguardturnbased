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
    private bool movedBackwards = false;


    private Rigidbody rb;
    private Camera faceCam;
    private Transform faceCamPos;

    public LayerMask FaceCamMask;

    [Header("InputSystem")]
    public InputActionReference spacebar;

    [Header("Money Effects")]
    public MoneyPopup moneyPopupPrefab;
    public Vector3 popupOffset = new Vector3(0, 1f, 0);


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
        if (!MainMenu.gameStarted) return;
        rb = GetComponent<Rigidbody>();
        turnManager = FindFirstObjectByType<TurnManager>();
        faceCam = GameObject.Find("FaceCamera").GetComponent<Camera>();
        faceCamPos = transform.Find("FaceCamPos");
        if (!faceCam.enabled)
            faceCam.gameObject.SetActive(true);
        DungeonGenerator gen = FindFirstObjectByType<DungeonGenerator>();
        boardSpaces = gen.boardSpaces;
    }


    void OnRoll(InputAction.CallbackContext ctx)
    {
        if (!MainMenu.gameStarted) return;

        if (!isMyTurn) return;
        spacebar.action.Disable();  
        RollDice();

    }

    void LateUpdate()
    {
        if (isMyTurn && faceCamPos != null)
        {
            faceCam.transform.position = faceCamPos.position;
            faceCam.transform.rotation = faceCamPos.rotation;
        }
    }


    public void StartPlayerTurn()
    {
        isMyTurn = true;
        spacebar.action.Enable();
    }

    public void RollDice()
    {
        if (!isMyTurn) return;
        int roll = Random.Range(1, 7);
        DiceUI.Instance.ShowRoll(roll);
        Debug.Log($"Player {turnManager.currentPlayerIndex + 1} rolled: {roll}");
        StartCoroutine(MoveSpaces(roll));
    }
    void ChangeMoney(int amount)
    {
        int oldMoney = money;
        money = Mathf.Max(0, money + amount);
        int moneyDifference = money - oldMoney;

        if (moneyDifference == 0)
            return;

        if (moneyPopupPrefab != null)
        {
            MoneyPopup popup = Instantiate(moneyPopupPrefab, transform.position + popupOffset, Quaternion.identity);
            popup.Setup(moneyDifference);
        }
        FindFirstObjectByType<PlayerUI>().UpdateCoins(money);

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

        yield return StartCoroutine(ResolveSpace());


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
                transform.position = Vector3.MoveTowards(transform.position,target,moveSpeed * 2f * Time.deltaTime);

                yield return null;
            }
        }


        movedBackwards = true;
        yield return StartCoroutine(ResolveSpace());
        movedBackwards = false;

    }

    IEnumerator ResolveSpace()
    {
        Transform space = boardSpaces[currentSpaceIndex];
        string spaceTag = space.parent != null ? space.parent.tag : space.tag;

        if (movedBackwards && spaceTag == "Start")
        {
            // Skip start tile effects when moving backwards
            yield return new WaitForSeconds(1f);

            isMyTurn = false;
            spacebar.action.Disable();
            turnManager.EndTurn();
            yield break;
        }



        switch (spaceTag)
        {
            case "Money":
                ChangeMoney(+3);
                break;

            case "LoseMoney":
                ChangeMoney(-5);
                break;

            case "Explosion":
                ChangeMoney(-10);
                yield return StartCoroutine(MoveBackwards(4));
                yield break;

            case "Start":
                Debug.Log("Player is on the start space");
                break;

            case "End":
                Debug.Log("Player reached the end!");
                break;
        }

        // Pause before ending turn
        yield return new WaitForSeconds(1.5f);

        if (currentSpaceIndex == boardSpaces.Count - 1)
        {
            hasFinished = true;
            isMyTurn = false;
            turnManager.EndTurn();
            yield break;
        }

        isMyTurn = false;
        spacebar.action.Disable();
        turnManager.EndTurn();
    }

}
