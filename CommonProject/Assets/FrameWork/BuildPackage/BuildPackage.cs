#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

using FrameWork.Common;
using UnityEditor.Build.Reporting;

public class BuildPackage
{
#if UNITY_ANDROID
    public static BuildTarget Target = BuildTarget.Android;
#elif UNITY_IOS
    public static BuildTarget Target = BuildTarget.iOS;
#else
    public static BuildTarget Target = BuildTarget.StandaloneWindows;
#endif
    
    public static void ExcuteBuild()
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

        var outputFile = string.Empty;
        switch (Target)
        {
            case BuildTarget.Android:
                outputFile = Path.ChangeExtension(GenerateProjectName(), ".apk");
                break;
            case BuildTarget.iOS:
                outputFile = GenerateProjectName();
                break;
            default:
                outputFile = Path.ChangeExtension(GenerateProjectName(), ".exe");
                break;
        }
        try
        {
            var buildReport = BuildPipeline.BuildPlayer(levels, outputFile, Target, BuildOptions.None);
            var result = buildReport.summary.result;
            if (result != BuildResult.Succeeded)
            {
                throw new Exception($"Build Finish With Result : {result}");
            }
            else
            {
                Debug.Log($"Build Finish With Result : {result}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Build Finish With Exception : {e}");
            throw;
        }
    }

    public static string GenerateProjectName()
    {
        var date = DateTime.Now.ToString("yyMMddhhmm");
        var codeName = "Test";
        var projectName = $"{codeName}_{Utils.GetBuildVersion()}_{Utils.GetAssetVersion()}_{date}";
        return projectName;
    }
}
#endif
