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
    public static BuildTarget PackTarget = BuildTarget.Android;

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

        var target = PackTarget;
        var projectName = GeneratePackageName();
        var outputPath = Application.dataPath.Replace("Asset", "BuildOutPut");
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        var fileName = string.Empty;
        switch (target)
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
                target,
                BuildOptions.None);
        
            Debug.Log("Build completed at " + outputFile);
        }
        catch (Exception e)
        {
            Debug.LogError($"Build Package Finish With Exception : {e}");
            Console.WriteLine(e);
            throw;
        }
    }

    public static string GeneratePackageName()
    {
        var date = DateTime.Now.ToString("yy_MMdd_hhmm");
        var projName = PlayerSettings.productName;
        //var projectName = $"{projName}-{Utils.GetBuildVersion().Replace(".", "_")}-{Utils.GetAssetVersion().Replace(".", "_")}_{date}";
        var projectName = $"{projName}-{date}";
        return projectName;
    }
}
#endif
