using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class ShaderGradientDrawer : MaterialPropertyDrawer
{

    private Gradient gradient;
    private Texture2D gradientTexture;
    private int textureResolution = 256;
    private bool needToBake;

    private const string subAssetTextureEnding = "_BakedTexture";
    private const string subAssetDataEnding = "_Data";

    public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
    {

        position.height = 20;

        EditorGUI.LabelField(position, label);

        Rect gradientPosition = position;
        float newWidth = gradientPosition.width * 0.6f;
        gradientPosition.x += gradientPosition.width - newWidth;
        gradientPosition.width = newWidth;

        if (gradient == null)
        {

            LoadGradientData(prop, editor.target as Material);

        }

        EditorGUI.BeginChangeCheck();

        gradient = EditorGUI.GradientField(gradientPosition, gradient);

        if (EditorGUI.EndChangeCheck())
        {

            DisplayGradientChange(prop);

            needToBake = true;

        }

        EditorGUI.indentLevel = 1;

        EditorGUI.BeginChangeCheck();

        position.y += 22;

        textureResolution = EditorGUI.DelayedIntField(position, "Resolution", textureResolution);

        if (EditorGUI.EndChangeCheck())
        {

            DisplayGradientChange(prop);

            needToBake = true;

        }

        EditorGUI.indentLevel = 0;

        Assembly assembly = Assembly.GetAssembly(typeof(Editor));
        Type type = assembly.GetType("UnityEditor.GradientPicker");
        FieldInfo field = type.GetField("s_GradientPicker", BindingFlags.NonPublic | BindingFlags.Static);

        if (field.GetValue(null) == null)
        {

            if (needToBake)
            {

                needToBake = false;

                BakeGradientTexture(prop, editor.target as Material);

            }

        }

    }

    public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
    {

        return 42;

    }

    private Texture2D GetTextureSubAsset(MaterialProperty prop, string materialPath)
    {

        Texture2D gradientBakedTexture = null;

        UnityEngine.Object[] subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(materialPath);

        for (int i = 0; i < subAssets.Length; i++)
        {

            if (subAssets[i].name == prop.name + subAssetTextureEnding)
            {

                gradientBakedTexture = subAssets[i] as Texture2D;

            }

        }

        return gradientBakedTexture;

    }

    private GradientData GetDataSubAsset(MaterialProperty prop, string materialPath)
    {

        GradientData gradientData = null;

        UnityEngine.Object[] subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(materialPath);

        for (int i = 0; i < subAssets.Length; i++)
        {

            if (subAssets[i].name == prop.name + subAssetDataEnding)
            {

                gradientData = subAssets[i] as GradientData;

            }

        }

        return gradientData;

    }

    private void ClearOldSubAssets(MaterialProperty prop, string materialPath)
    {

        UnityEngine.Object[] subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(materialPath);

        for (int i = 0; i < subAssets.Length; i++)
        {

            if (subAssets[i].name == prop.name + subAssetTextureEnding || subAssets[i].name == prop.name + subAssetDataEnding)
            {

                AssetDatabase.RemoveObjectFromAsset(subAssets[i]);

            }

        }

        AssetDatabase.SaveAssets();

    }

    private void DefaultGradient()
    {

        gradient = new Gradient();

        textureResolution = 256;

    }

    private void LoadGradientData(MaterialProperty prop, Material material)
    {

        GradientData gradientData = GetDataSubAsset(prop, AssetDatabase.GetAssetPath(material));

        if (gradientData == null)
        {

            DefaultGradient();

        }
        else
        {

            gradient = gradientData.gradient;

            textureResolution = gradientData.resolution;

        }

    }

    private void WriteGradientToTexture()
    {

        gradientTexture = new Texture2D(textureResolution, 1, TextureFormat.RGBA32, false, true);
        gradientTexture.filterMode = FilterMode.Point;
        gradientTexture.wrapMode = TextureWrapMode.Clamp;
        gradientTexture.anisoLevel = 0;

        for (int i = 0; i < textureResolution; i++)
        {

            Color color = gradient.Evaluate(((textureResolution - i) / (float)textureResolution));

            gradientTexture.SetPixel(i, 0, color);

        }

        gradientTexture.Apply();

    }

    private void DisplayGradientChange(MaterialProperty prop)
    {

        WriteGradientToTexture();

        prop.textureValue = gradientTexture;

    }

    private void BakeGradientTexture(MaterialProperty prop, Material material)
    {

        WriteGradientToTexture();

        SaveDataTexture(prop, material);

    }

    private void SaveDataTexture(MaterialProperty prop, Material material)
    {

        string materialPath = AssetDatabase.GetAssetPath(material);

        ClearOldSubAssets(prop, materialPath);

        gradientTexture.name = prop.name + subAssetTextureEnding;

        AssetDatabase.AddObjectToAsset(gradientTexture, material);

        GradientData gradientData = ScriptableObject.CreateInstance<GradientData>();

        gradientData.name = prop.name + subAssetDataEnding;
        gradientData.gradient = gradient;
        gradientData.resolution = textureResolution;

        AssetDatabase.AddObjectToAsset(gradientData, material);

        AssetDatabase.SaveAssets();

        prop.textureValue = GetTextureSubAsset(prop, materialPath);

    }

}
