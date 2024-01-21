using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class VignetteIntensityController : MonoBehaviour
{
    public float fadeDuration = 5f;
    public float minIntensity = 0f;
    public float maxIntensity = 1f;

    private UnityEngine.Rendering.Universal.Vignette vignette;
    public Volume volume;

    void Start()
    {
        // Get the Vignette effect from the Post-Processing Layer
        volume.profile.TryGet(out vignette);

        // Start the intensity decrease
        StartCoroutine(DecreaseIntensityOverTime());
    }

    IEnumerator DecreaseIntensityOverTime()
    {
        
        float elapsedTime = 0f;
        Debug.Log(vignette);
        Debug.Log(vignette.intensity);
        float startIntensity = (float)vignette.intensity;

        while (elapsedTime < fadeDuration)
        {
            // Calculate the new intensity using Lerp
            float newIntensity = Mathf.Lerp(startIntensity, minIntensity, elapsedTime / fadeDuration);

            // Set the Vignette intensity
            vignette.intensity.value = newIntensity;

            // Wait for the next frame
            yield return null;

            // Update the elapsed time
            elapsedTime += Time.deltaTime;
        }

        // Ensure the final intensity is exactly the minimum value
        vignette.intensity.value = minIntensity;
    }
}
