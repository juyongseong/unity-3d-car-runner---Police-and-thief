using UnityEngine;
using UnityEditor;
using System.IO;

namespace Gamestrap
{
    public class TextureLoader
    {
        private static string path;

        private static void LoadPath()
        {
            string[] assets = AssetDatabase.FindAssets("t:Texture gamestrap_");

            if (assets.Length == 0)
            {
                Debug.LogError("GamestrapUI name not found, make sure you have the Gamestrap scripts in your project.");
                return;
            }

            path = AssetDatabase.GUIDToAssetPath(assets[0]);
            DirectoryInfo dir = Directory.GetParent(path);
            path = "Assets" + dir.FullName.Substring(Application.dataPath.Length) + "\\";
        }

        public static Texture2D Load(string assetName)
        {
            if (path == null || path.Length == 0)
                LoadPath();
            return (Texture2D) AssetDatabase.LoadAssetAtPath(path + assetName,typeof(Texture2D)); ;
        }
    }
}