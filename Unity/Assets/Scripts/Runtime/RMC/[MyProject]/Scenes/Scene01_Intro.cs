using RMC.MyProject.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace RMC.MyProject.Scenes
{
    //  Namespace Properties ------------------------------


    //  Class Attributes ----------------------------------


    /// <summary>
    /// Replace with comments...
    /// </summary>
    public class Scene01_Intro : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public HudUI HudUI { get { return _hudUI; } }


        //  Fields ----------------------------------------
        [SerializeField]
        private HudUI _hudUI;

        [SerializeField]
        private Rigidbody _playerRigidBody;

        [SerializeField]
        private float _moveSpeed = 3;

        [SerializeField] 
        private float _jumpSpeed = 5;

        
        // 2. These variables are to hold the Action references
        InputAction _moveInputAction;
        InputAction _jumpInputAction;
        InputAction _resetInputAction;
        
        //  Unity Methods ---------------------------------
        protected void Start()
        {
            Debug.Log($"{GetType().Name}.Start()");
            
            
            // Input
            _moveInputAction = InputSystem.actions.FindAction("Move");
            _jumpInputAction = InputSystem.actions.FindAction("Jump");
            _resetInputAction = InputSystem.actions.FindAction("Reset");
            
            // Set UI Text
            HudUI.SetScore("Score: 000");
            HudUI.SetLives("Lives: 003");
            HudUI.SetInstructions("Instructions: Arrows, Spacebar, R");
            HudUI.SetTitle(SceneManager.GetActiveScene().name);
        }


        protected void Update()
        {
            Vector2 moveInputVector2 = _moveInputAction.ReadValue<Vector2>();
            
            if (moveInputVector2.magnitude > 0.1f)
            {
                Vector3 moveInputVector3 = new Vector3
                (
                    moveInputVector2.x,
                    0,
                    moveInputVector2.y
                );
                
                // Move with arrow keys / WASD / gamepad
                _playerRigidBody.AddForce(moveInputVector3 * _moveSpeed, ForceMode.Force);
            }

            if (_jumpInputAction.WasPerformedThisFrame())
            {
                // Jump with spacebar / gamepad
                _playerRigidBody.AddForce(Vector3.up * _jumpSpeed, ForceMode.Impulse);
            }
            
            if (_resetInputAction.IsPressed())
            {
                // Reload the current scene with R key 
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            
        }


        //  Methods ---------------------------------------
        public string SamplePublicMethod(string message)
        {
            return message;
        }


        //  Event Handlers --------------------------------
        public void Target_OnCompleted(string message)
        {

        }
    }
}