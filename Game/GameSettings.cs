using UnityEngine;
using UnityEngine.Rendering;

public class GameSettings : MonoBehaviour
{
    private Volume volume;
    public VolumeProfile[] qualityProfiles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volume = GetComponent<Volume>();
        int qualityIndex = PlayerPrefs.GetInt("QualityLevel");
        volume.profile = qualityProfiles[qualityIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
