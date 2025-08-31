using System.Threading.Tasks;
using R3;
using RMC.Audio;
using RMC.MyProject.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace RMC.MyProject.Scenes
{
    /// <summary>
    /// Main entry point for the Scene.
    /// </summary>
    public class GameController : MonoBehaviour
    {
        //  Properties ------------------------------------
        public GameView GameView { get { return _gameView; } }

        public bool IsInputEnabled
        {
            get
            {
                return _isInputEnabled;
            }
            set
            {
                _isInputEnabled = value;

                // Freeze player in position when no input
                _playerRigidBody.isKinematic = !_isInputEnabled;
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
        private GameView _gameView;

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
        private bool _isInputEnabled = true;
        private bool _isPlayerGrounded = false;
        private CompositeDisposable _disposable = new CompositeDisposable();

        // Audio
        private const string PlayerResetAudioClip = "ItemRead01";
        private const string GameWinAudioClip = "Music_Win01";
        private const string PlayerJumpAudioClip = "ItemUpdate01";
        private const string PlayerMoveAudioClip = "Click01";

        // Model
        private GameModel _gameModel;
        
        //  Unity Methods ---------------------------------
        
        /// <summary>
        /// Runs once per Scene during the beginning. Use for initialization
        /// </summary>
        protected void Start()
        {
            Debug.Log($"{GetType().Name}.Start()");

            // Input
            _movePlayerInputAction = InputSystem.actions.FindAction("MovePlayer");
            _jumpPlayerInputAction = InputSystem.actions.FindAction("JumpPlayer");
            _resetGameInputAction = InputSystem.actions.FindAction("ResetGame");
            _toggleThemeInputAction = InputSystem.actions.FindAction("ToggleTheme");
            IsInputEnabled = true;

            // Model
            _gameModel = new GameModel();
            _gameModel.Score.Subscribe(GameModel_OnScoreChanged).AddTo(_disposable);
            _gameModel.Lives.Subscribe(GameModel_OnLivesChanged).AddTo(_disposable);
            _gameModel.Score.Value = GameModel.ScoreMin;
            _gameModel.Lives.Value = GameModel.LivesMax;

            // View
            _gameView.Initialize(_gameModel);

            // Singleton
            MyProjectSingleton.Instance.ThemeManager.IsDark.Subscribe(MyProjectSingleton_OnIsDarkChanged).AddTo(_disposable);
        }

        /// <summary>
        /// Runs once per Scene during the end. Use for deinitialization
        /// </summary>
        protected void OnDestroy()
        {
            _disposable?.Dispose();
        }

        /// <summary>
        /// Runs every frame. Use for input/physics/gameplay
        /// </summary>
        protected void Update()
        {
            if (!IsInputEnabled)
            {
                return;
            }
            
            CheckIsPlayerGrounded();
            CheckUserInput();
            CheckPlayerFalling();
        }

        //  Methods ---------------------------------------
        
        /// <summary>
        /// Take user input from keyboard/mouse/gamepad
        /// </summary>
        private void CheckUserInput()
        {
            Vector2 moveInputVector2 = _movePlayerInputAction.ReadValue<Vector2>();

            if (moveInputVector2.magnitude > GameModel.MoveInputMinimumMagnitude)
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
                _gameModel.Score.Value += 1;

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
                _gameModel.Lives.Value -= 1;
                
                // Play sound
                PlayAudioClip(PlayerResetAudioClip);
                
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
            MyProjectSingleton.Instance.ThemeManager.IsDark.Value =
                !MyProjectSingleton.Instance.ThemeManager.IsDark.Value;
        }

        /// <summary>
        /// Check if the player is grounded using a raycast.
        /// </summary>
        private void CheckIsPlayerGrounded()
        {
            // Raycast slightly below the rigidbody position to detect ground.
            // Assumes pivot near center; adjust distance in Inspector.
            var playerCenterpoint = _playerRigidBody.transform.position;
            Ray playerCenterpointDownward = new Ray(playerCenterpoint, Vector3.down);
            IsPlayerGrounded= Physics.Raycast(playerCenterpointDownward, out _, GameModel.PhysicsRaycastMaximumDistance);
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

        private void MyProjectSingleton_OnIsDarkChanged(bool value)
        {
            string themeName = "Light";
            if (value)
            {
                themeName = "Dark";
            }
            _gameModel.Title.Value = $"{SceneManager.GetActiveScene().name} ({themeName})";
            _gameModel.Instructions.Value = "Instructions: WASD/Arrows, Spacebar, R, T";
        }

        private async void GameModel_OnScoreChanged(int value)
        {
            //Check game over
            if (_gameModel.Score.Value >= GameModel.ScoreMax)
            {
                // Play sound
                PlayAudioClip(GameWinAudioClip);

                // Disable player
                IsInputEnabled = false;

                // Wait before disable (Cosmetic polish)
                await Task.Delay(3000/2);

                // Wait for rest of sound
                await Task.Delay(3000/2);

                // Reload
                ReloadGame();
            }
        }

        private void GameModel_OnLivesChanged(int value)
        {
            //Check game over
            if (_gameModel.Lives.Value <= 0)
            {
                ReloadGame();
            }
        }
    }
}