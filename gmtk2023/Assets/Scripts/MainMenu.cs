using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioClip buttonSound;
    private static GameObject audioPlayer;
    private static AudioSource audioSource;
    private float musicVolume = 1f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component from the current GameObject
        audioSource.volume = musicVolume; // Set the initial volume
    }
    public void UpdateVolume(float volume)
    {
        musicVolume = volume;
        audioSource.volume = volume; // Update the volume of the AudioSource
    }
    private void Awake()
    {
        // Create an audio player GameObject if it doesn't exist
        if (audioPlayer == null)
        {
            audioPlayer = new GameObject("AudioPlayer");
            audioSource = audioPlayer.AddComponent<AudioSource>();
            // source = audioPlayer.AddComponent<AudioSource>();
            DontDestroyOnLoad(audioPlayer);
        }
        else
        {
            audioSource = audioPlayer.GetComponent<AudioSource>();
        }
    }
    private IEnumerator PlayGameWithSoundEffect()
    {
        AudioSource newSource = playSound();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.GetActiveScene().buildIndex + 1
        Destroy(newSource); // Destroy the new AudioSource after loading the scene
    }
    public void PlayGame()
    {
        StartCoroutine(PlayGameWithSoundEffect());

    }
    public void QuitGame()
    {
        AudioSource newSource = playSound();
        Application.Quit();
        Destroy(newSource);
    }
    public AudioSource playSound()
    {
        AudioSource newSource = audioPlayer.AddComponent<AudioSource>();
        newSource.PlayOneShot(buttonSound);
        return newSource;
    }
}
