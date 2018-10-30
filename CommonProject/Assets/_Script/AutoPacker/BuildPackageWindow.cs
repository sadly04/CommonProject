//=====================================================
// Autuor: sdy
// Time: 2018/10/30 17:10:47
// Description: 
//=====================================================
#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SimpleFrame.Tool;

//确认窗口, 可用快捷键启动
public class BuildPackageWindow : EditorWindow
{
    [MenuItem("MyTools/Package/Build Android Package #&amp;b")]
    public static void ShowPackageWindow()
    {
        //弹出确认窗口
        EditorWindow.GetWindow(typeof(BuildPackageWindow), false, "Build Package Window");
    }

    const string buildPathPlayerPrefsStr = "BuildPathPlayerPrefsStr";
    string showStr;
    Texture2D defaultIcon;
    //
    string apkPath, apkName;

    void OnEnable()
    {
            //确认信息
            showStr = "Package Info :";
            showStr += "\nCompany Name : " + PlayerSettings.companyName;
            showStr += "\nProduct Name : " + PlayerSettings.productName;
            showStr += "\nIdentifier : " + PlayerSettings.applicationIdentifier;
            showStr += "\nBundle Version : " + PlayerSettings.bundleVersion;
            showStr += "\nBundle Version Code : " + PlayerSettings.Android.bundleVersionCode;
            //图标Iocn
            Texture2D[] texture2Ds = PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.Android);
            defaultIcon = texture2Ds.Length > 0 ? texture2Ds[0] : null;
            //打包默认路径及默认文件名
            apkPath = PlayerPrefs.GetString(buildPathPlayerPrefsStr);
            apkName = PlayerSettings.productName;
        }

        void OnGUI()
        {
            GUILayout.Space(10);
            GUILayout.Label("Please Make Sure");
            defaultIcon = EditorGUILayout.ObjectField(defaultIcon, typeof(Texture2D), true) as Texture2D;
            EditorGUILayout.TextArea(showStr);
            GUILayout.Space(5);
            apkPath = EditorGUILayout.TextField("Package Path : ", apkPath);
            if (!Directory.Exists(apkPath))
                EditorGUILayout.TextArea("Package Path Is Error");
            apkName = EditorGUILayout.TextField("Package Name : ", apkName);
            if (string.IsNullOrEmpty(apkName))
                EditorGUILayout.TextArea("Package Name Is Empty");
            //调用系统窗口，选择保存路径
            if (GUILayout.Button("Choose Path"))
            {
                //EditorUtility.OpenFolderPanel("Choose Output Path", "", "");
                string tmpPath = EditorUtility.SaveFolderPanel("Choose Package Output Path", "", "");
                if (!string.IsNullOrEmpty(tmpPath))
                {
                    apkPath = tmpPath;
                    PlayerPrefs.SetString(buildPathPlayerPrefsStr, apkPath);
                }
            }
            GUILayout.Space(10);
            if (GUILayout.Button("Cancel"))
                this.Close();
            GUILayout.Space(5);
            if (Directory.Exists(apkPath) && !string.IsNullOrEmpty(apkName))
            {
                if (GUILayout.Button("Build Android"))
                {
                    this.Close();
                    PackageTool.Build(apkPath, apkName, BuildTarget.Android);
                }
                GUILayout.Space(5);
                if (GUILayout.Button("Build IOS"))
                {
                    this.Close();
                    PackageTool.Build(apkPath, apkName, BuildTarget.iOS);
                }
            }
            this.Repaint();
        }
        void OnDisable()
        {
            showStr = null;
defaultIcon = null;
        }
    }

#endif