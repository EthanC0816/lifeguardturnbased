using UnityEngine;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
    private RectTransform playerRect;
    [SerializeField] private RectTransform user;
    [SerializeField] private RectTransform coinRect;

    private void Awake()
    {
        playerRect = GetComponent<RectTransform>();
        playerRect.pivot = new Vector2(0f, 0.5f); // LEFT center
        user.pivot = new Vector2(0.5f, 0f);
    }

    private void Start()
    {
        playerRect.localScale = new Vector3(0f, 1f, 1f);
        user.localScale = new Vector3(1f,0f,1f);
        StartCoroutine(ShowBackgroundAfterDelay());
        StartCoroutine(ShowUsernameAfterDelay());
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
        yield return new WaitForSeconds(2f);
        PlayerBackground();
    }

    public void PlayerBackground()
    {
        playerRect.LeanScaleX(1f, 0.5f).setEaseInCirc();
    }

    public void PlayerUser()
    {
        user.LeanScaleY(1f, 0.5f).setEaseOutElastic();
    }


}
