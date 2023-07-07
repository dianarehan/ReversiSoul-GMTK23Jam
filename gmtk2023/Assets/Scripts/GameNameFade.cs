/*using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameNameFade : MonoBehaviour
{
    public float fadeDuration = 2f;  // Duration of the fade effect in seconds
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = 1f - (elapsedTime / fadeDuration);
            canvasGroup.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;  // Ensure the alpha is fully faded out
        // Proceed to the next step (showing story images)
    }
}
*/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameNameFade : MonoBehaviour
{
    public float fadeDuration = 2f;  // Duration of the fade effect in seconds
    public Image storyImage;
    public Sprite[] storyImages;
    public KeyCode continueKey = KeyCode.Return;  // Keycode for the Enter key
    public string mainMenuSceneName = "MainMenu";

    private CanvasGroup canvasGroup;
    private int currentIndex = 0;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(ShowTextFadeSequence());
    }

    private IEnumerator ShowTextFadeSequence()
    {
        // Show and fade the game name text
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = 1f - (elapsedTime / fadeDuration);
            canvasGroup.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;  // Ensure the alpha is fully faded out

        // Show the story images
        StartCoroutine(ShowStoryImages());
    }

    private IEnumerator ShowStoryImages()
    {
        yield return new WaitForSeconds(1f);  // Optional delay between text fade and image display

        foreach (Sprite image in storyImages)
        {
            storyImage.sprite = image;
            yield return new WaitForSeconds(1f);  // Delay between each image
        }

        // Transition to the main menu
        TransitionToMainMenu();
    }

    private void TransitionToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
