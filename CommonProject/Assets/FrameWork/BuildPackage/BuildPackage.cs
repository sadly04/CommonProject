#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using FrameWork;
using UnityEditor.Build.Reporting;

public class BuildPackage : IProcess
{
#if UNITY_ANDROID
    public static BuildTarget Target = BuildTarget.Android;
#elif UNITY_IOS
    public static BuildTarget Target = BuildTarget.iOS;
#else
    public static BuildTarget Target = BuildTarget.StandaloneWindows;
#endif

    public void Execute()
    {
        // throw new NotImplementedException();
    }

    public static void ExecuteBuild()
    {
        var variables = Environment.GetCommandLineArgs();
        var environmentDic = (from variable in variables 
                where variable != null
                let strs = variable.Split('|') 
                where strs.Length == 2 
                select new KeyValuePair<string, string>(strs[0], strs[1]))
            .ToDictionary(ele =>ele.Key, ele => ele.Value);
        
        var levels = (from sence in EditorBuildSettings.scenes
            where sence != null
            where sence.enabled
            select sence.path).ToArray();

        var projectName = GeneratePackageName();
        var outputPath = Application.dataPath.Replace("Asset", "BuildOutPut");
        var fileName = string.Empty;
        switch (Target)
        {
            case BuildTarget.Android:
                fileName = Path.ChangeExtension(projectName, ".apk");
                break;
            case BuildTarget.iOS:
                fileName = projectName;
                break;
            default:
                fileName = Path.ChangeExtension(projectName, ".exe");
                break;
        }
        var outputFile = Path.Combine(outputPath, fileName);
        try
        {
            BuildPipeline.BuildPlayer(
                levels,
                outputFile,
                Target,
                BuildOptions.None);
        
            Debug.Log("Build completed at " + outputFile);
        }
        catch (Exception e)
        {
            Debug.LogError($"Build Finish With Exception : {e}");
            throw;
        }
    }

    public static string GeneratePackageName()
    {
        var date = DateTime.Now.ToString("yyMMddhhmm");
        var projName = PlayerSettings.productName;
        //var projectName = $"{projName}-{Utils.GetBuildVersion().Replace(".", "_")}-{Utils.GetAssetVersion().Replace(".", "_")}_{date}";
        var projectName = $"{projName}-{date}";
        return projectName;
    }
}
#endif
