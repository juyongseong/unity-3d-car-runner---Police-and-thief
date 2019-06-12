using UnityEngine;
namespace Gamestrap
{
    public class ThemeToggler : MonoBehaviour
    {

        public GamestrapTheme[] themes;
        private int index;

        public void NextTheme()
        {
            if (themes.Length == 0)
            {
                return;
            }
            index = (index + 1) % themes.Length;
            themes[index].ApplyTheme();
        }
    }
}