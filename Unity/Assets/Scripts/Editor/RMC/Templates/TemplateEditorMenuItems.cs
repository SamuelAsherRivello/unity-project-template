using UnityEditor;
using UnityEngine;

namespace RMC.Templates
{
	//  Namespace Properties ------------------------------

	//  Class Attributes ----------------------------------

	/// <summary>
	/// Replace with comments...
	/// </summary>
	public class TemplateEditorMenuItems
	{
		//  Events ----------------------------------------


		//  Properties ------------------------------------


		//  Fields ----------------------------------------
		private const string MenuItemName01 = "Window/[MyProject]/SampleMenuItem";


		//  Unity Methods ---------------------------------


		//  Methods ---------------------------------------
		[MenuItem(MenuItemName01, false, 0)]
		public static void SampleMenuItem()
		{
			Debug.Log("SampleMenuItem()");
		}

		[MenuItem(MenuItemName01, true, 0)]
		public static bool SampleMenuItem_Validate()
		{
			return true; //return false to grey-out the menu item
		}

		//  Event Handlers --------------------------------
	}
}
