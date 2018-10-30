using System.IO;
using UnityEditor;
using UnityEngine;

public class ScriptModify : UnityEditor.AssetModificationProcessor
{
    private static readonly string stateCode = "" +
        "//=====================================================\r\n" +
        "// Autuor: sdy\r\n" +
        "// Time: #CreateTime#\r\n" +
        "// Description: \r\n" +
        "//=====================================================\r\n";

    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs"))
        {
            string allText = stateCode;
            allText += File.ReadAllText(path);

            allText = allText.Replace("#CreateTime#", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            File.WriteAllText(path, allText);
        }

        
    }

}
