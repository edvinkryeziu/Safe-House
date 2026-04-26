using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance {get; private set;}
    private Volume volume;
    public VolumeProfile[] qualityProfiles;
    public float brightness;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        volume = GetComponent<Volume>();
        int qualityIndex = PlayerPrefs.GetInt("QualityLevel");
        volume.profile = qualityProfiles[qualityIndex];
        brightness = PlayerPrefs.GetFloat("Brightness", 0f);
        if (volume.profile.TryGet<ColorAdjustments>(out ColorAdjustments colorAdjustments))
        {
            colorAdjustments.postExposure.value = brightness;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBrightness(float value)
    {
        if (volume.profile.TryGet<ColorAdjustments>(out ColorAdjustments colorAdjustments))
        {
            colorAdjustments.postExposure.value = value;
            brightness = value;
        }
    }
}
