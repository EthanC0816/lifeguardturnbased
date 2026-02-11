using System.Collections;
using TMPro;
using UnityEngine;

public class DiceUI : MonoBehaviour
{
    public static DiceUI Instance;

    public TextMeshProUGUI diceText;
    Vector3 startPos;

    public ParticleSystem spawnParticles;

    private void Awake()
    {
        Instance = this;
        startPos = diceText.rectTransform.position;
        diceText.gameObject.SetActive(false);
    }

   public void ShowRoll(int value)
    {
        diceText.text = value.ToString();

        if (value <= 2)
            diceText.color = Color.red;
        else if (value <= 4)
            diceText.color = Color.orange;
        else
            diceText.color = Color.green;

        diceText.fontSize = 677f;
        diceText.alpha = 1f;
        diceText.gameObject.SetActive(true);

        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
    {
        float duration = 2f;
        float floatSpeed = 100f;
        
        float t = 0;

        
        spawnParticles.startColor = diceText.color * Color.gray;
        spawnParticles.Play();

        while (t < duration)
        {
            t += Time.deltaTime;

            diceText.rectTransform.position += Vector3.up * floatSpeed * Time.deltaTime;

            if (t >= duration / 2f)
            {
                float fadeT = (t - duration / 2f) / (duration / 2f);
                diceText.alpha = Mathf.Lerp(1f, 0f, fadeT);
            }

            yield return null;

        }
        diceText.rectTransform.position = startPos;
        diceText.gameObject.SetActive(false);

    }
    
}
