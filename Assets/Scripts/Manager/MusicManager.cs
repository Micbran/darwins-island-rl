using UnityEngine;

public class MusicManager : LevelManager<MusicManager>
{
    [SerializeField] private AudioClip dungeonMusicClip;
    [SerializeField] private AudioClip bossMusicClip;
    private AudioSource current;
    public AudioSource CurrentAudioSource => this.current;

    public override void Awake()
    {
        base.Awake();
        this.current = new AudioSource();
    }

    public void OnGenerationComplete()
    {
        if (GameManager.Instance.Floor <= 6)
        {
            this.current = AudioHelper.PlayClipLoop(this.dungeonMusicClip, 0.6f);
        }
        else
        {
            this.current = AudioHelper.PlayClipLoop(this.bossMusicClip, 0.08f);
        }
    }
}
