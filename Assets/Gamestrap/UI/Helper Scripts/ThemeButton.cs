using UnityEngine;
namespace Gamestrap
{
    /// <summary>
    /// Automatically registers to the UI button and 
    /// applies the theme when the button is pressed
    /// </summary>
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class ThemeButton : MonoBehaviour
    {
        public GamestrapTheme theme;

        void Awake()
        {
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                if (theme != null)
                    theme.ApplyTheme();
            });
        }

    }
}