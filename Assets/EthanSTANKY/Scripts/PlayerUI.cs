using UnityEngine;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private RectTransform playerRect;     
    [SerializeField] private RectTransform coinText;       
    [SerializeField] private RectTransform user;           
    [SerializeField] private RectTransform coinRect;       

    private void Awake()
    {
        
        if (playerRect == null) playerRect = GameObject.Find("PlayerTitleBackground").GetComponent<RectTransform>();
        if (user == null) user = GameObject.Find("PlayerName").GetComponent<RectTransform>();
        if (coinRect == null) coinRect = GameObject.Find("CoinsBackground").GetComponent<RectTransform>();
        if (coinText == null) coinText = GameObject.Find("CoinsText").GetComponent<RectTransform>();

        
        playerRect.pivot = new Vector2(0f, 0.5f);
        coinRect.pivot = new Vector2(0f, 0.5f);
        user.pivot = new Vector2(0.5f, 0f);
        coinText.pivot = new Vector2(0.5f, 0f);
    }

    private void Start()
    {
        
        playerRect.localScale = new Vector3(0f, 1f, 1f);
        coinRect.localScale = new Vector3(0f, 1f, 1f);
        user.localScale = new Vector3(1f, 0f, 1f);
        coinText.localScale = new Vector3(1f, 0f, 1f);

        // Timed animations
        StartCoroutine(ShowBackgroundAfterDelay());
        StartCoroutine(ShowUsernameAfterDelay());
        StartCoroutine(ShowCoinsBackgroundAfterDelay());
        StartCoroutine(ShowCoinTextAfterDelay());
    }

    public void ShowPlayerTurn(int playerIndex)
    {
        user.GetComponent<TMPro.TextMeshProUGUI>().text = $"Player {playerIndex + 1}";

        user.localScale = new Vector3(1f, 0f, 1f);
        PlayerUser();
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

    public void PlayerUser()
    {
        user.LeanScaleY(1f, 0.5f).setEaseOutElastic();
    }
}
