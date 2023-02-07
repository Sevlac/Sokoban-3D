using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelection : MonoBehaviour
{
    [SerializeField]
    private GameObject _level;

    [SerializeField]
    private GameObject _menuManager;

    [SerializeField]
    private GameObject _levelMenu;

    [SerializeField]
    private GameObject _unlockAllToggle;

    [SerializeField]
    private List<Sprite> _levelSprites;

    private List<GameObject> _levelsGameObjects = new List<GameObject>();

    private float _widthContainer;
    private float _heightContainer;

    private void  Awake()
    {
        _widthContainer = transform.gameObject.GetComponent<RectTransform>().rect.width;
        _heightContainer = transform.gameObject.GetComponent<RectTransform>().rect.height;
        SpawnLevelSprite();

        Debug.Log("LevelMax =" +  _levelsGameObjects.Count);
        PlayerPrefs.SetInt("LevelMax", _levelsGameObjects.Count);
        Debug.Log("LevelMax =" + PlayerPrefs.GetInt("LevelMax"));
    }

    private void Update()
    {
        if (_levelMenu.activeSelf)
        {
            UpdateLevels();
        }
    }

    private void SpawnLevelSprite()
    {

        GameObject newLevel;
        int cpt = 1;
        float posX = _widthContainer*0.05f;
        float posY = _heightContainer*0.1f;
        foreach (Sprite sprite in _levelSprites)
        {
            newLevel = Instantiate(_level, transform);
            newLevel.GetComponent<Image>().sprite = sprite;
            newLevel.GetComponent<RectTransform>().sizeDelta = new Vector2(_heightContainer * 0.35f, _heightContainer * 0.35f);
            newLevel.GetComponent<RectTransform>().sizeDelta = new Vector2(_heightContainer * 0.35f, _heightContainer * 0.35f);
            newLevel.GetComponent<RectTransform>().anchoredPosition = new Vector3(posX,-posY,0);
            newLevel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + cpt;
            int levelNumber = cpt;
            newLevel.GetComponent<Button>().onClick.AddListener(() => LevelButtonClicked(levelNumber)); 

            if (PlayerPrefs.GetInt("level"+cpt+"locked",1) == 0 || cpt == 1)
            {
                newLevel.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                newLevel.GetComponent<Button>().enabled = false;
            }
            posX += newLevel.GetComponent<RectTransform>().rect.width + (_heightContainer * 0.1f);
            if (posX + newLevel.GetComponent<RectTransform>().rect.width + (_heightContainer * 0.1f) >= _widthContainer)
            {
                posX = _widthContainer * 0.05f;
                posY -= newLevel.GetComponent<RectTransform>().rect.width + _heightContainer * 0.1f;
            }
            _levelsGameObjects.Add(newLevel);
            cpt++;
        }
    }
    private void LevelButtonClicked(int number)
    {
        PlayerPrefs.SetInt("currentLevel", number);
        _menuManager.GetComponent<MenuManager>().PlayLevel();

    }

    public void UpdateLevels()
    {
        for (int i = 1; i <= _levelsGameObjects.Count; i++)
        {
            if (PlayerPrefs.GetInt("level" + i + "locked", 1) == 0 || i == 1 || _unlockAllToggle.GetComponent<Toggle>().isOn)
            {
                _levelsGameObjects[i - 1].transform.GetChild(1).gameObject.SetActive(false);
                _levelsGameObjects[i - 1].GetComponent<Button>().enabled = true;
            }
            else
            {
                _levelsGameObjects[i - 1].transform.GetChild(1).gameObject.SetActive(true);
                _levelsGameObjects[i - 1].GetComponent<Button>().enabled = false;

            }
        }
    }

}
