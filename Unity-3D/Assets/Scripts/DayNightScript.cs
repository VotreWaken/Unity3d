using System.Linq;
using UnityEngine;

public class DayNightScript : MonoBehaviour
{
    [SerializeField]
    private Material daySkyboxMaterial;
    [SerializeField]
    private Material nightSkyboxMaterial;
    private Light[] dayLights;
    private Light[] nightLights;
    private bool isDay;
    private AudioSource daySound;
    private AudioSource nightSound;

    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        daySound = audioSources[0];
        nightSound = audioSources[1];
        dayLights = GameObject.FindGameObjectsWithTag("DayLight")
            .Select(x => x.GetComponent<Light>())
            .ToArray();
        nightLights = GameObject.FindGameObjectsWithTag("NightLight")
            .Select(x => x.GetComponent<Light>())
            .ToArray();

        SwitchDayNight(true);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.N))
        {
            SwitchDayNight(!isDay);
        }
    }
    private void SwitchDayNight(bool isDay)
    {
        this.isDay = isDay;
        foreach (var light in dayLights)
        {
            light.enabled = isDay;
        }
        foreach (var light in nightLights)
        {
            light.enabled = !isDay;
        }
        RenderSettings.skybox = isDay ? daySkyboxMaterial : nightSkyboxMaterial;
        RenderSettings.ambientIntensity = isDay ? 1.0f : 0.3f;
        if (isDay){   
            nightSound.Stop();
            daySound.Play();
        }
        else{
            daySound.Stop();
            nightSound.Play();  
        }
    }
}
