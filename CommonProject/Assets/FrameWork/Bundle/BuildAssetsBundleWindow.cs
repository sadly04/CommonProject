#if UNITY_EDITOR
using UnityEditor;

public class BuildAssetsBundleWindow : EditorWindow
{
    [MenuItem("Tools/BuildBundle")]
    public static void OpenBuildPackageWindow()
    {
        BuildAssetBundle.ExecuteBuild();
    }
}

#endif
