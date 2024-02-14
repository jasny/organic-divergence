using UnityEngine;

public class AudioController : MonoBehaviour, IBeatTimeSubject
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip song;

    public void Start()
    {
        BeatTimer.Instance.Register(this);
    }

    public void OnBeat(int beatCount)
    {
        if (source.isPlaying && beatCount > 0) return;
        StartAudioTrack(song, BeatTimer.Instance.BeatInterval * (beatCount - 1));
    }

    private void StartAudioTrack(AudioClip track, float time)
    {
        source.Stop();
        source.clip = track;
        source.time = time;
        source.Play();
    }

    public void StopAudioTrack()
    {
        if (!source.isPlaying) return;
        
        source.Stop();
        source.clip = null;
    }
}
