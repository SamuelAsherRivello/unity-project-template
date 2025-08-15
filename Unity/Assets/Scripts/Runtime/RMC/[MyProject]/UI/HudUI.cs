using UnityEngine;
using UnityEngine.UIElements;

namespace RMC.MyProject.UI
{
    /// <summary>
    /// Renders user interface elements onto the Unity screen.
    /// </summary>
    public class HudUI : MonoBehaviour
    {
        //  Properties ------------------------------------
        public Label LivesLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("UpperLeftLabel"); }}
        public Label ScoreLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("UpperRightLabel"); }}
        public Label InstructionsLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("LowerLeftLabel"); }}
        public Label TitleLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("LowerRightLabel"); }}
        
        //  Fields ----------------------------------------
        [SerializeField]
        private UIDocument _uiDocument;

        //  Unity Methods ---------------------------------
        protected void Start()
        {
            Debug.Log($"{GetType().Name}.Start()");
        }

        //  Methods ---------------------------------------

        //  Event Handlers --------------------------------
    }
}