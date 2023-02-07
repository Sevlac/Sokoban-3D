using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Board : MonoBehaviour
{
    // This dictionnary contain the positions of cells as a key and their name as a value ("Ground" or "Wall"). 
    private Dictionary<Vector3,string> _environnementDictionary = new Dictionary<Vector3,string>();

    // This dictionnary contain the positions of Targets as a key and their Transform as a value.
    private Dictionary<Vector3, Transform> _targetDictionary = new Dictionary<Vector3, Transform>();

    // This dictionnary contain the positions of Crates and of the Player  as a key and their Transform as a Value.
    private Dictionary<Vector3, Transform> _boxDictionary = new Dictionary<Vector3, Transform>();

    // Contain the particle system used for the confettits when the player win.
    [SerializeField]
    private ParticleSystem _particle;

    // This List contain the Transform of each Crates and of the Player.
    [SerializeField]
    private List<Transform> _boxGoals;

    // Contain the transform of the gameobject that contain all the cells "ground", "Wall" and "Target" in the hierarchy.
    [SerializeField]
    private Transform _environnement;

    // Contain the transform of the gameobject in the hierarchy that will get in child the background generated in CreateBackground().
    [SerializeField]
    private Transform _background;


    // Contain the transform of the Player.
    [SerializeField]
    private Transform _player;

    // Will Contain the transform of a crates.
    private Transform _crates;

    // Contain the position of the Player
    private Vector3 _playerPosition;

    private bool _isWinning = false;

    // Contain the prefab Wall
    [SerializeField]
    private GameObject _prefabWall;

    // xMin, xMax, zMin, zMax position of the board.
    private float _xMin, _xMax, _zMin, _zMax;

    // size of the background on each side of the board.
    private float _largeur = 5f;

    private void Awake()
    {
        
        _crates = null;
        _particle.Stop();
        CreateDictionnarys();

        // the player's pivot is in the center of the cube and the cell's pivot under the player is on the forward left angle.
        // so if the player position is (0.5 ,0.5 ,0) then the cell position under the player is (0 ,0 ,0,5).
        _playerPosition = _player.position + new Vector3(-0.5f,-0.5f,0.5f);
        CreateBackground();
    }

    // This function fill the dictionnarys with the walls, grounds, crates, targets and  the player.
    private void CreateDictionnarys()
    {
        for (int i = 0; i < _environnement.childCount; i++)
        {

            Transform cell = _environnement.GetChild(i);
            Vector3 cellPosition = cell.position;
            string cellName = cell.name;

            SetXZMinAndMax(i, cellPosition);

            if (cellName == "Wall")
            {
                if (_environnementDictionary.ContainsKey(cellPosition))
                {
                    _environnementDictionary[cellPosition] = cellName;
                }
                else
                {
                    _environnementDictionary.Add(cellPosition, cellName);
                }
            }
            else if (cellName == "Target")
            {
                _targetDictionary.Add(cellPosition, cell);
                _environnementDictionary.Add(cellPosition, "Ground");
            }
            else if (cellName == "Ground")
            {
                if (!_environnementDictionary.ContainsKey(cellPosition))
                {
                    _environnementDictionary.Add(cellPosition, cellName);
                }
            }
        }

        foreach (var box in _boxGoals)
        {
            _boxDictionary.Add(box.position + new Vector3(-0.5f, -0.5f, 0.5f), box);
        }
    }

    public bool VerifyIfPlayerCanMove(Vector3 direction)
    {
        if (!_isWinning)
        {
            Vector3 positionToMovePlayerOn = _playerPosition + direction;

            // verify if _environnementDictionnary contain a key that is the position where the player want to go on
            if (_environnementDictionary.ContainsKey(positionToMovePlayerOn))
            {
                // verify if _environnementDictionnary contain a value "Ground" at the position where the player want to go on
                if (_environnementDictionary[positionToMovePlayerOn] == "Ground")
                {
                    // verify if _boxDictionnary contain a key that is the position where the player want to go on
                    if (_boxDictionary.ContainsKey(positionToMovePlayerOn))
                    {
                        // verify if _environnementDictionnary contain a key that is the position where the box in front of the player want to go on
                        if (_environnementDictionary.ContainsKey(positionToMovePlayerOn + direction))
                        {
                            // verify if _environnementDictionnary contain a value "Ground" at the position where the box in front of the player want to go on
                            if (_environnementDictionary[positionToMovePlayerOn + direction] == "Ground")
                            {
                                // verify if _boxDictionnary  doesn't contain a key that is the position where the box in front of the player want to go on
                                if (!(_boxDictionary.ContainsKey(positionToMovePlayerOn + direction)))
                                {
                                    _crates = _boxDictionary[positionToMovePlayerOn];
                                    UpdateKeyFromBoxDictionnary(positionToMovePlayerOn, positionToMovePlayerOn + direction, _crates);
                                    UpdateKeyFromBoxDictionnary(_playerPosition, positionToMovePlayerOn, _boxDictionary[_playerPosition]);
                                    _playerPosition += direction;
                                    StartCoroutine(LevelWin());
                                    _crates.GetComponentInChildren<Move>().MoveTo(direction);
                                    return true;
                                }
                            }

                        }
                    }
                    else
                    {
                        UpdateKeyFromBoxDictionnary(_playerPosition, positionToMovePlayerOn, _boxDictionary[_playerPosition]);
                        _playerPosition += direction;
                        StartCoroutine(LevelWin());
                        return true;
                    }
                }

            }
            
        }
        return false;
    }

    private IEnumerator LevelWin()
    {
        foreach (var target in _targetDictionary)
        {
            foreach(var box in _boxDictionary)
            {
                // If the target and the box have the same color
                if (target.Value.gameObject.GetComponent<MeshRenderer>().material.color == box.Value.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color)
                {
                    // If the target and the box have the same position
                    if (target.Key == box.Key)
                    {
                        _isWinning = true;
                        break;
                    }
                    else
                    {
                        _isWinning = false;
                    }
                }
            }
            if (_isWinning == false)
            {
                break;
            }
        }
        if (_isWinning)
        {
            // wait 0.4 sec to finish the movement of the player before activate the winMenu in MenuManager
            yield return new WaitForSeconds(0.4f);
            _particle.Play();
            PlayerPrefs.SetInt("isWinning", 1);
        }
    }

    private void SetXZMinAndMax(int cellIndex, Vector3 cellPosition)
    {
        if (cellIndex == 0)
        {
            _xMin = cellPosition.x;
            _xMax = cellPosition.x;
            _zMin = cellPosition.z;
            _zMax = cellPosition.z;
        }
        if (cellPosition.x < _xMin)
        {
            _xMin = cellPosition.x;
        }
        if (cellPosition.x > _xMax)
        {
            _xMax = cellPosition.x;
        }
        if (cellPosition.z < _zMin)
        {
            _zMin = cellPosition.z;
        }
        if (cellPosition.z > _zMax)
        {
            _zMax = cellPosition.z;
        }
    }
    private void CreateBackground()
    {
        for (float couche = 0; couche <= _largeur; couche+= _largeur)
        {
            for (float z = _zMin - _largeur; z <= _zMax + _largeur; z++)
            {
                for (float x = _xMin - _largeur; x <= _xMax + _largeur; x++)
                {
                    if (!(z >= _zMin && z <= _zMax && x >= _xMin && x <= _xMax && couche == 0))
                    {
                        GameObject newWall;
                        newWall = Instantiate(_prefabWall, _background);
                        newWall.transform.position = new Vector3(x, 0 - couche, z);
                        if (z < _zMin)
                        {
                            newWall.transform.localScale = new Vector3(1, 0.5f, 1);
                        }
                        else if (x == _xMin - _largeur || x == _xMax + _largeur || z == _zMax + _largeur)
                        {
                            newWall.transform.localScale = new Vector3(1, 5, 1);
                        }
                        else
                        {
                            newWall.transform.localScale = new Vector3(1, Random.Range(0f, 4.5f), 1);
                        }
                    }
                }
            }
        }
        
    }

    private void UpdateKeyFromBoxDictionnary(Vector3 oldKey, Vector3 newKey, Transform value)
    {
        _boxDictionary.Remove(oldKey);
        _boxDictionary.Add(newKey, value);

    }
}
