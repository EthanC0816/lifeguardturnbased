using TMPro;
using UnityEngine;
using System.Collections;

public class MoneyPopup : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float floatSpeed = 1f;
    public float lifetime = 3f;

    Color baseColor;

    public void Setup(int amount)
    {
        if (amount >= 0)
        {
            text.text = $"+{amount}";
            text.color = Color.green;
        }
        else
        {
            text.text = amount.ToString();
            text.color = Color.red;
        }

        baseColor = text.color;
        StartCoroutine(FloatAndFade());
    }

    IEnumerator FloatAndFade()
    {
        float t = 0f;

        while (t < lifetime)
        {
            if (Camera.main != null)
                transform.LookAt(Camera.main.transform);

            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180f, 0);

            transform.position += Vector3.up * floatSpeed * Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, t / lifetime);
            text.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

            t += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
