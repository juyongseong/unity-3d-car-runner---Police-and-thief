using UnityEngine;
using System.Collections;
using UnityEditor;
using Gamestrap;
using System.IO;

namespace Gamestrap
{
    public class UIIcons
    {
        private static Texture2D paletteNormal, paletteSelected, palettePressed;

        #region Static Properties

        public static Texture2D PaletteNormal
        {
            get
            {
                if (paletteNormal == null)
                    paletteNormal = TextureLoader.Load("gamestrap_palette_normal.psd");
                return paletteNormal;
            }
        }

        public static Texture2D PaletteSelected
        {
            get
            {
                if (paletteSelected == null)
                    paletteSelected = TextureLoader.Load("gamestrap_palette_selected.psd");
                return paletteSelected;
            }
        }

        public static Texture2D PalettePressed
        {
            get
            {
                if (palettePressed == null)
                    palettePressed = TextureLoader.Load("gamestrap_palette_pressed.psd");
                return palettePressed;
            }
        }
        #endregion
    }
}