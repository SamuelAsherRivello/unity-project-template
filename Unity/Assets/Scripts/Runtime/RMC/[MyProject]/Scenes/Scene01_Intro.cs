using RMC.Core.Audio;
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
        
        /// <summary>
        /// Runs once per Scene. Use for initialization
        /// </summary>
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

        
        /// <summary>
        /// Runs every frame. Use for input/physics/gameplay
        /// </summary>
        protected void Update()
        {
            HandleUserInput();
            CheckPlayerFalling();
        }

        
        //  Methods ---------------------------------------
        
        /// <summary>
        /// Take user input from keyboard/mouse/gamepad
        /// </summary>
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
                
                if (_moveInputAction.WasPerformedThisFrame())
                {
                    PlayAudioClip("Click01");
                }
            }

            if (_jumpInputAction.WasPerformedThisFrame())
            {
                // Jump with spacebar / gamepad
                _playerRigidBody.AddForce(Vector3.up * _playerJumpSpeed, ForceMode.Impulse);
                PlayAudioClip("ItemUpdate01");
            }
            
            if (_resetInputAction.IsPressed())
            {
                // Reload the current scene with R key 
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

        }

        /// <summary>
        /// Check for out of bounds
        /// </summary>
        private void CheckPlayerFalling()
        {
            if (_playerRigidBody.transform.position.y < -5)
            {
                // Reload the current scene if character falls off Floor
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        

        /// <summary>
        /// Play system using the AudioManager imported via https://github.com/SamuelAsherRivello/rmc-core/
        /// </summary>
        /// <param name="audioClipName">Must match AudioClip name within Assets/Settings/Audio/AudioManagerConfiguration.asset</param>
        private void PlayAudioClip(string audioClipName)
        {
            AudioManager.Instance.PlayAudioClip(audioClipName);
        }

        //  Event Handlers --------------------------------
    }
}