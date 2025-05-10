using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PostFXAdjust : MonoBehaviour
{
    private PostProcessVolume vol;
    private LensDistortion lens;
    private ChromaticAberration chrom;
    private Vignette vig;
    
    // Start is called before the first frame update
    void Start()
    {
        vol = GetComponent<PostProcessVolume>();
        vol.profile.TryGetSettings(out lens);
        vol.profile.TryGetSettings(out chrom);
        vol.profile.TryGetSettings(out vig);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
