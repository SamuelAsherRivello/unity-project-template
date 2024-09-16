using UnityEngine;
using UnityEngine.UIElements;

namespace RMC.MyProject.UI
{
    //  Namespace Properties ------------------------------


    //  Class Attributes ----------------------------------


    /// <summary>
    /// Replace with comments...
    /// </summary>
    public class HudUI : MonoBehaviour
    {
        //  Events ----------------------------------------


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
        public string SetLives(string message)
        {
            return UpperLeftLabel.text = message;
        }
        
        public string SetScore(string message)
        {
            return UpperRightLabel.text = message;
        }
        
        public string SetInstructions(string message)
        {
            return LowerLeftLabel.text = message;
        }
        
        public string SetTitle(string message)
        {
            return LowerRightLabel.text = message;
        }


        //  Event Handlers --------------------------------
    }
}