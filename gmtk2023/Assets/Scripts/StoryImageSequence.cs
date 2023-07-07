/*using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryImageSequence : MonoBehaviour
{
    public Image storyImage;
    public Sprite[] storyImages;
    public KeyCode continueKey = KeyCode.Return;  // Keycode for the Enter key

    private int currentIndex = 0;

    private void Start()
    {
        ShowNextImage();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Continue"))
        {
            if (currentIndex < storyImages.Length)
                ShowNextImage();
            else
                TransitionToStartMenu();
        }
    }

    private void ShowNextImage()
    {
        storyImage.sprite = storyImages[currentIndex];
        currentIndex++;
    }

    private void TransitionToStartMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
*/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StoryImageSequence : MonoBehaviour
{
    public Image storyImage;
    public Sprite[] storyImages;
    public float fadeInDuration = 1f;  // Duration of the fade-in effect in seconds
    public float fadeOutDuration = 1f;  // Duration of the fade-out effect in seconds
    public float imageDisplayDuration = 3f;  // Duration to display each image in seconds
    public string mainMenuSceneName = "MainMenu";

    private int currentIndex = 0;

    private void Start()
    {
        StartCoroutine(DisplayStoryImages());
    }

    private IEnumerator DisplayStoryImages()
    {
        // Fade in the first image
        yield return StartCoroutine(FadeImage(storyImage, 0f, 1f, fadeInDuration));

        // Display the first image
        storyImage.sprite = storyImages[currentIndex];
        currentIndex++;

        // Wait for the specified image display duration
        yield return new WaitForSeconds(imageDisplayDuration);

        // Loop through the remaining images
        for (int i = currentIndex; i < storyImages.Length; i++)
        {
            // Fade out the previous image
            yield return StartCoroutine(FadeImage(storyImage, 1f, 0f, fadeOutDuration));

            // Set the image sprite to the current story image
            storyImage.sprite = storyImages[i];

            // Fade in the image
            yield return StartCoroutine(FadeImage(storyImage, 0f, 1f, fadeInDuration));

            // Wait for the specified image display duration
            yield return new WaitForSeconds(imageDisplayDuration);
        }

        // Fade out the last image
        yield return StartCoroutine(FadeImage(storyImage, 1f, 0f, fadeOutDuration));

        // Transition to the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private IEnumerator FadeImage(Image image, float startAlpha, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color startColor = image.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            image.color = Color.Lerp(startColor, targetColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = targetColor;  // Ensure the target alpha is set correctly
    }
}


