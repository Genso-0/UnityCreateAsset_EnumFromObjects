
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EnumFromObjects : EditorWindow
{ 
    [MenuItem("Assets/Creation/Enum from Objects")]
    static void Init()
    {
        EnumFromObjects window = ScriptableObject.CreateInstance<EnumFromObjects>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        window.ShowPopup();
    }


    string enumName = "EnumType";
    void OnGUI()
    {
        EditorGUILayout.LabelField("Please enter a name for the enum", EditorStyles.wordWrappedLabel);
        GUILayout.Space(10); 
        enumName = EditorGUILayout.TextField("Enum type", enumName);
        GUILayout.Space(10);
        if (GUILayout.Button("Generate enum")) 
        {
            Create(enumName);
            this.Close();
        }
    }
    static void Create(string enumName)
    {
        List<string> names = new List<string>();
        foreach (UnityEngine.Object o in Selection.objects)//Get names of selected objects
        { 
            if (o == null)
            {
                continue;
            }
            names.Add((o as UnityEngine.Object).name); 
        }
        if (names.Count == 0)
        {
            Debug.Log("Nothing selected");
            return;
        }
        //get file path of first object in selections array
        var selected = Selection.objects[0];
        string savePath = AssetDatabase.GetAssetPath(selected);
        savePath = savePath.Substring(0, savePath.LastIndexOf('/') + 1);
        string newAssetName = savePath + enumName + ".cs"; //create path

        //write to file. We create an enum out of the names of the selected objects.
        using (StreamWriter outfile =
            new StreamWriter(newAssetName))
        {
            outfile.WriteLine("public enum " + enumName + " {");
            outfile.WriteLine("None,");
            for (int i = 0; i < names.Count; i++)
            {
                outfile.WriteLine(names[i] + ",");
            }
            outfile.WriteLine("}");
        }
        AssetDatabase.Refresh();
    }
}

