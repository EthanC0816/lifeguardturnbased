using UnityEngine;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private RectTransform user;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        rect.pivot = new Vector2(0f, 0.5f); // LEFT center
        user.pivot = new Vector2(0.5f, 0f);
    }

    private void Start()
    {
        rect.localScale = new Vector3(0f, 1f, 1f);
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

    public void PlayerBackground()
    {
        rect.LeanScaleX(1f, 0.5f).setEaseInCirc();
    }

    public void PlayerUser()
    {
        user.LeanScaleY(1f, 0.5f).setEaseOutElastic();
    }


}
