using UnityEngine;

public class ControlCamera : MonoBehaviour
{
    [SerializeField] private Transform _player;
    private void Update()
    {
        transform.position = new Vector3(_player.position.x, transform.position.y, _player.position.z - 3.5f);
    }
}
