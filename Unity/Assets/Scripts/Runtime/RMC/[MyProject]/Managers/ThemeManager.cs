using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace RMC.MyProject.Scenes
{
    public class ThemeManager
    {
        // Members --------------------------------------
        private readonly StyleSheet _lightStyleSheet;
        private readonly StyleSheet _darkStyleSheet;

        // Reactive Extensions --------------------------

        /// <summary>
        /// Property to get or set the current theme. Defaults to Dark.
        /// Setting this property toggles between Light and Dark themes.
        /// </summary>
        public readonly ReactiveProperty<bool> IsDark = new ReactiveProperty<bool>(true);

        /// <summary>
        /// Constructor for ThemeManager. Initializes the theme to Dark.
        /// </summary>
        /// <param name="lightStyleSheet"></param>
        /// <param name="darkStyleSheet"></param>
        public ThemeManager(StyleSheet lightStyleSheet, StyleSheet darkStyleSheet)
        {
            _lightStyleSheet = lightStyleSheet;
            _darkStyleSheet = darkStyleSheet;
            IsDark.Subscribe(_ => OnIsDarkChanged());
        }

        /// <summary>
        /// Applies the Light theme.
        /// </summary>
        private void ApplyLightTheme()
        {
            List<UIDocument> uiDocuments = Object.FindObjectsByType<UIDocument>(FindObjectsSortMode.None).ToList();

            foreach (var uiDocument in uiDocuments)
            {
                if (_darkStyleSheet != null)
                {
                    uiDocument.rootVisualElement.styleSheets.Remove(_darkStyleSheet);
                }

                if (_lightStyleSheet != null && !uiDocument.rootVisualElement.styleSheets.Contains(_lightStyleSheet))
                {
                    uiDocument.rootVisualElement.styleSheets.Add(_lightStyleSheet);
                }
            }
        }

        /// <summary>
        /// Applies the Dark theme.
        /// </summary>
        private void ApplyDarkTheme()
        {
            List<UIDocument> uiDocuments = Object.FindObjectsByType<UIDocument>(FindObjectsSortMode.None).ToList();

            foreach (var uiDocument in uiDocuments)
            {
                if (_lightStyleSheet != null)
                {
                    uiDocument.rootVisualElement.styleSheets.Remove(_lightStyleSheet);
                }

                if (_darkStyleSheet != null && !uiDocument.rootVisualElement.styleSheets.Contains(_darkStyleSheet))
                {
                    uiDocument.rootVisualElement.styleSheets.Add(_darkStyleSheet);
                }
            }
        }

        // Event Handlers --------------------------------

        /// <summary>
        /// Applies the current theme based on the value of IsDark.
        /// </summary>
        private void OnIsDarkChanged()
        {
            if (IsDark.Value)
            {
                ApplyDarkTheme();
            }
            else
            {
                ApplyLightTheme();
            }
        }
    }
}