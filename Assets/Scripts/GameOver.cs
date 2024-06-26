using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public GameObject screenParent;
    public GameObject scoreParent;
    public TextMeshProUGUI scoreText;
    public UnityEngine.UI.Image[] stars;
    private bool gameWon = false;

    // Start is called before the first frame update
    void Start()
    {
        screenParent.SetActive(false);

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ShowWin(int score, int starCount)
    {
        if (gameWon) return; 

        gameWon = true; 

        Debug.Log("ShowWin called");
        screenParent.SetActive(true);

        scoreText.text = score.ToString();
        scoreText.enabled = false;

        Animator animator = GetComponent<Animator>();

        if (animator)
        {
            animator.Play("GameOverShow");
        }

        StartCoroutine(ShowWinCoroutine(starCount));
    }

    private IEnumerator ShowWinCoroutine(int starCount)
    {
        Debug.Log($"Starting ShowWinCoroutine with starCount: {starCount}");

        if (stars == null || stars.Length == 0)
        {
            Debug.LogError("Stars array is not assigned or is empty");
            yield break;
        }

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].enabled = false;
        }

        if (starCount >= stars.Length)
        {
            Debug.LogWarning("starCount exceeds the number of available stars");
            starCount = stars.Length - 1; 
        }

        for (int i = 0; i <= starCount; i++)
        {
            if (i < stars.Length)
            {
                Debug.Log($"Enabling star {i}");
                stars[i].enabled = true;

                if (i > 0)
                {
                    Debug.Log($"Disabling star {i - 1}");
                    stars[i - 1].enabled = false;
                }

                yield return new WaitForSeconds(0.5f);
            }
        }

        if (starCount < stars.Length)
        {
            stars[starCount].enabled = true;
        }

        yield return new WaitForSeconds(0.5f);
        scoreText.enabled = true;
        Debug.Log("Score text enabled");
        Time.timeScale = 0;
    }

    public void OnReplayClicked()
    {
        Time.timeScale = 1;
        gameWon = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void OnDoneClicked()
    {
        SceneManager.LoadScene("LevelOverview");
    }
}
