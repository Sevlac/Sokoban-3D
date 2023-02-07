using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    // return the script Move.cs attached to Roll.cs in the transform
    private Move _moveComponent;

    // retrun the script Board.cs attached to the transform Grid
    [SerializeField]
    private Board _boardScript;

    // bool that return true if the player move else false
    public static bool _isMoving; 

    void Awake()
    {
        _moveComponent = GetComponentInChildren<Move>();
    }
    
    void Update()
    {
        Movement();

    }

    private void Movement()
    {
        if (!_isMoving && Time.timeScale > 0)
        {
            // return true if a | d | left | right is pressed (works with azerty too).
            if (Input.GetButtonDown("Horizontal")) 
            {
                //Input.GetAxisRaw("Horizontal") return -1 for left et 1 for right.
                Vector3 direction = Input.GetAxisRaw("Horizontal") * new Vector3(1, 0, 0);

                // Call the function that verify if the player can move in the script Board.cs
                if (_boardScript.VerifyIfPlayerCanMove(direction)) 
                {
                    _isMoving = true;

                    // call the function that move the player in script Move.cs
                    _moveComponent.MoveTo(direction); 
                }
            }

            // return true if w | s | up | down is pressed (works with azerty too).
            else if (Input.GetButtonDown("Vertical")) 
            {
                // Input.GetAxisRaw("Vertical") return -1 for down et 1 for up.
                Vector3 direction = Input.GetAxisRaw("Vertical") * new Vector3(0, 0, 1); 
                if (_boardScript.VerifyIfPlayerCanMove(direction))
                {
                    _isMoving = true;
                    _moveComponent.MoveTo(direction); 
                }
            }
        }
    }
}
