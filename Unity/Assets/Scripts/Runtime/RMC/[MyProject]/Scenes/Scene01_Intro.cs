using System.Threading.Tasks;
using Cysharp.R3;
using RMC.Audio;
using RMC.MyProject.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace RMC.MyProject.Scenes
{
    /// <summary>
    /// Main entry point for the Scene.
    /// </summary>
    public class Scene01_Intro : MonoBehaviour
    {

        //  Constants -------------------------------------
        private const int ScoreMax = 5;
        private const int LivesMax = 3;
        private const float MoveInputMinimumMagnitude = 0.1f;
        private const float PhysicsRaycastMaximumDistance = 0.6f;

        //  Properties ------------------------------------
        public HudUI HudUI { get { return _hudUI; } }

        public int Score 
        {
            get
            {
                return _reactiveScore.Value;
            }
            set
            {
                _reactiveScore.Value = value;
                HudUI.ScoreLabel.text = $"Score: {_reactiveScore.Value:000}/{ScoreMax:000}";
            }
        }
        
        public int Lives 
        {
            get
            {
                return _lives;
            }
            set
            {
                _lives = value;
                HudUI.LivesLabel.text = $"Lives: {_lives:000}/{LivesMax:000}";
            }
        }

        public bool IsEnabledInput
        {
            get
            {
                return _isEnabledInput;
            }
            set
            {
                _isEnabledInput = value;

                // Freeze player in position when no input
                _playerRigidBody.isKinematic = !_isEnabledInput;
            }
        }

        public bool IsPlayerGrounded
        {
            get
            {
                return _isPlayerGrounded;
            }
            private set
            {
                _isPlayerGrounded = value;
            }
        }

        //  Fields ----------------------------------------
        [Header("UI")]
        [SerializeField]
        private HudUI _hudUI;

        [Header("Player")]
        [SerializeField]
        private Rigidbody _playerRigidBody;

        [Header("Configuration")]
        [SerializeField]
        private float _playerMoveSpeedGround = 1000;

        [SerializeField]
        private float _playerMoveSpeedAir = 250;
        
        [SerializeField]
        private float _playerJumpSpeed = 5;

        // Input
        private InputAction _movePlayerInputAction;
        private InputAction _jumpPlayerInputAction;
        private InputAction _resetGameInputAction;
        private InputAction _toggleThemeInputAction;
        
        // Data
        private int _lives = 0;
        private bool _isEnabledInput = true;
        private bool _isPlayerGrounded = false;

        // R3 ReactiveProperty demonstration
        private readonly ReactiveProperty<int> _reactiveScore = new ReactiveProperty<int>(0);

        // Audio
        private const string PlayerResetAudioClip = "ItemRead01";
        private const string GameWinAudioClip = "Music_Win01";
        private const string PlayerJumpAudioClip = "ItemUpdate01";
        private const string PlayerMoveAudioClip = "Click01";
        
        //  Unity Methods ---------------------------------
        
        /// <summary>
        /// Runs once per Scene. Use for initialization
        /// </summary>
        protected void Start()
        {
            Debug.Log($"{GetType().Name}.Start()");
            
            // R3 ReactiveProperty demonstration - Subscribe to score changes
            _reactiveScore.Subscribe(newScore => 
            {
                Debug.Log($"[R3 Demo] Score changed to: {newScore}");
            }).AddTo(this);
            
            // Input
            _movePlayerInputAction = InputSystem.actions.FindAction("MovePlayer");
            _jumpPlayerInputAction = InputSystem.actions.FindAction("JumpPlayer");
            _resetGameInputAction = InputSystem.actions.FindAction("ResetGame");
            _toggleThemeInputAction = InputSystem.actions.FindAction("ToggleTheme");
            IsEnabledInput = true;

            // UI
            Score = 0;
            Lives = LivesMax;
            SetInstructions();
            SetTitle();
        }

        /// <summary>
        /// Set the instructions based on the current input configuration.
        /// </summary>
        private void SetInstructions()
        {
            HudUI.InstructionsLabel.text = $"Instructions: WASD/Arrows, Spacebar, R, T";
        }

        /// <summary>
        /// Set the title based
        /// </summary>
        private void SetTitle()
        {
            string themeName = "Light";
            if (MyProjectSingleton.Instance.ThemeManager.IsDark)
            {
                themeName = "Dark";
            }

            HudUI.TitleLabel.text = $"{SceneManager.GetActiveScene().name} ({themeName})";
        }

        /// <summary>
        /// Runs every frame. Use for input/physics/gameplay
        /// </summary>
        protected void Update()
        {
            if (!IsEnabledInput)
            {
                return;
            }
            
            UpdateIsPlayerGrounded();
            HandleUserInput();
            CheckPlayerFalling();
        }

        //  Methods ---------------------------------------
        
        /// <summary>
        /// Take user input from keyboard/mouse/gamepad
        /// </summary>
        private async void HandleUserInput()
        {
            Vector2 moveInputVector2 = _movePlayerInputAction.ReadValue<Vector2>();

            if (moveInputVector2.magnitude > MoveInputMinimumMagnitude)
            {
                Vector3 moveInputVector3 = new Vector3
                (
                    moveInputVector2.x,
                    0,
                    moveInputVector2.y
                );

                // Move with arrow keys / WASD / gamepad
                if (IsPlayerGrounded)
                {
                    _playerRigidBody.AddForce(moveInputVector3 * (_playerMoveSpeedGround * Time.deltaTime), ForceMode.Force);
                }
                else
                {
                    _playerRigidBody.AddForce(moveInputVector3 * (_playerMoveSpeedAir * Time.deltaTime), ForceMode.Force);
                }

                if (_movePlayerInputAction.WasPerformedThisFrame())
                {
                    PlayAudioClip(PlayerMoveAudioClip);
                }
            }

            // Only allow jump when grounded
            if (_jumpPlayerInputAction.WasPerformedThisFrame() && IsPlayerGrounded)
            {
                // Jump with spacebar / gamepad
                _playerRigidBody.AddForce(Vector3.up * _playerJumpSpeed, ForceMode.Impulse);
                IsPlayerGrounded = false; // Prevent immediate re-jump until next ground check

                // Reward points per jump
                Score += 1;

                //Check game over
                if (Score >= ScoreMax)
                {
                    // Play sound
                    PlayAudioClip(GameWinAudioClip);

                    // Disable player
                    IsEnabledInput = false;

                    // Wait before disable (Cosmetic polish)
                    await Task.Delay(3000/2);    
                    
                    // Wait for rest of sound
                    await Task.Delay(3000/2);    
                    
                    // Reload
                    ReloadGame();
                    return;
                }
                
                // Play sound
                PlayAudioClip(PlayerJumpAudioClip);
          
            }
            
            if (_resetGameInputAction.WasPerformedThisFrame())
            {
                // Reload the current scene with R key 
                ReloadGame();
            }

            if (_toggleThemeInputAction.WasPerformedThisFrame())
            {
                // Toggle the current theme with T key
                ToggleTheme();
            }
        }

        /// <summary>
        /// Check for out of bounds
        /// </summary>
        private void CheckPlayerFalling()
        {
            if (_playerRigidBody.transform.position.y < -5)
            {
                // Decrement life
                Lives -= 1;
                
                // Play sound
                PlayAudioClip(PlayerResetAudioClip);
                
                //Check game over
                if (Lives <= 0)
                {
                    ReloadGame();
                    return;
                }
                
                // Reset player position
                _playerRigidBody.transform.position = new Vector3(0, 0, 0);
                _playerRigidBody.angularVelocity = Vector3.zero;
                _playerRigidBody.linearVelocity = Vector3.zero;
            }
        }

        /// <summary>
        /// Restart the same Scene as a #hack to restart the game
        /// </summary>
        private void ReloadGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Reload the current Scene to toggle themes.
        /// </summary>
        private void ToggleTheme()
        {
            MyProjectSingleton.Instance.ThemeManager.IsDark =
                !MyProjectSingleton.Instance.ThemeManager.IsDark;

            SetTitle();
        }

        /// <summary>
        /// Check if the player is grounded using a raycast.
        /// </summary>
        private void UpdateIsPlayerGrounded()
        {
            // Raycast slightly below the rigidbody position to detect ground.
            // Assumes pivot near center; adjust distance in Inspector.
            var playerCenterpoint = _playerRigidBody.transform.position;
            Ray playerCenterpointDownward = new Ray(playerCenterpoint, Vector3.down);
            IsPlayerGrounded= Physics.Raycast(playerCenterpointDownward, out _, PhysicsRaycastMaximumDistance);
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