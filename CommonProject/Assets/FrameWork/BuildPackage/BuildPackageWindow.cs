﻿#if UNITY_EDITOR
using UnityEditor;

public class BuildPackageWindow : EditorWindow
{
   [MenuItem("Tools/BuildPackage/Windows")]
   public static void OpenBuildPackageWindow()
   {
      BuildPackage.ExcuteBuild();
   }
}
#endif