using UnityEngine;

namespace RMC.Templates
{
	//  Namespace Properties ------------------------------


	//  Class Attributes ----------------------------------
	
	/// <summary>
	/// Replace with comments...
	/// </summary>
	[CreateAssetMenu (
		fileName = "TemplateScriptableObject",
		menuName = "[MyProject]/TemplateScriptableObject",
		order = 0)]

	public class TemplateScriptableObject : ScriptableObject
	{
		//  Properties ------------------------------------
		public string SamplePublicText { get { return _samplePublicText; } set { _samplePublicText = value; }}


		//  Fields ----------------------------------------
		[SerializeField]
		private string _samplePublicText;


		//  Unity Methods ---------------------------------
		protected void OnEnable()
		{

		}
	}
}
