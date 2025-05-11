using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PostFXAdjust : MonoBehaviour
{
    public AnimationCurve cosCurve;
    public AnimationCurve sinCurve;

    private PostProcessVolume vol;
    private LensDistortion lens;
    private ChromaticAberration chrom;
    private Vignette vig;

    private Coroutine lensIntensity;
    private Coroutine lensScale;
    private Coroutine chromIntensity;
    private Coroutine vigIntensity;
    private Coroutine vigSmoothness;
    private int painScale = 0;
    // Start is called before the first frame update
    void Start()
    {
        vol = GetComponent<PostProcessVolume>();
        vol.profile.TryGetSettings(out lens);
        vol.profile.TryGetSettings(out chrom);
        vol.profile.TryGetSettings(out vig);
    }
    public void IncreasePain()
    {
        Debug.Log("Pain level: " + painScale);
        switch (painScale)
        {
            case 0:
                DistortLensIntensity();
                DistortLensScale();
                ChromaticIntensity();
                VignetteIntensity();
                VignetteSmoothness();
                painScale++;
                break;
            case 1:
                DistortLensIntensity(0f, -10f, 4f);
                DistortLensScale(0.95f, 1.05f, 4f);
                ChromaticIntensity(0f, 0.3f, 4f);
                VignetteIntensity(0f, 0.2f, 4f);
                VignetteSmoothness(1f, 1f, 4f);
                painScale++;
                break;
            case 2:
                DistortLensIntensity(-5f, -20f, 3f);
                DistortLensScale(0.95f, 1.05f, 3f);
                ChromaticIntensity(0.25f, 0.75f, 3f);
                VignetteIntensity(0.15f, 0.4f, 3f);
                VignetteSmoothness(0.9f, 1f, 3f);
                painScale++;
                break;
            case 3:
                DistortLensIntensity(-10f, -30f, 2f);
                DistortLensScale(0.95f, 1.05f, 2f);
                ChromaticIntensity(0.5f, 1f, 2f);
                VignetteIntensity(0.3f, 0.6f, 2f);
                VignetteSmoothness(0.75f, 1f, 2f);
                painScale = 0;
                break;
        }

    }
    public void DistortLensIntensity(float minIntensity = 0, float maxIntensity = 0, float cycleTime = 5f)
    {
        if (lensIntensity != null)
            StopCoroutine(lensIntensity);
        lensIntensity = StartCoroutine(LensIntensityShift(minIntensity, maxIntensity, cycleTime));
    }
    private IEnumerator LensIntensityShift(float min, float max, float cycleTime)
    {
        float timer = 0f;
        while (max != min)
        {
            if (timer < cycleTime)
                timer += Time.deltaTime;
            else
                timer = 0f;

            lens.intensity.value = Mathf.Lerp(min, max, cosCurve.Evaluate(timer / cycleTime));
            yield return null;
        }
    }
    public void DistortLensScale(float minScale = 1, float maxScale = 1, float cycleTime = 5f)
    {
        if (lensScale != null)
            StopCoroutine(lensScale);
        lensScale = StartCoroutine(LensScaleShift(minScale, maxScale, cycleTime));
    }
    private IEnumerator LensScaleShift(float min, float max, float cycleTime)
    {
        float timer = 0f;
        while (max != min)
        {
            if (timer < cycleTime)
                timer += Time.deltaTime;
            else
                timer = 0f;

            lens.scale.value = Mathf.Lerp(min, max, sinCurve.Evaluate(timer / cycleTime));
            yield return null;
        }
    }
    public void ChromaticIntensity(float minScale = 0, float maxScale = 0, float cycleTime = 5f)
    {
        if (chromIntensity != null)
            StopCoroutine(chromIntensity);
        chromIntensity = StartCoroutine(ChromIntensityShift(minScale, maxScale, cycleTime));
    }
    private IEnumerator ChromIntensityShift(float min, float max, float cycleTime)
    {
        float timer = 0f;
        cycleTime *= 1.4f;
        while (max != min)
        {
            if (timer < cycleTime)
                timer += Time.deltaTime;
            else
                timer = 0f;

            chrom.intensity.value = Mathf.Lerp(min, max, sinCurve.Evaluate(timer / cycleTime));
            yield return null;
        }
    }
    public void VignetteIntensity(float minScale = 0, float maxScale = 0, float cycleTime = 5f)
    {
        if (vigIntensity != null)
            StopCoroutine(vigIntensity);
        vigIntensity = StartCoroutine(VignetteIntensityShift(minScale, maxScale, cycleTime));
    }
    private IEnumerator VignetteIntensityShift(float min, float max, float cycleTime)
    {
        float timer = 0f;
        cycleTime *= 2.5f;
        while (max != min)
        {
            if (timer < cycleTime)
                timer += Time.deltaTime;
            else
                timer = 0f;

            vig.intensity.value = Mathf.Lerp(min, max, cosCurve.Evaluate(timer / cycleTime));
            yield return null;
        }
    }
    public void VignetteSmoothness(float minScale = 1, float maxScale = 1, float cycleTime = 5f)
    {
        if (vigSmoothness != null)
            StopCoroutine(vigSmoothness);
        vigSmoothness = StartCoroutine(VignetteSmoothnessShift(minScale, maxScale, cycleTime));
    }
    private IEnumerator VignetteSmoothnessShift(float min, float max, float cycleTime)
    {
        float timer = 0f;
        cycleTime *= 2.5f;
        while (max != min)
        {
            if (timer < cycleTime)
                timer += Time.deltaTime;
            else
                timer = 0f;

            vig.smoothness.value = Mathf.Lerp(min, max, sinCurve.Evaluate(timer / cycleTime));
            yield return null;
        }
    }
}
