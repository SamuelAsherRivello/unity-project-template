using UnityEngine;
using UnityEngine.UIElements;

namespace RMC.MyProject.UI
{
    /// <summary>
    /// Renders the <see cref="PlayerData"/> in the Unity Game Window.
    /// </summary>
    public class HudUI : MonoBehaviour
    {
        //  Properties ------------------------------------
        public Label UpperLeftLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("UpperLeftLabel"); }}
        public Label UpperRightLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("UpperRightLabel"); }}
        public Label LowerLeftLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("LowerLeftLabel"); }}
        public Label LowerRightLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("LowerRightLabel"); }}
        
        
        //  Fields ----------------------------------------
        [SerializeField]
        private UIDocument _uiDocument;


        //  Unity Methods ---------------------------------
        protected void Start()
        {
            Debug.Log($"{GetType().Name}.Start()");
        }

        
        //  Methods ---------------------------------------
        public void SetLives(string message)
        {
            UpperLeftLabel.text = message;
        }
        
        
        public void SetScore(string message)
        {
            UpperRightLabel.text = message;
        }
        
        
        public void SetInstructions(string message)
        {
            LowerLeftLabel.text = message;
        }
        
        
        public void SetTitle(string message)
        {
            LowerRightLabel.text = message;
        }

        
        //  Event Handlers --------------------------------
    }
}