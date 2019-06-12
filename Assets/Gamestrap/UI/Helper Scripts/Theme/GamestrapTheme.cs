using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Gamestrap
{
    /// <summary>
    /// Scriptable Object incharge of saving all of the UI settings of Gamestrap UI Toolkit
    /// </summary>
    public class GamestrapTheme : ScriptableObject
    {
        private static int recursiveLevel = 0;

        public List<ColorSet> palette = new List<ColorSet>();
        public List<EffectSet> effectSets = new List<EffectSet>();

        public void ApplyTheme()
        {
            foreach (ColorSet set in palette)
            {
                ApplyColorSetTag(set);
            }

            foreach (EffectSet set in effectSets)
            {
                ApplyEffectSetTag(set);
            }
        }

        #region Color Methods
        private void ApplyColorSetTag(ColorSet set)
        {
            if (set.tag.Length > 0 && set.tag != "Untagged")
            {
                GameObject[] list = GameObject.FindGameObjectsWithTag(set.tag);
                foreach (GameObject go in list)
                {
                    recursiveLevel = 0;
                    AssignColorsToSelection(go, set);
                }
            }
        }

        private void AssignColorsToSelection(GameObject gameObject, ColorSet colorSet)
        {
            recursiveLevel++;
            if (gameObject.GetComponent<UnityEngine.UI.Button>())
            {
                UnityEngine.UI.Button button = gameObject.GetComponent<UnityEngine.UI.Button>();
                SetColorBlock(button, colorSet);
                SetDetailColor(gameObject, colorSet);
            }
            else if (gameObject.GetComponent<UnityEngine.UI.InputField>())
            {
                UnityEngine.UI.InputField input = gameObject.GetComponent<UnityEngine.UI.InputField>();
                SetColorBlock(input, colorSet);
                input.selectionColor = colorSet.highlighted;
                input.textComponent.color = colorSet.pressed;
                input.placeholder.color = colorSet.highlighted;
            }
            else if (gameObject.GetComponent<UnityEngine.UI.Scrollbar>())
            {
                UnityEngine.UI.Scrollbar sb = gameObject.GetComponent<UnityEngine.UI.Scrollbar>();
                SetColorBlock(sb, colorSet);
                gameObject.GetComponent<UnityEngine.UI.Image>().color = colorSet.disabled;
            }
            else if (gameObject.GetComponent<UnityEngine.UI.Slider>())
            {
                UnityEngine.UI.Slider slider = gameObject.GetComponent<UnityEngine.UI.Slider>();
                SetColorBlock(slider, colorSet);
                slider.fillRect.gameObject.GetComponent<UnityEngine.UI.Image>().color = colorSet.normal;
                SetTextColorRecursive(gameObject, colorSet);
            }
            else if (gameObject.GetComponent<UnityEngine.UI.Toggle>())
            {
                UnityEngine.UI.Toggle toggle = gameObject.GetComponent<UnityEngine.UI.Toggle>();
                SetColorBlock(toggle, colorSet);
                toggle.graphic.color = colorSet.normal;
                SetTextColorRecursive(gameObject, colorSet);
            }
            else if (gameObject.transform.childCount > 0) // Recursive search for components
            {
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    AssignColorsToSelection(gameObject.transform.GetChild(i).gameObject, colorSet);
                }
            }
            else if (recursiveLevel == 1)
            {
                if (gameObject.GetComponent<UnityEngine.UI.Image>())
                {
                    UnityEngine.UI.Image image = gameObject.GetComponent<UnityEngine.UI.Image>();
                    image.color = colorSet.normal;
                }
                else if (gameObject.GetComponent<UnityEngine.UI.Text>())
                {
                    UnityEngine.UI.Text text = gameObject.GetComponent<UnityEngine.UI.Text>();
                    text.color = colorSet.normal;
                }
            }
        }
        #endregion

        #region Effect Methods
        private void ApplyEffectSetTag(EffectSet set)
        {
            if (set.tag.Length > 0 && set.tag != "Untagged")
            {
                GameObject[] list = GameObject.FindGameObjectsWithTag(set.tag);
                foreach (GameObject go in list)
                {
                    UIBehaviour[] effects;
#if UNITY_4_6 || UNITY_5_0 || UNITY_5_1
                    effects = go.GetComponents<BaseVertexEffect>();
#else
                    effects = go.GetComponents<BaseMeshEffect>();
#endif
                    foreach (UIBehaviour behaviour in effects)
                    {
                        DestroyImmediate(behaviour);
                    }

                    ActivateEffects(go, set);
                    go.SetActive(false);
                    go.SetActive(true);
                }
            }
        }

        private void ActivateEffects(GameObject gameObject, EffectSet set)
        {
            if (gameObject.GetComponent<UnityEngine.UI.Image>())
            {
                if (set.gradient)
                {
                    GradientEffect gradientEffect = gameObject.GetComponent<GradientEffect>();
                    if (!gradientEffect)
                    {
                        
                        gradientEffect = gameObject.AddComponent<GradientEffect>();
                    }
                    gradientEffect.top = set.gradientTop;
                    gradientEffect.bottom = set.gradientBottom;
                }
                else if (gameObject.GetComponent<GradientEffect>())
                {
                    DestroyImmediate(gameObject.GetComponent<GradientEffect>());
                }

                if (set.radialGradient)
                {
                    RadialGradientEffect gradientEffect = gameObject.GetComponent<RadialGradientEffect>();
                    if (!gradientEffect)
                    {
                        gradientEffect = gameObject.AddComponent<RadialGradientEffect>();
                    }
                    gradientEffect.centerColor = set.radialColor;
                    gradientEffect.centerPosition = set.centerPosition;
                    gradientEffect.radius = set.radius;
                }
                else if (gameObject.GetComponent<RadialGradientEffect>())
                {
                    DestroyImmediate(gameObject.GetComponent<RadialGradientEffect>());
                }
            }

            if (gameObject.GetComponent<UnityEngine.UI.Image>() || gameObject.GetComponent<UnityEngine.UI.Text>())
            {
                if (set.shadow)
                {
                    ShadowEffect shadowEffect = gameObject.GetComponent<ShadowEffect>();
                    if (!shadowEffect)
                    {
                        shadowEffect = gameObject.AddComponent<ShadowEffect>();
                    }
                    shadowEffect.effectDistance = set.shadowOffset;
                    shadowEffect.effectColor = set.shadowColor;
                }
                else if (gameObject.GetComponent<ShadowEffect>())
                {
                    DestroyImmediate(gameObject.GetComponent<ShadowEffect>());
                }

                if (set.mirrorEffect)
                {
                    MirrorEffect mirrorEffect = gameObject.GetComponent<MirrorEffect>();
                    if (!mirrorEffect)
                    {
                        mirrorEffect = gameObject.AddComponent<MirrorEffect>();
                    }
                    mirrorEffect.top = set.mirrorTop;
                    mirrorEffect.bottom = set.mirrorBottom;
                    mirrorEffect.offset = set.mirrorOffset;
                    mirrorEffect.scale = set.mirrorScale;
                    mirrorEffect.skew = set.mirrorSkew;
                }
                else if (gameObject.GetComponent<MirrorEffect>())
                {
                    DestroyImmediate(gameObject.GetComponent<MirrorEffect>());
                }

                if (set.skewEffect)
                {
                    SkewEffect skewEffect = gameObject.GetComponent<SkewEffect>();
                    if (!skewEffect)
                    {
                        skewEffect = gameObject.AddComponent<SkewEffect>();
                    }
                    skewEffect.skew = set.skew;
                    skewEffect.perspective = set.perspective;
                }
                else if (gameObject.GetComponent<SkewEffect>())
                {
                    DestroyImmediate(gameObject.GetComponent<SkewEffect>());
                }

            }

            if (gameObject.transform.childCount > 0) // Recursive search for components
            {
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    ActivateEffects(gameObject.transform.GetChild(i).gameObject, set);
                }
            }
        }
        #endregion

        #region Recursive Color Setting

        /// <summary>
        /// Sets all of the current variables to a ColorBlock and returns it
        /// </summary>
        /// <param name="cb">The color block from the UI element</param>
        /// <returns>The color block with the new values</returns>
        public void SetColorBlock(UnityEngine.UI.Selectable selectable, ColorSet colorSet)
        {
            UnityEngine.UI.Image img = selectable.GetComponent<UnityEngine.UI.Image>();
            if (selectable.transition == UnityEngine.UI.Selectable.Transition.ColorTint)
            {
                if (img)
                {
                    img.color = Color.white;
                }
                UnityEngine.UI.ColorBlock cb = selectable.colors;
                cb.normalColor = colorSet.normal;
                cb.highlightedColor = colorSet.highlighted;
                cb.pressedColor = colorSet.pressed;
                cb.disabledColor = colorSet.disabled;
                selectable.colors = cb;
            }
            else if (selectable.GetComponent<UnityEngine.UI.Image>())
            {
                img.color = colorSet.normal;
            }
        }

        /// <summary>
        /// Searches if the GameObject has children and if the children have components type Image or Text.
        /// If they do then it will assign the variable detail to the Color of the image or text.
        /// </summary>
        /// <param name="go">UI GameObject with children</param>
        private void SetDetailColor(GameObject go, ColorSet colorSet)
        {
            int children = go.transform.childCount;
            for (int i = 0; i < children; i++)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                if (child.GetComponent<UnityEngine.UI.Image>())
                {
                    child.GetComponent<UnityEngine.UI.Image>().color = colorSet.detail;
                }
                if (child.GetComponent<UnityEngine.UI.Text>())
                {
                    UnityEngine.UI.Text t = child.GetComponent<UnityEngine.UI.Text>();
                    t.color = colorSet.detail;
                }
            }
        }

        /// <summary>
        /// Looks recursively for component Text in the GameObjects children 
        /// and also changes the color to the Detail variable if it finds any.
        /// </summary>
        /// <param name="go">UI GameObject with children</param>
        private void SetTextColorRecursive(GameObject go, ColorSet colorSet)
        {
            int children = go.transform.childCount;
            for (int i = 0; i < children; i++)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                if (child.GetComponent<UnityEngine.UI.Text>())
                {
                    UnityEngine.UI.Text t = child.GetComponent<UnityEngine.UI.Text>();
                    t.color = colorSet.normal;
                }
                SetTextColorRecursive(child, colorSet);
            }
        }

        #endregion
    }
    /// <summary>
    /// ColorSet is a color in the pallete of the Gamestrap UI Toolkit which handles the colors to be set to any UI in the scene
    /// </summary>
    [System.Serializable]
    public class ColorSet
    {
        public string name = "";
        public string tag = "";
        public Color normal;
        public Color highlighted;
        public Color pressed;
        public Color disabled;
        public Color detail;

        public ColorSet()
        {

        }

        public ColorSet(string name, string tag, Color normal, Color highlighted, Color pressed, Color disabled, Color detail)
        {
            this.name = name;
            this.tag = tag;
            this.normal = normal;
            this.highlighted = highlighted;
            this.pressed = pressed;
            this.disabled = disabled;
            this.detail = detail;
        }

        public ColorSet(Color[] colors)
        {
            if (colors.Length >= 5)
            {
                normal = colors[0];
                highlighted = colors[1];
                pressed = colors[2];
                disabled = colors[3];
                detail = colors[4];
            }
        }

        public ColorSet Clone()
        {
            return (ColorSet)this.MemberwiseClone();
        }
    }

    /// <summary>
    /// Effect is a set of effects in the Gamestrap UI Toolkit which handles the effects to be set to any UI in the scene
    /// </summary>
    [System.Serializable]
    public class EffectSet
    {
        public string name = "";
        public string tag = "";

        public bool gradient;
        public Color gradientTop = Color.white;
        public Color gradientBottom = Color.black;

        public bool shadow;
        public Color shadowColor = Color.black;
        public Vector2 shadowOffset = new Vector2(2, -2);

        public bool radialGradient;
        public Color radialColor = Color.white;
        public Vector2 centerPosition;
        public float radius = 100f;

        public bool mirrorEffect;
        public float mirrorScale = 1f;
        public Vector2 mirrorOffset;
        public float mirrorSkew;
        public Color mirrorTop = Color.white;
        public Color mirrorBottom = Color.white;

        public bool skewEffect;
        public float skew;
        public float perspective = 1f;


        public EffectSet Clone()
        {
            return (EffectSet)this.MemberwiseClone();
        }
    }
}