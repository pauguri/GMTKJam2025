using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource introMusic;
    public AudioSource loopMusic;

    private void Start()
    {
        // Singleton check
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist between scenes

        introMusic.Play();
        loopMusic.PlayDelayed(introMusic.clip.length);
    }
}

