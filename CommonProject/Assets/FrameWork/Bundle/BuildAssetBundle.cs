using System.Collections;
using System.Collections.Generic;
using FrameWork;
using UnityEditor;
using UnityEngine;

public partial class BuildAssetBundle : IProcess
{
    public void Execute()
    {
        //throw new System.NotImplementedException();
    }

    public static void ExecuteBuild()
    {
        // 设置输出路径和文件名
        string outputPath = "Assets/StreamingAssets/mybundle";

        // 选择要打包的对象
        Object[] selectedObjects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        if (selectedObjects.Length > 0)
        {
            // 打包资源到指定路径
            BuildPipeline.BuildAssetBundle(
                null, // 没有场景需要打包
                selectedObjects,
                outputPath,
                BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,
                BuildTarget.StandaloneWindows64);

            Debug.Log("Asset bundle created at " + outputPath);
        }
        else
        {
            Debug.LogError("No object selected. Please select one or more objects to create asset bundle.");
        }
    }
}