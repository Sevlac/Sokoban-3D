using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField]
    private GameObject _winMenu;

    [SerializeField]
    private GameObject _mainMenu;

    [SerializeField]
    private GameObject _levelsMenu;

    [SerializeField]
    private GameObject _pauseMenu;

    [SerializeField]
    private GameObject _endGameMenu;

    private bool _inGame;

    private int isWinning = 0;

    private int _currentLevel;

    protected override void Awake()
    {
        base.Awake();
        PlayerPrefs.SetInt("isWinning", 0);
        Cursor.visible = true;
        _levelsMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _inGame && isWinning == 0 )
        {
            EscPressed();
            Debug.Log(Cursor.visible);
        }
        ActivateWinPage();
    }

    public void ActivateWinPage()
    {
        isWinning = PlayerPrefs.GetInt("isWinning");
        if (isWinning == 1)
        {
            PlayerPrefs.SetInt("level" + (_currentLevel + 1) + "locked", 0);
            _winMenu.SetActive(true);
            Cursor.visible = true;
        }

    }

    public void DesactivateWinPage()
    {
        PlayerPrefs.SetInt("isWinning", 0);
        _winMenu.SetActive(false);

    }

    public void NextLevelPressed()
    {
        Debug.Log("currentLevel+1 =" + (_currentLevel+1));
        Debug.Log("LevelMax =" + PlayerPrefs.GetInt("LevelMax"));
        // If the current Level is the last level activate endGame canvas else next level
        if (_currentLevel+1 > PlayerPrefs.GetInt("LevelMax"))
        {
            DesactivateWinPage();
            _inGame = false;
            _endGameMenu.SetActive(true);
            Cursor.visible = true;
        }
        else
        {
            PlayerPrefs.SetInt("currentLevel", _currentLevel + 1);
            PlayLevel();
        }
    }

    public void PlayLevel()
    {
        _currentLevel = PlayerPrefs.GetInt("currentLevel", 1);
        SceneManager.LoadScene("Level" + _currentLevel, LoadSceneMode.Single);
        DesactivateWinPage();
        _mainMenu.SetActive(false);
        _levelsMenu.SetActive(false);
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
        _inGame = true;
        Cursor.visible = false;
    }

    public void LevelsPressed()
    {
        _mainMenu.SetActive(false);
        _levelsMenu.SetActive(true);
    }

    public void MainMenuPressed()
    {
        if (isWinning == 1)
        {
            PlayerPrefs.SetInt("currentLevel", _currentLevel + 1);
        }
        _inGame = false;
        DesactivateWinPage();
        _mainMenu.SetActive(true);
        _levelsMenu.SetActive(false);
        _pauseMenu.SetActive(false);
        _endGameMenu.SetActive(false);

    }
    public void EscPressed()
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        if (_pauseMenu.activeSelf)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey("currentLevel");
        for (int i = 1; i <= PlayerPrefs.GetInt("LevelMax"); i++)
        {
            PlayerPrefs.DeleteKey("level" + i + "locked");
        }
    }
}
