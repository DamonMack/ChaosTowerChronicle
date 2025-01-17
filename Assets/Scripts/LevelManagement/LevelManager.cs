using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    string nextLevel;
    public static bool isGameOver;
    public static bool isPaused;
    private static LevelManager instance;

    /// <summary> Called whenever the game is concluded </summary>
    public static UnityAction<GameOver> OnGameOver;

    /// <summary> Called when pausing or unpausing the game </summary>
    public static UnityAction OnTogglePause;

    [SerializeField]
    private Text gameText;
    public string winMsg = "YOU WIN!";

    private GameOver gameOverType;
    float rawTimeScale;
    const float smoothTime = 0.5f;
    float lastTimeScale;
    float timeSinceGameOver;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        isGameOver = false;
        isPaused = false;

        rawTimeScale = 1;
        lastTimeScale = 1;
        timeSinceGameOver = 0;
        Time.timeScale = 1;

        SaveManager.RetrievePlayTime();
        SaveManager.RetrieveControlParameters();
    }

    public static void Lose()
    {
        if (OnGameOver != null)
        {
            OnGameOver.Invoke(GameOver.Lose);
        }
        instance.gameOverType = GameOver.Lose;

        isGameOver = true;
        SaveManager.SavePlayTime();
        instance.gameText.gameObject.SetActive(true);
        instance.gameText.text = "GAME OVER!";
    }

    public static void Win()
    {
        if (OnGameOver != null)
        {
            OnGameOver.Invoke(GameOver.Win);
        }
        instance.gameOverType = GameOver.Win;

        isGameOver = true;
        SaveManager.SavePlayTime();
        instance.gameText.gameObject.SetActive(true);
        instance.gameText.text = instance.winMsg;
    }

    public void NextLevel()
    {
        if (nextLevel == null || nextLevel.Length == 0)
            return;
        SceneManager.LoadScene(nextLevel);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if (isGameOver || isPaused)
        {
            rawTimeScale -= Time.unscaledDeltaTime / smoothTime;
        }
        else
        {
            rawTimeScale += Time.unscaledDeltaTime / smoothTime;
            SaveManager.Playtime += Time.deltaTime;
        }
        rawTimeScale = Mathf.Clamp01(rawTimeScale);
        float timeScale = Mathf.SmoothStep(0, 1, rawTimeScale);
        if (timeScale != lastTimeScale)
        {
            Time.timeScale = timeScale;
            lastTimeScale = timeScale;
        }

        if (isGameOver)
        {
            timeSinceGameOver += Time.unscaledDeltaTime;
            if (timeSinceGameOver >= 2.0f)
            {
                if (gameOverType == GameOver.Win)
                {
                    NextLevel();
                }
                else
                {
                    RetryLevel();
                }
            }
        }
    }

    private void TogglePause()
    {
        /*
        isPaused = !isPaused;
        if (OnTogglePause != null) {
            OnTogglePause.Invoke();
        }
        */
    }

    void OnEnable()
    {
        InputManager.pause.AddListener(this.TogglePause);
    }

    void OnDisable()
    {
        InputManager.pause.RemoveListener(this.TogglePause);
    }
}
