#if UNITY_EDITOR
using UnityEditor;

public class BuildPackageWindow : EditorWindow
{
   [MenuItem("Tools/BuildPackage")]
   public static void OpenBuildPackageWindow()
   {
      BuildPackage.ExecuteBuild();
   }
}
#endif