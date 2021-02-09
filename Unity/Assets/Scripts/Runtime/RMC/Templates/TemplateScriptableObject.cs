using UnityEngine;

namespace RMC.Templates
{
	//  Namespace Properties ------------------------------


	//  Class Attributes ----------------------------------
	[CreateAssetMenu (
		fileName = "TemplateScriptableObject",
		menuName = "[MyProject]/TemplateScriptableObject",
		order = 0)]

	/// <summary>
	/// Replace with comments...
	/// </summary>
	public class TemplateScriptableObject : ScriptableObject
	{
		//  Properties ------------------------------------
		public string SamplePublicText
		{
			get { return _samplePublicText; }
			set { _samplePublicText = value; }
		}


		//  Fields ----------------------------------------
		[SerializeField]
		private string _samplePublicText;


		//  Unity Methods ---------------------------------
		protected void OnEnable()
		{

		}
	}
}
