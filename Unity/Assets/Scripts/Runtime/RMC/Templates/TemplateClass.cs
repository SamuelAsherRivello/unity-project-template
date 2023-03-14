using UnityEngine;

namespace RMC.Templates
{
	//  Namespace Properties ------------------------------

	//  Class Attributes ----------------------------------

	/// <summary>
	/// Replace with comments...
	/// </summary>
	public class TemplateClass
	{
		//  Events ----------------------------------------


		//  Properties ------------------------------------
		public string SamplePublicText { get { return _samplePublicText; } set { _samplePublicText = value; }}


		//  Fields ----------------------------------------
		private string _samplePublicText;


		//  Initialization --------------------------------
		public TemplateClass()
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
