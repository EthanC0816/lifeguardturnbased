using UnityEngine;

public class StartButtonController : MonoBehaviour
{
    public GameObject startButton;

    void Start()
    {
        startButton.SetActive(false);
    }

    void OnEnable()
    {
        InvokeRepeating(nameof(CheckSelection), 0f, 0.1f);
    }

    void CheckSelection()
    {
        if (MainMenu.playerCountChosen)
        {
            startButton.SetActive(true);
            CancelInvoke(nameof(CheckSelection));
        }
    }
}
