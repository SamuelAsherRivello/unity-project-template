using UnityEditor;
using UnityEngine;

namespace _3rdParty.Unity
{
	public static class SharedMenuItems
	{
		//  Properties ------------------------------------

        
		//  Fields ----------------------------------------
		
		//  General Methods -------------------------------
		[MenuItem(ReadMeConstants.PathMenuItemWindowCompanyProject + "/" +
					ReadMeConstants.Open + " ReadMe", false,
					ReadMeConstants.PriorityMoralisTools_Examples)]
		public static void OpenReadMe()
		{
			ReadMeEditor.SelectReadmeGuid("3b4d333465945474ea57ff6e62ba4f37");
		}


		[MenuItem(ReadMeConstants.PathMenuItemWindowCompanyProject + "/" +
					"Load Layout (10x16)", false,
					ReadMeConstants.PriorityMoralisTools_Examples)]
		public static void LoadExampleLayout_10x16()
		{
			string guid = "3ecd0049f50df9c428b9fa47981a9a12";
			string path = AssetDatabase.GUIDToAssetPath(guid);
			ReadMeReflectionUtility.UnityEditor_WindowLayout_LoadWindowLayout(path);
		}
		
		
		[MenuItem(ReadMeConstants.PathMenuItemWindowCompanyProject + "/" +
					"Load Layout (16x10)", false,
					ReadMeConstants.PriorityMoralisTools_Examples)]
		public static void LoadExampleLayout_16x10()
		{
			string guid = "302b25cf337dea943a15941cc4453446";
			string path = AssetDatabase.GUIDToAssetPath(guid);
			ReadMeReflectionUtility.UnityEditor_WindowLayout_LoadWindowLayout(path);
		}

 
		
		///////////////////////////////////////////
		// Assets Menu
		///////////////////////////////////////////

		[MenuItem( ReadMeConstants.PathMenuItemAssetsCompanyProject + "/" + "Copy Guid", false,
					ReadMeConstants.PriorityMoralisTools_Examples)]
		public static void CopyGuidToClipboard()
		{
			// Support only if exactly 1 object is selected in project window
			var objs = Selection.objects;
			if (objs.Length != 1)
			{
				return;
			}

			var obj = objs[0];
			string path = AssetDatabase.GetAssetPath(obj);
			GUID guid = AssetDatabase.GUIDFromAssetPath(path);
			GUIUtility.systemCopyBuffer = guid.ToString();
			Debug.Log($"CopyGuidToClipboard() success! Value '{GUIUtility.systemCopyBuffer}' copied to clipboard.");
		}
		
		
		[MenuItem( ReadMeConstants.PathMenuItemAssetsCompanyProject + "/" + "Copy Guid", true,
					ReadMeConstants.PriorityMoralisTools_Examples)]
		public static bool CopyGuidToClipboard_ValidationFunction()
		{
			// Support only if exactly 1 object is selected in project window
			var objs = Selection.objects;
			return objs.Length == 1;
		}
	}
}
