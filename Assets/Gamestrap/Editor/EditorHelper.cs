using System;
using UnityEngine;

namespace Gamestrap
{
    public class EditorHelper
    {
        
        public static Texture2D LoadTexture(string path)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(System.IO.File.ReadAllBytes(path));
            texture.Apply();
            return texture;
        }

        public static void BackgroundColor(Color color, Action guiContent)
        {
            Color defaultColor = GUI.contentColor;
            GUI.contentColor = color;
            guiContent();
            GUI.contentColor = defaultColor;
        }

        /// <summary>
        /// For it to work you need to place before the element you want to check the event the following line:
        /// GUI.SetNextControlName("<UniqueName>");
        /// </summary>
        /// <param name="controlName"></param>
        /// <param name="type"></param>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        public static bool CheckEvent(string controlName, EventType type, KeyCode keyCode)
        {
            return controlName == GUI.GetNameOfFocusedControl()
                && Event.current.type == type 
                && keyCode == Event.current.keyCode;
        }
    }
}