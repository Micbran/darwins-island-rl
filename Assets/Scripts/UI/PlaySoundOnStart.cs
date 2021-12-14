using UnityEngine;

public class PlaySoundOnStart : MonoBehaviour
{
    [SerializeField] private AudioClip WinSound;

    private void Start()
    {
        AudioHelper.PlayClip2D(WinSound, 0.7f);
    }
}
