using UnityEngine;

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
        public string SamplePublicText { get { return _samplePublicText; } set { _samplePublicText = value; }}


        //  Fields ----------------------------------------
        [SerializeField]
        private string _samplePublicText;


        //  Unity Methods ---------------------------------
        protected void Start()
        {
            Debug.Log($"{GetType().Name}.Start()");
        }


        protected void Update()
        {

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