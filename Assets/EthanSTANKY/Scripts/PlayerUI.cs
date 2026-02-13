using UnityEngine;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
    public GameObject titleText;
    public GameObject startText;

    [SerializeField] private RectTransform titleTransform;
    [SerializeField] private RectTransform startTransform;

    [SerializeField] private RectTransform playerRect;
    [SerializeField] private RectTransform coinText;       
    [SerializeField] private RectTransform user;
    [SerializeField] private RectTransform coinRect;       
    [SerializeField] private TMPro.TextMeshProUGUI coinNumberText; 

    [SerializeField] private TMPro.TextMeshProUGUI roundNumberText;

    public RectTransform playerAmountText;
    public RectTransform twoPlayersButton;
    public RectTransform threePlayersButton;
    public RectTransform fourPlayersButton;


    private RectTransform coinNumberTransform;
    private RectTransform roundNumberTransform;

    public GameObject spaceToRollText;

    private bool hasShownFirstPlayer = false;

    private void Awake()
    {
        playerRect.pivot = new Vector2(0f, 0.5f);
        coinRect.pivot = new Vector2(0f, 0.5f);
        user.pivot = new Vector2(0.5f, 0f);
        coinText.pivot = new Vector2(0.5f, 0f);

        coinNumberTransform = coinNumberText.GetComponent<RectTransform>();
        roundNumberTransform = roundNumberText.GetComponent<RectTransform>();
    }

    private void Start()
    {
        playerAmountText.localScale = new Vector3(0f, 0f, 1f);
        twoPlayersButton.localScale = new Vector3(0f, 0f, 1f);
        threePlayersButton.localScale = new Vector3(0f, 0f, 1f);
        fourPlayersButton.localScale = new Vector3(0f, 0f, 1f);
        playerRect.localScale = new Vector3(0f, 1f, 1f);
        coinRect.localScale = new Vector3(0f, 1f, 1f);
        user.localScale = new Vector3(1f, 0f, 1f);
        coinText.localScale = new Vector3(1f, 0f, 1f);
        coinNumberTransform.localScale = new Vector3(1f, 0f, 1f);
        roundNumberTransform.localScale = new Vector3(1f, 0f, 1f);

        titleTransform = titleText.GetComponent<RectTransform>();
        startTransform = startText.GetComponent<RectTransform>();

        titleTransform.localScale = new Vector3(0f, 0f, 1f);
        startTransform.localScale = new Vector3(0f, 0f, 1f);

        StartCoroutine(MainMenuAnimations());
        ShowPlayerAmountMenu();
        StartCoroutine(WaitForGameStart());
    }


    private IEnumerator MainMenuAnimations()
    {
    yield return new WaitForSeconds(0.5f);
    titleTransform.LeanScale(new Vector3(1f, 1f, 1f), 0.8f).setEaseOutBack();

    yield return new WaitForSeconds(0.2f);
    twoPlayersButton.LeanScale(new Vector3(1f, 1f, 1f), 0.8f).setEaseOutBack();

    yield return new WaitForSeconds(0.2f);
    threePlayersButton.LeanScale(new Vector3(1f, 1f, 1f), 0.8f).setEaseOutBack();

    yield return new WaitForSeconds(0.2f);
    fourPlayersButton.LeanScale(new Vector3(1f, 1f, 1f), 0.8f).setEaseOutBack();

    }

    private IEnumerator WaitForGameStart()
    {
        while (!MainMenu.gameStarted)
            yield return null;

        titleTransform.LeanScale(Vector3.zero, 0.4f).setEaseInBack();
        startTransform.LeanScale(Vector3.zero, 0.4f).setEaseInBack();
        yield return new WaitForSeconds(0.4f);

        titleText.SetActive(false); 
        startText.SetActive(false);

        StartCoroutine(ShowBackgroundAfterDelay());
        StartCoroutine(ShowUsernameAfterDelay());
        StartCoroutine(ShowCoinsBackgroundAfterDelay());
        StartCoroutine(ShowCoinTextAfterDelay());
        StartCoroutine(ShowRoundTextAfterDelay());
    }

    public void ShowPlayerTurn(int playerIndex)
    {
        if (!hasShownFirstPlayer)
        {
            hasShownFirstPlayer = true;
            return;
        }

        var player = FindFirstObjectByType<TurnManager>().players[playerIndex];

        user.GetComponent<TMPro.TextMeshProUGUI>().text = $"Player {playerIndex + 1}";
        UpdateCoins(player.money);

        user.localScale = new Vector3(1f, 0f, 1f);
        PlayerUser();
    }

    public void PlayStartButtonAnimation()
    {
        startTransform.localScale = new Vector3(0f, 0f, 1f);
        startTransform.LeanScale(new Vector3(1f, 1f, 1f), 0.8f).setEaseOutBack();
    }
    public void BackToPlayerSelect()
    {
        MainMenu.playerCountChosen = false;

        ShowPlayerAmountMenu();       
        startText.SetActive(false);           
        startTransform.localScale = Vector3.zero; 
    }


    public void ShowPlayerAmountMenu()
    {
        playerAmountText.localScale = Vector3.zero;
        twoPlayersButton.localScale = Vector3.zero;
        threePlayersButton.localScale = Vector3.zero;
        fourPlayersButton.localScale = Vector3.zero;

        StartCoroutine(AnimatePlayerAmountMenuIn());
    }

    public void HidePlayerAmountMenu()
    {
        playerAmountText.LeanScale(Vector3.zero, 0.3f).setEaseInBack();
        twoPlayersButton.LeanScale(Vector3.zero, 0.3f).setEaseInBack();
        threePlayersButton.LeanScale(Vector3.zero, 0.3f).setEaseInBack();
        fourPlayersButton.LeanScale(Vector3.zero, 0.3f).setEaseInBack();
    }

    private IEnumerator AnimatePlayerAmountMenuIn()
    {
        yield return new WaitForSeconds(0.4f);
        playerAmountText.LeanScale(Vector3.one, 0.8f).setEaseOutBack();

        yield return new WaitForSeconds(0.15f);
        twoPlayersButton.LeanScale(Vector3.one, 0.8f).setEaseOutBack();

        yield return new WaitForSeconds(0.15f);
        threePlayersButton.LeanScale(Vector3.one, 0.8f).setEaseOutBack();

        yield return new WaitForSeconds(0.15f);
        fourPlayersButton.LeanScale(Vector3.one, 0.8f).setEaseOutBack();
    }

    public void UpdateCoins(int amount)
    {
        coinNumberText.text = amount.ToString();

        StartCoroutine(PlayCoinAnimation());
    }

    public void UpdateRound(int round)
    {
        roundNumberText.text = $"Round: {round}";
        RoundNumberText();
    }

    private IEnumerator ShowBackgroundAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        PlayerBackground();
    }

    private IEnumerator ShowUsernameAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        PlayerUser();
    }

    private IEnumerator ShowCoinsBackgroundAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        CoinBackground();
    }

    private IEnumerator ShowCoinTextAfterDelay()
    {
        yield return new WaitForSeconds(2.5f);
        CoinText();
        CoinNumberText();
    }

    private IEnumerator ShowRoundTextAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        RoundNumberText();
    }

    private IEnumerator PlayCoinAnimation()
    {
        LeanTween.cancel(coinNumberTransform);
        coinNumberTransform.localScale = new Vector3(1f, 0f, 1f);
        yield return null;
        CoinNumberText();
    }

    public void PlayerBackground()
    {
        playerRect.LeanScaleX(1f, 0.5f).setEaseInCirc();
    }

    public void CoinBackground()
    {
        coinRect.LeanScaleX(1f, 0.5f).setEaseInCirc();
    }

    public void CoinText()
    {
        coinText.LeanScaleY(1f, 0.5f).setEaseOutElastic();
    }

    public void CoinNumberText()
    {
        coinNumberTransform.LeanScaleY(1f, 0.5f).setEaseOutElastic();
    }

    public void RoundNumberText()
    {
        roundNumberTransform.LeanScaleY(1f, 0.5f).setEaseOutElastic();
    }

    public void PlayerUser()
    {
        user.LeanScaleY(1f, 0.5f).setEaseOutElastic();
    }
}
