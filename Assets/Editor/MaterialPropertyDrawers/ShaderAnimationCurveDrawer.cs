using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class ShaderAnimationCurveDrawer : MaterialPropertyDrawer
{

    private AnimationCurve curve;
    private Rect curveRange = new Rect(0, 0, 1, 1);
    private Texture2D curveTexture;
    private int textureResolution = 256;
    private bool needToBake;

    private const string subAssetTextureEnding = "_BakedTexture";
    private const string subAssetDataEnding = "_Data";

    public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
    {

        position.height = 20;
        
        EditorGUI.LabelField(position, label);

        Rect curvePosition = position;
        float newWidth = curvePosition.width * 0.6f;
        curvePosition.x += curvePosition.width - newWidth;
        curvePosition.width = newWidth;

        if (curve == null)
        {

            LoadCurveData(prop, editor.target as Material);

        }

        EditorGUI.BeginChangeCheck();

        curve = EditorGUI.CurveField(curvePosition, curve, Color.green, curveRange);

        if (EditorGUI.EndChangeCheck())
        {

            DisplayCurveChange(prop);

            needToBake = true;

        }

        EditorGUI.indentLevel = 1;

        EditorGUI.BeginChangeCheck();

        position.y += 22;

        textureResolution = EditorGUI.DelayedIntField(position, "Resolution", textureResolution);

        if (EditorGUI.EndChangeCheck())
        {

            DisplayCurveChange(prop);

            needToBake = true;

        }

        position.y += 22;

        EditorGUI.LabelField(position, "Range");

        newWidth = position.width * 0.4f;
        curvePosition.x += curvePosition.width - newWidth;
        curvePosition.width = newWidth;
        curvePosition.y += 44;

        EditorGUI.BeginChangeCheck();

        curveRange = EditorGUI.RectField(curvePosition, curveRange);

        if (EditorGUI.EndChangeCheck())
        {

            SaveRange(prop, editor.target as Material);

        }

        EditorGUI.indentLevel = 0;

        Assembly assembly = Assembly.GetAssembly(typeof(Editor));
        Type type = assembly.GetType("UnityEditor.CurveEditorWindow");
        FieldInfo field = type.GetField("s_SharedCurveEditor", BindingFlags.NonPublic | BindingFlags.Static);

        if (field.GetValue(null) == null)
        {

            if (needToBake)
            {

                needToBake = false;

                BakeCurveTexture(prop, editor.target as Material);

            }

        }

    }

    public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
    {

        return 82;

    }

    private Texture2D GetTextureSubAsset(MaterialProperty prop, string materialPath)
    {

        Texture2D curveBakedTexture = null;

        UnityEngine.Object[] subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(materialPath);

        for (int i = 0; i < subAssets.Length; i++)
        {

            if (subAssets[i].name == prop.name + subAssetTextureEnding)
            {

                curveBakedTexture = subAssets[i] as Texture2D;

            }

        }

        return curveBakedTexture;

    }
    
    private AnimationCurveData GetDataSubAsset(MaterialProperty prop, string materialPath)
    {

        AnimationCurveData curveData = null;

        UnityEngine.Object[] subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(materialPath);

        for (int i = 0; i < subAssets.Length; i++)
        {

            if (subAssets[i].name == prop.name + subAssetDataEnding)
            {

                curveData = subAssets[i] as AnimationCurveData;

            }

        }

        return curveData;

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

    private void SaveRange(MaterialProperty prop, Material material)
    {

        AnimationCurveData curveData = GetDataSubAsset(prop, AssetDatabase.GetAssetPath(material));

        if (curveData != null)
        {

            AssetDatabase.RemoveObjectFromAsset(curveData);

            curveData.animationCurveRange = curveRange;

            AssetDatabase.AddObjectToAsset(curveData, material);

            AssetDatabase.SaveAssets();

        }

    }

    private void DefaultCurve()
    {

        curve = AnimationCurve.Linear(0, 0, 1, 1);

        curveRange = new Rect(0, 0, 1, 1);

        textureResolution = 256;

    }

    private void LoadCurveData(MaterialProperty prop, Material material)
    {

        AnimationCurveData curveData = GetDataSubAsset(prop, AssetDatabase.GetAssetPath(material));

        if (curveData == null)
        {

            DefaultCurve();

        }
        else
        {

            curve = curveData.animationCurve;

            curveRange = curveData.animationCurveRange;

            textureResolution = curveData.resolution;

        }
        
    }

    private void WriteCurveToTexture()
    {

        curveTexture = new Texture2D(textureResolution, 1, TextureFormat.RGBA32, false, true);
        curveTexture.filterMode = FilterMode.Point;
        curveTexture.wrapMode = TextureWrapMode.Clamp;
        curveTexture.anisoLevel = 0;

        float curveSize = curve.keys[curve.keys.Length - 1].time;

        for (int i = 0; i < textureResolution; i++)
        {

            float value = curve.Evaluate(((textureResolution - i) / (float)textureResolution) * curveSize);

            curveTexture.SetPixel(i, 0, FloatToColor(value));

        }

        curveTexture.Apply();

    }
    
    private void DisplayCurveChange(MaterialProperty prop)
    {

        WriteCurveToTexture();

        prop.textureValue = curveTexture;

    }

    private void BakeCurveTexture(MaterialProperty prop, Material material)
    {

        WriteCurveToTexture();

        SaveDataTexture(prop, material);

    }

    private void SaveDataTexture(MaterialProperty prop, Material material)
    {

        string materialPath = AssetDatabase.GetAssetPath(material);

        ClearOldSubAssets(prop, materialPath);

        curveTexture.name = prop.name + subAssetTextureEnding;

        AssetDatabase.AddObjectToAsset(curveTexture, material);

        AnimationCurveData curveData = ScriptableObject.CreateInstance<AnimationCurveData>();

        curveData.name = prop.name + subAssetDataEnding;
        curveData.animationCurve = curve;
        curveData.animationCurveRange = curveRange;
        curveData.resolution = textureResolution;

        AssetDatabase.AddObjectToAsset(curveData, material);

        AssetDatabase.SaveAssets();

        prop.textureValue = GetTextureSubAsset(prop, materialPath);

    }

    private float ColorToFloat(Color color)
    {

        return Mathf.Floor(color.r * 255) +
              (Mathf.Floor(color.g * 255) / 100) +
              (Mathf.Floor(color.b * 255) / 10000) +
              (Mathf.Floor(color.a * 255) / 1000000);

    }

    private Color FloatToColor(float value)
    {

        float f1 = Mathf.Floor(value);
        float f2 = Mathf.Floor((value - f1) * 100) / 100;
        float f3 = Mathf.Floor((value - f1 - f2) * 10000) / 10000;
        float f4 = Mathf.Floor((value - f1 - f2 - f3) * 1000000) / 1000000;

        byte b1 = (byte)Mathf.Floor(f1);
        byte b2 = (byte)Mathf.Floor(f2 * 100);
        byte b3 = (byte)Mathf.Floor(f3 * 10000);
        byte b4 = (byte)Mathf.Floor(f4 * 1000000);

        return new Color(b1 / 255f, b2 / 255f, b3 / 255f, b4 / 255f);

    }

}
