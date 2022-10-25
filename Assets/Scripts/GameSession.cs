using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int numOfHearts = 3;
    [SerializeField] int score = 0;
    [SerializeField] Text ScoreText;

    [SerializeField] private Image[] LivesImage;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;


    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    public void Update()
    {
        if (playerLives > numOfHearts)
        {
            playerLives = numOfHearts;
        }
        for (int i = 0; i < LivesImage.Length; i++)
        {
            if (i < playerLives)
            {
                LivesImage[i].sprite = fullHeart;
            }
            else
            {
                LivesImage[i].sprite = emptyHeart;
            }
            if (i < numOfHearts)
            {
                LivesImage[i].enabled = true;
            }
            else
            {
                LivesImage[i].enabled = false;
            }
        }
        ScoreText.text = score.ToString();
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
    }
    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }
    private void TakeLife()
    {
        StartCoroutine("WaitTakeLife");
    }
    IEnumerator WaitTakeLife()
    {
        playerLives--;
        yield return new WaitForSeconds(2.5f);
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    private void ResetGameSession()
    {
        StartCoroutine("WaitResetSession");
    }
    IEnumerator WaitResetSession()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(7);
        Destroy(gameObject);
    }
}
