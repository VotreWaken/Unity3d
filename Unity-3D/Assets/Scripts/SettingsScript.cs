using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Slider effectsVolumeSlider;

    [SerializeField]
    private Slider ambientVolumeSlider;

    [SerializeField]
    private Slider musicVolumeSlider;

    private GameObject content;

    void Start()
    {
        content = transform.Find("Content").gameObject;
        Time.timeScale = content.activeInHierarchy ? 0f : 1f;
        OnEffectSlider(effectsVolumeSlider.value);
        OnAmbientSlider(ambientVolumeSlider.value);
        OnMusicSlider(musicVolumeSlider.value);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Time.timeScale = content.activeInHierarchy ? 1f : 0f;
            content.SetActive(!content.activeInHierarchy);
        }
    }

    public void OnEffectSlider(float value)
    {
        float dB = Mathf.Lerp(-80, 10f, value);
        audioMixer.SetFloat("EffectsVolume", dB);
        // Debug.Log(value);
    }

    public void OnAmbientSlider(float value)
    {
        float dB = Mathf.Lerp(-80, 10f, value);
        audioMixer.SetFloat("AmbientVolume", dB);
        // Debug.Log(value);
    }

    public void OnMusicSlider(float value)
    {
        float dB = Mathf.Lerp(-80, 10f, value);
        audioMixer.SetFloat("MusicVolume", dB);
        // Debug.Log(value);
    }

    public void OnMuteAllToggle(bool isMutted)
    {
        float dB = isMutted ? -80f : 0f;
        audioMixer.SetFloat("MasterVolume", dB);
    }
}
