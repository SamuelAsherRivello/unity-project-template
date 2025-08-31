using R3;
using RMC.MyProject.Scenes;
using UnityEngine;
using UnityEngine.UIElements;

namespace RMC.MyProject.UI
{
    /// <summary>
    /// Renders user interface elements onto the Unity screen.
    /// </summary>
    public class GameView : MonoBehaviour
    {
        //  Properties ------------------------------------
        public Label LivesLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("UpperLeftLabel"); }}
        public Label ScoreLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("UpperRightLabel"); }}
        public Label InstructionsLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("LowerLeftLabel"); }}
        public Label TitleLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("LowerRightLabel"); }}

        //  Fields ----------------------------------------
        [SerializeField]
        private UIDocument _uiDocument;
        private GameModel _gameModel;
        private CompositeDisposable _disposable = new CompositeDisposable();
        public bool IsInitialized { get; private set; }

        //  Unity Methods ---------------------------------

        protected void OnDestroy()
        {
            Debug.Log($"{GetType().Name}.Dispose()");
            _disposable?.Dispose();
            IsInitialized = false;
        }

        //  Methods ---------------------------------------
        public void Initialize(GameModel gameModel)
        {
            Debug.Log($"{GetType().Name}.Initialize()");

            if (IsInitialized)
            {
                return;
            }

            IsInitialized = true;

            // Model
            _gameModel = gameModel;
            _gameModel.Lives.Subscribe(GameModel_OnLivesChanged).AddTo(_disposable);
            _gameModel.Score.Subscribe(GameModel_OnScoreChanged).AddTo(_disposable);
            _gameModel.Title.Subscribe(GameModel_OnTitleChanged).AddTo(_disposable);
            _gameModel.Instructions.Subscribe(GameModel_OnInstructionsChanged).AddTo(_disposable);
        }

        //  Event Handlers --------------------------------

        private void GameModel_OnInstructionsChanged(string value)
        {
            InstructionsLabel.text = $"{value}";
        }

        private void GameModel_OnTitleChanged(string value)
        {
            TitleLabel.text = $"{value}";
        }

        private void GameModel_OnScoreChanged(int value)
        {
            ScoreLabel.text = $"Score: {_gameModel.Score.Value:000}/{GameModel.ScoreMax:000}";
        }

        private void GameModel_OnLivesChanged(int value)
        {
            LivesLabel.text = $"Lives: {_gameModel.Lives.Value:000}/{GameModel.LivesMax:000}";
        }
    }
}