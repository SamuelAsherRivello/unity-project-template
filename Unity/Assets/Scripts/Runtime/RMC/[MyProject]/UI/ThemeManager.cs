using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace RMC.MyProject.Scenes
{
    public class ThemeManager
    {
        private readonly StyleSheet _lightStyleSheet;
        private readonly StyleSheet _darkStyleSheet;
        private bool _isDark = true;

        /// <summary>
        /// Property to get or set the current theme. Defaults to Dark.
        /// Setting this property toggles between Light and Dark themes.
        /// </summary>
        public bool IsDark
        {
            get => _isDark;
            set
            {
                if (_isDark != value)
                {
                    _isDark = value;
                    ApplyTheme();
                }
            }
        }

        /// <summary>
        /// Constructor for ThemeManager. Initializes the theme to Dark.
        /// </summary>
        /// <param name="lightStyleSheet"></param>
        /// <param name="darkStyleSheet"></param>
        public ThemeManager(StyleSheet lightStyleSheet, StyleSheet darkStyleSheet)
        {
            _lightStyleSheet = lightStyleSheet;
            _darkStyleSheet = darkStyleSheet;

            // Initialize with the dark theme
            ApplyDarkTheme();
        }


        /// <summary>
        /// Applies the current theme based on the value of IsDark.
        /// </summary>
        private void ApplyTheme()
        {
            if (_isDark)
            {
                ApplyDarkTheme();
            }
            else
            {
                ApplyLightTheme();
            }
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
    }
}