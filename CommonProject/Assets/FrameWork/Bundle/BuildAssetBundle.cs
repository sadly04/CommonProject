#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FrameWork;
using UnityEditor;
using UnityEngine;

public partial class BuildAssetBundle : IProcess
{
    public static BuildTarget BundleTarget = BuildTarget.Android;
    public static List<AssetBundleBuild> BundleBuilds = new List<AssetBundleBuild>();

    public void Execute()
    {
        //throw new System.NotImplementedException();
    }

    public static void ExecuteBuild()
    {
        // 设置输出路径和文件名
        string outputPath = "Assets/StreamingAssets/updata";

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        AddFolder(Path.Combine(Application.dataPath, "Data/Material"));

        var target = BundleTarget;
        try
        {
            BuildPipeline.BuildAssetBundles(
                outputPath,
                BundleBuilds.ToArray(),
                BuildAssetBundleOptions.AssetBundleStripUnityVersion,
                target);
        }
        catch (Exception e)
        {
            Debug.LogError($"Build Bundle Finish With Exception : {e}");
            Console.WriteLine(e);
            throw;
        }
    }

    public static void AddFolder(string folderPath)
    {
        if (string.IsNullOrEmpty(folderPath))
            return;

        AssetBundleBuild bundle = new AssetBundleBuild();
        bundle.assetBundleName = "bundle_" + Path.GetFileNameWithoutExtension(folderPath);
        //bundle.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(folderPath);

        // 获取所有文件路径
        List<string> assetsPaths = new List<string>();
        DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
        FileInfo[] fileInfos = dirInfo.GetFiles("*", SearchOption.AllDirectories);
        foreach (FileInfo fileInfo in fileInfos)
        {
            string filePath = fileInfo.FullName.Replace("\\", "/");
            //filePath = "Assets" + filePath.Replace(Application.dataPath, "");
            if (filePath.EndsWith(".meta") == false)
            {
                string assetPath = "Assets" + filePath.Replace(Application.dataPath, "");
                assetsPaths.Add(assetPath);
                SetAssetBundleLabel(assetPath, bundle.assetBundleName, String.Empty);
            }
        }

        bundle.assetNames = assetsPaths.ToArray();
        BundleBuilds.Add(bundle);
        Debug.Log("Added folder to AssetBundle queue: " + folderPath);
    }

    public static void SetAssetBundleLabel(string assetPath, string assetBundleName, string assetBundleVariant)
    {
        AssetImporter importer = AssetImporter.GetAtPath(assetPath);

        if (importer == null)
        {
            Debug.LogError("Asset not found at path: " + assetPath);
            return;
        }

        importer.SetAssetBundleNameAndVariant(assetBundleName, assetBundleVariant);

        Debug.Log("AssetBundle label set for asset: " + assetPath);
    }
}
#endif