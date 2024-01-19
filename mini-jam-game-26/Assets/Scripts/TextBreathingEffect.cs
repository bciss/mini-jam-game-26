using UnityEngine;
using TMPro;

public class TextBreathingEffect : MonoBehaviour
{
    public TMP_Text textComponent;
    public float breathSpeed = 1.0f;
    public float minAlpha = 0.5f;
    public float maxAlpha = 1.0f;


    void Update()
    {
        // Calculate the new alpha value based on the breathing effect
        float newAlpha = Mathf.PingPong(Time.time * breathSpeed, maxAlpha - minAlpha) + minAlpha;

        // Apply the new alpha value to the text component
        SetTextAlpha(newAlpha);
    }

    void SetTextAlpha(float alpha)
    {
        Color textColor = textComponent.color;
        textColor.a = alpha;
        textComponent.color = textColor;
    }
}