using System.Collections;
using Cysharp.R3;
using NUnit.Framework;
using RMC.MyProject.Scenes;
using UnityEngine;
using UnityEngine.TestTools;

namespace RMC.MyProject.Tests
{
    /// <summary>
    /// Tests for R3 ReactiveProperty integration in Scene01_Intro
    /// </summary>
    [Category("RMC.MyProject.R3")]
    public class R3ReactivePropertyTest
    {
        //  Fields ----------------------------------------
        private GameObject _parentGameObject = null;
        private Scene01_Intro _scene01Intro = null;

        //  Initialization --------------------------------

        /// <summary>
        /// Optional. Called before every [Test] method
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _parentGameObject = new GameObject();
            _scene01Intro = _parentGameObject.AddComponent<Scene01_Intro>();
        }

        /// <summary>
        /// Optional. Called after every [Test] method
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            if (_parentGameObject != null)
            {
                if (Application.isPlaying)
                {
                    // Unity prefers this while playing
                    GameObject.Destroy(_parentGameObject);
                }
                else
                {
                    // Unity prefers this while NOT playing
                    GameObject.DestroyImmediate(_parentGameObject, false);
                }
                
                _parentGameObject = null;
                _scene01Intro = null;
            }
        }

        //  Methods ---------------------------------------

        /// <summary>
        /// Test that ReactiveProperty can be created and disposed
        /// </summary>
        [Test]
        public void ReactiveProperty_CanBeCreatedAndDisposed()
        {
            // Arrange & Act
            var reactiveProperty = new ReactiveProperty<int>(42);

            // Assert
            Assert.IsNotNull(reactiveProperty);
            Assert.AreEqual(42, reactiveProperty.Value);

            // Cleanup
            reactiveProperty.Dispose();
        }

        /// <summary>
        /// Test that ReactiveProperty subscription works
        /// </summary>
        [Test]
        public void ReactiveProperty_SubscriptionTriggersOnValueChange()
        {
            // Arrange
            var reactiveProperty = new ReactiveProperty<int>(0);
            int callbackCount = 0;
            int lastValue = -1;

            // Act
            var subscription = reactiveProperty.Subscribe(value =>
            {
                callbackCount++;
                lastValue = value;
            });

            // Initial subscription should trigger immediately
            Assert.AreEqual(1, callbackCount);
            Assert.AreEqual(0, lastValue);

            // Change value
            reactiveProperty.Value = 42;

            // Assert
            Assert.AreEqual(2, callbackCount);
            Assert.AreEqual(42, lastValue);

            // Cleanup
            subscription.Dispose();
            reactiveProperty.Dispose();
        }

        /// <summary>
        /// A [UnityTest] that verifies R3 works in Unity context
        /// </summary>
        [UnityTest]
        public IEnumerator ReactiveProperty_WorksInUnityContext()
        {
            // Arrange
            var reactiveProperty = new ReactiveProperty<string>("initial");
            bool subscriptionTriggered = false;
            string receivedValue = null;

            // Act
            reactiveProperty.Subscribe(value =>
            {
                subscriptionTriggered = true;
                receivedValue = value;
            });

            // Wait a frame
            yield return null;

            // Assert initial state
            Assert.IsTrue(subscriptionTriggered);
            Assert.AreEqual("initial", receivedValue);

            // Reset and test value change
            subscriptionTriggered = false;
            reactiveProperty.Value = "changed";

            // Wait a frame
            yield return null;

            // Assert
            Assert.IsTrue(subscriptionTriggered);
            Assert.AreEqual("changed", receivedValue);

            // Cleanup
            reactiveProperty.Dispose();
        }
    }
}