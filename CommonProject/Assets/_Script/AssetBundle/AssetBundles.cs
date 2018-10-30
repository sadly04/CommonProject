//=====================================================
// Autuor: sdy
// Time: #CreateTime#
// Description: 
//=====================================================
#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;//�����ڼ���  
using UnityEditor;//�༭״̬��  

public class AssetBundleEdi : Editor
{

    public static Object[] Objs = new Object[] { };

    [MenuItem("Assets/AssetsBundle/BuildSelectObjects")]
    static void BuildSelect()
    {
        //��ȡ����ѡ�еĶ���  
        Objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        //����һ���༭���ڣ�  
        AssetBundleWindow.ShowWindow();
    }

    /// <summary>  
    /// ��ʼ�����  
    /// </summary>  
    public static void StartBuild()
    {
        Debug.Log("��ʼ�����");
        string path = AssetBundleWindow.AsbPath;
        Debug.Log("ѡ��·����" + path);

        //���ó�asb[]  
        AssetBundleBuild abb = new AssetBundleBuild();
        abb.assetNames = new string[Objs.Length];
        for (int i = 0; i < Objs.Length; i++)
        {
            abb.assetNames[i] = AssetDatabase.GetAssetPath(Objs[i]);
        }
        if (AssetBundleWindow.IsWindows)
        {
            //����·����  
            Debug.Log("��Ҫ�����Windows");
            abb.assetBundleName = AssetBundleWindow.AssetBudleName + "_windows.UnityAsb";
            //��ʼ�����  
            BuildPipeline.BuildAssetBundles(path, new AssetBundleBuild[] { abb }, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        }
        if (AssetBundleWindow.IsAndorid)
        {
            //����·����  
            Debug.Log("��Ҫ�������׿");
            abb.assetBundleName = AssetBundleWindow.AssetBudleName + "_android.UnityAsb";
            //��ʼ�����  
            BuildPipeline.BuildAssetBundles(path, new AssetBundleBuild[] { abb }, BuildAssetBundleOptions.None, BuildTarget.Android);
        }
        if (AssetBundleWindow.IsApple)
        {
            //����·����  
            Debug.Log("��Ҫ�����ƻ��");
            abb.assetBundleName = AssetBundleWindow.AssetBudleName + "_ios.UnityAsb";
            //��ʼ�����  
            BuildPipeline.BuildAssetBundles(path, new AssetBundleBuild[] { abb }, BuildAssetBundleOptions.None, BuildTarget.iOS);
        }
        Debug.Log("�����ɣ�");
    }
}

/// <summary>  
/// �������ڣ�  
/// </summary>  
public class AssetBundleWindow : EditorWindow
{
    /// <summary>  
    /// asb�����֣�  
    /// </summary>  
    public static string AssetBudleName;
    /// <summary>  
    /// asb����·����  
    /// </summary>  
    public static string AsbPath;
    /// <summary>  
    /// �Ƿ���Windows�´����  
    /// </summary>  
    public static bool IsWindows = false;
    /// <summary>  
    /// �Ƿ��ڰ�׿�´����  
    /// </summary>  
    public static bool IsAndorid = false;
    /// <summary>  
    /// �Ƿ���ƻ���´����  
    /// </summary>  
    public static bool IsApple = false;


    AssetBundleWindow()
    {
        titleContent = new GUIContent("��Դ���");
    }


    public static void ShowWindow()
    {
        GetWindow(typeof(AssetBundleWindow));
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 15;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        if (AssetBundleEdi.Objs.Length > 0)
        {
            GUILayout.Label("��ǰ�ܹ�ѡ��" + AssetBundleEdi.Objs.Length + "����Դ��");

            //�����ļ�·��ѡ��  
            GUILayout.Space(10);
            if (GUILayout.Button("·��ѡ��", GUILayout.Width(200)))
            {
                AsbPath = EditorUtility.SaveFolderPanel("��ѡ����·��", Application.streamingAssetsPath, AssetBudleName);
                //���￪�������ػ��ƣ�  
                Repaint();
            }

            GUI.skin.label.fontSize = 10;
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            if (string.IsNullOrEmpty(AsbPath))
            {
                //�������ѡ������  
                GUILayout.Label("û��ѡ����·����");
            }
            else
            {
                //�������ѡ������  
                GUILayout.Label("��ǰѡ����·����" + AsbPath);

                //��3��togle  
                IsWindows = GUI.Toggle(new Rect(10, 100, 600, 20), IsWindows, "�����Windowsƽ̨");

                IsAndorid = GUI.Toggle(new Rect(10, 120, 600, 20), IsAndorid, "�����Androidƽ̨");

                IsApple = GUI.Toggle(new Rect(10, 140, 600, 20), IsApple, "�����IOSƽ̨");

                //�����ļ������ť  
                GUILayout.Space(100);
                GUILayout.Label("�����뵼���İ�����");
                //����һ�����������  
                AssetBudleName = EditorGUILayout.TextField(AssetBudleName);

                GUILayout.Space(10);

                if (GUILayout.Button("��ʼ���", GUILayout.Width(200)))
                {
                    if (string.IsNullOrEmpty(AssetBudleName))
                    {
                        Debug.Log("���������ļ�����");
                    }
                    else
                    {
                        AssetBundleEdi.StartBuild();
                    }
                }
            }


        }
        else
        {
            GUILayout.Label("��ǰδѡ���κ���Դ��");
        }
    }
}

#endif