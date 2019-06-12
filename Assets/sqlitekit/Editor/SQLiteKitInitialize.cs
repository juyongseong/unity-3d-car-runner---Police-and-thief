using System;
using System.Collections.Generic;
using UnityEditor;

namespace UnityStandardAssets.CrossPlatformInput.Inspector
{
    [InitializeOnLoad]
    public class SQLiteKitInitialize
    {
		static SQLiteKitInitialize()
        {/*
            var defines = GetDefinesList(buildTargetGroups[0]);
            if (!defines.Contains("SQLITE_NATIVE"))
            {
				SetEnabled("SQLITE_NATIVE", true, false);
				SetEnabled("SQLITE_NATIVE", true, true);
            }*/
        }


        [MenuItem("Edit/SQLite Native/Enable")]
        private static void Enable()
        {
			SetEnabled("SQLITE_NATIVE", true, true);
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
				case BuildTarget.iOS:
				case BuildTarget.StandaloneWindows:
				case BuildTarget.StandaloneWindows64:
				
                //case BuildTarget.WP8Player:
                //case BuildTarget.BlackBerry:
				//case BuildTarget.PSM: 
				//case BuildTarget.Tizen: 
				//case BuildTarget.WSAPlayer: 
				EditorUtility.DisplayDialog("SQLite Native",
                                                "Make sure that SQLite.native.zip appied on your project Plugin folder.",
                                                "OK");
                    break;

                default:
                    EditorUtility.DisplayDialog("SQLite Native",
                                                "Sqlite native library is not supported on current platform.",
                                                "OK");
                    break;
            }
        }


		[MenuItem("Edit/SQLite Native/Enable", true)]
        private static bool EnableValidate()
        {
			var defines = GetDefinesList(buildTargetGroups[0]);
			return !defines.Contains("SQLITE_NATIVE");
        }


		[MenuItem("Edit/SQLite Native/Disable")]
        private static void Disable()
        {
			SetEnabled("SQLITE_NATIVE", false, true);
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
				case BuildTarget.iOS:
				case BuildTarget.StandaloneWindows:
				case BuildTarget.StandaloneWindows64:
					EditorUtility.DisplayDialog("SQLite Native",
						"You have disabled SQLite Native libraries.",
	                                                "OK");
                    break;
            }
        }


		[MenuItem("Edit/SQLite Native/Disable", true)]
        private static bool DisableValidate()
        {
			var defines = GetDefinesList(buildTargetGroups[0]);
			return defines.Contains("SQLITE_NATIVE");
        }


        private static BuildTargetGroup[] buildTargetGroups = new BuildTargetGroup[]
            {
				BuildTargetGroup.Standalone,
                BuildTargetGroup.Android,
				BuildTargetGroup.iOS
            };

        private static void SetEnabled(string defineName, bool enable, bool mobile)
        {
            //Debug.Log("setting "+defineName+" to "+enable);
            foreach (var group in buildTargetGroups)
            {
                var defines = GetDefinesList(group);
                if (enable)
                {
                    if (defines.Contains(defineName))
                    {
                        return;
                    }
                    defines.Add(defineName);
                }
                else
                {
                    if (!defines.Contains(defineName))
                    {
                        return;
                    }
                    while (defines.Contains(defineName))
                    {
                        defines.Remove(defineName);
                    }
                }
                string definesString = string.Join(";", defines.ToArray());
                PlayerSettings.SetScriptingDefineSymbolsForGroup(group, definesString);
            }
        }


        private static List<string> GetDefinesList(BuildTargetGroup group)
        {
            return new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';'));
        }
    }
}
