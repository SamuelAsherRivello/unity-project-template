using RMC.MyProject.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace RMC.MyProject.Scenes
{
    /// <summary>
    /// Main entry point for the Scene.
    /// </summary>
    public class Scene01_Intro : MonoBehaviour
    {
        //  Properties ------------------------------------
        public HudUI HudUI { get { return _hudUI; } }


        //  Fields ----------------------------------------
        [Header("UI")]
        [SerializeField]
        private HudUI _hudUI;

        [Header("Player")]
        [SerializeField]
        private Rigidbody _playerRigidBody;
        
        [SerializeField]
        private float _playerMoveSpeed = 4;
        
        [SerializeField]
        private float _playerJumpSpeed = 5;

        // Input
        private InputAction _moveInputAction;
        private InputAction _jumpInputAction;
        private InputAction _resetInputAction;
        
        //  Unity Methods ---------------------------------
        protected void Start()
        {
            Debug.Log($"{GetType().Name}.Start()");
            
            // Input
            _moveInputAction = InputSystem.actions.FindAction("Move");
            _jumpInputAction = InputSystem.actions.FindAction("Jump");
            _resetInputAction = InputSystem.actions.FindAction("Reset");
            
            // UI
            HudUI.SetScore("Score: 000");
            HudUI.SetLives("Lives: 003");
            HudUI.SetInstructions("Instructions: WASD/Arrows, Spacebar, R");
            HudUI.SetTitle(SceneManager.GetActiveScene().name);
            
        }


        protected void Update()
        {
            HandleUserInput();
            CheckPlayerFalling();
        }



        //  Methods ---------------------------------------
        private void HandleUserInput()
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
                _playerRigidBody.AddForce(moveInputVector3 * _playerMoveSpeed, ForceMode.Acceleration);
            }

            if (_jumpInputAction.WasPerformedThisFrame())
            {
                // Jump with spacebar / gamepad
                _playerRigidBody.AddForce(Vector3.up * _playerJumpSpeed, ForceMode.Impulse);
            }
            
            if (_resetInputAction.IsPressed())
            {
                // Reload the current scene with R key 
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

        }

        
        private void CheckPlayerFalling()
        {
            if (_playerRigidBody.transform.position.y < -5)
            {
                // Reload the current scene if character falls off Floor
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        //  Event Handlers --------------------------------
    }
}