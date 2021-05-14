using System.IO;
using UnityEngine;
using UnityEditor;

public class FileCreatorWindow : EditorWindow
{

    private DefaultAsset folder;
    private string filename;

    [MenuItem("Window/Custom/File Creator")]
    private static void Init()
    {

        FileCreatorWindow window = GetWindow<FileCreatorWindow>();

        window.titleContent = new GUIContent("File Creator");

        window.Show();

    }

    private void OnGUI()
    {

        if (Selection.activeObject != null && Selection.activeObject.GetType() == typeof(DefaultAsset))
        {

            folder = Selection.activeObject as DefaultAsset;

        }
        
        folder = (DefaultAsset)EditorGUILayout.ObjectField("Folder", folder, typeof(DefaultAsset), false);

        filename = EditorGUILayout.TextField("Filename", filename);

        if (GUILayout.Button("MonoBehaviour"))
        {

            CreateMonoBehaviour();

        }
        
        if (GUILayout.Button("Unlit Shader"))
        {

            CreateUnlitShader();

        }
        
        if (GUILayout.Button("Post Processing Shader"))
        {

            CreatePostProcessingShader();

        }
        
        if (GUILayout.Button("HLSL Include"))
        {

            CreateHLSL();

        }
        
        if (GUILayout.Button("Custom File"))
        {

            CreateCustomFile();

        }

    }

    private void CreateMonoBehaviour()
    {

        string filePath = AssetDatabase.GetAssetPath(folder) + "/" + filename + ".cs";

        string templatePath = "Assets/Editor/CustomWindows/FileCreator/FileTemplates/MonoBehaviourTemplate.cs";

        CreateNewFile(templatePath, filePath, "Template", filename);

    }

    private void CreateUnlitShader()
    {

        string filePath = AssetDatabase.GetAssetPath(folder) + "/" + filename + ".shader";

        string templatePath = "Assets/Editor/CustomWindows/FileCreator/FileTemplates/UnlitShaderTemplate.shader";

        CreateNewFile(templatePath, filePath, "Template", filename);

    }
    
    private void CreatePostProcessingShader()
    {

        string filePath = AssetDatabase.GetAssetPath(folder) + "/" + filename + ".shader";

        string templatePath = "Assets/Editor/CustomWindows/FileCreator/FileTemplates/PostProcessingShaderTemplate.shader";

        CreateNewFile(templatePath, filePath, "Template", filename);

    }

    private void CreateHLSL()
    {

        string filePath = AssetDatabase.GetAssetPath(folder) + "/" + filename + ".hlsl";

        string templatePath = "Assets/Editor/CustomWindows/FileCreator/FileTemplates/HLSLTemplate.hlsl";

        CreateNewFile(templatePath, filePath, "TEMPLATE", filename.ToUpper());

    }

    private void CreateNewFile(string templatePath, string newFilePath, string stringToReplace, string newString)
    {

        if (!File.Exists(newFilePath))
        {

            string[] templateData = File.ReadAllLines(templatePath);

            using (StreamWriter stream = File.CreateText(newFilePath))
            {

                for (int i = 0; i < templateData.Length; i++)
                {

                    stream.WriteLine(templateData[i].Replace(stringToReplace, newString));

                }

            }

            AssetDatabase.Refresh();

            Debug.Log("File created at " + newFilePath);

        }
        else
        {

            Debug.Log(newFilePath + " already exists");

        }

    }

    private void CreateCustomFile()
    {

        string filePath = AssetDatabase.GetAssetPath(folder) + "/" + filename;

        if (!File.Exists(filePath))
        {

            using (File.CreateText(filePath)) { };

            AssetDatabase.Refresh();

            Debug.Log("File created at " + filePath);

        }
        else
        {

            Debug.Log(filePath + " already exists");

        }

    }

}
