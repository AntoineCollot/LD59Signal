using UnityEngine;

public class SignalAudio : MonoBehaviour
{
    [SerializeField] float lowPitch;
    [SerializeField] float highPitch;
    AudioSource source;

    float baseVolume;
    float volume, refVolume;
    const float SMOOTH_VOLUME = 0.1f;

    void Start()
    {
        source = GetComponent<AudioSource>();
        baseVolume = source.volume;
    }

    void Update()
    {
        float targetVolume = 1;
        if (!GameManager.Instance.GameIsPlaying)
            targetVolume = 0;

        volume = Mathf.SmoothDamp(volume, targetVolume * baseVolume, ref refVolume, SMOOTH_VOLUME);
        float freq = Curves.QuintEaseOut(1, 0, PlayerAim.Instance.AimDist01);
        source.pitch = Mathf.Lerp(lowPitch,highPitch, freq);
        source.volume = volume;
    }
}
