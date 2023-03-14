using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace RMC.Templates
{
	//  Namespace Properties ------------------------------

	//  Class Attributes ----------------------------------

	/// <summary>
	/// Replace with comments...
	/// </summary>
	[Category("RMC.MyProject")]
	public class TemplateClassEditModeTest
	{
		//  Fields ----------------------------------------


		//  Initialization --------------------------------

		/// <summary>
		/// Optional. Called before every [Test] method
		/// </summary>
		[SetUp]
		public void Setup()
		{
			// Add something as needed...
		}

		/// <summary>
		/// Optional. Called after every [Test] method
		/// </summary>
		[TearDown]
		public void TearDown()
		{
			// Add something as needed...
		}

		//  Methods ---------------------------------------

		/// <summary>
		/// A [Test] behaves as an ordinary method
		/// </summary>
		[Test]
		public void SamplePublicText_GetValueIsExpected_WhenSet()
		{
			// Arrange
			TemplateClass templateClass = new TemplateClass();
			string text = "MyText";

			// Act
			templateClass.SamplePublicText = text;

			//  -----------------------------------------------
			// NOTE: This is a silly demo. It offers low value
			//       to unit test a set/get like this.
			//  -----------------------------------------------

			// Assert
			Assert.AreEqual(templateClass.SamplePublicText, text);
		}

		/// <summary>
		/// A [UnityTest] behaves like a coroutine in Play Mode. In Edit Mode you can use
		// `yield return null;` to skip a frame.
		/// </summary>
		/// <returns></returns>
		[UnityTest]
		public IEnumerator SamplePublicText_GetValueIsExpected_1FrameAfterWhenSet()
		{
			// Arrange
			TemplateClass templateClass = new TemplateClass();
			string text = "MyText";

			// Act
			templateClass.SamplePublicText = text;

			//  -----------------------------------------------
			// NOTE: This is a silly demo. There is no reason
			//		 to skip a frame here.
			//  -----------------------------------------------

			// Use yield to skip a frame.
			yield return null;

			// Assert
			Assert.AreEqual(templateClass.SamplePublicText, text);
		}
	}
}
