//=====================================================
// Autuor: sdy
// Time: 2018/10/30 17:18:26
// Description: 
//=====================================================
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
//Assets/Editor文件夹下
namespace SimpleFrame.Tool
{
    public class PackageTool
    {
        //打包操作
        public static void Build(string apkPath, string apkName, BuildTarget buildTarget)
        {
            if (!Directory.Exists(apkPath) || string.IsNullOrEmpty(apkName))
                return;
            if (BuildPipeline.isBuildingPlayer)
                return;
            //打包场景路径
            List<string> sceneNames = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene != null && scene.enabled)
                {
                    sceneNames.Add(scene.path);
                    //Debug.Log(scene.path);
                }
            }
            if(sceneNames.Count == 0)
            {
                Debug.Log("Build Scene Is None");
                return;
            }
            BuildTargetGroup buildTargetGroup = GetBuildTargetGroupByBuildTarget(buildTarget);
//切换平台
EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);
            //设置打包平台、标签
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, null);
            string path = apkPath + "/" + apkName;
            if (buildTarget == BuildTarget.Android)
                path += ".apk";
            else if(buildTarget == BuildTarget.iOS)
                path += "_IOS";
            Debug.Log("Start Build Package");
            //开始打包
            BuildPipeline.BuildPlayer(sceneNames.ToArray(), path, buildTarget, BuildOptions.None);
        }
        ////回调  场景运行前操作
        //[PostProcessScene(1)]
        //public static void BeforeBuild()
        //{
        //    Debug.Log("Build Start");
        //}
        //回调  打包后操作
        [PostProcessBuild(1)]
public static void AfterBuild(BuildTarget target, string pathToBuiltProject)
        {
            Debug.Log("Build Success  " + target + "  " + pathToBuiltProject);
            //打开文件或文件夹
            System.Diagnostics.Process.Start(pathToBuiltProject);
        }
        static BuildTargetGroup GetBuildTargetGroupByBuildTarget(BuildTarget buildTarget)
        {
            switch(buildTarget)
            {
                case BuildTarget.Android:
                    return BuildTargetGroup.Android;
                case BuildTarget.iOS:
                    return BuildTargetGroup.iOS;
                case BuildTarget.StandaloneOSX:
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return BuildTargetGroup.Standalone;
                    //······
                default:
                    return BuildTargetGroup.Standalone;
            }
        }
    }
}
#endif