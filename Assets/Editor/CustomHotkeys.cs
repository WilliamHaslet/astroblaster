using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

public class CustomHotkeys : MonoBehaviour
{

    [MenuItem("GameObject/Custom/Toggle _z")]
    private static void Toggle()
    {

        foreach (Transform t in Selection.transforms)
        {

            Undo.RecordObject(t.gameObject, "CustomHotkeysToggle");

            t.gameObject.SetActive(!t.gameObject.activeInHierarchy);

        }

        MarkDirty();

    }

    [MenuItem("GameObject/Custom/Play _x")]
    private static void Play()
    {

        EditorApplication.isPlaying = !EditorApplication.isPlaying;

    }
    
    [MenuItem("GameObject/Custom/Pause _c")]
    private static void Pause()
    {

        EditorApplication.isPaused = !EditorApplication.isPaused;

    }
    
    [MenuItem("GameObject/Custom/Step _v")]
    private static void Step()
    {

        EditorApplication.Step();

    }
    
    [MenuItem("GameObject/Custom/Clear Console _q")]
    private static void ClearConsole()
    {

        Assembly assembly = Assembly.GetAssembly(typeof(Editor));

        Type type = assembly.GetType("UnityEditor.LogEntries");

        MethodInfo method = type.GetMethod("Clear");

        method.Invoke(new object(), null);

    }
    
    [MenuItem("GameObject/Custom/Reset Transform _3")]
    private static void ResetTransform()
    {

        foreach (Transform t in Selection.transforms)
        {

            Undo.RecordObject(t, "CustomHotkeysResetTransform");

            t.position = Vector3.zero;
            t.rotation = Quaternion.identity;
            t.localScale = Vector3.one;

        }

        MarkDirty();

    }
    
    [MenuItem("GameObject/Custom/Reset Local Transform #3")]
    private static void ResetLocalTransform()
    {

        foreach (Transform t in Selection.transforms)
        {

            Undo.RecordObject(t, "CustomHotkeysResetLocalTransform");

            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;

        }

        MarkDirty();

    }
    
    [MenuItem("GameObject/Custom/Reset Position _4")]
    private static void ResetPosition()
    {

        foreach (Transform t in Selection.transforms)
        {

            Undo.RecordObject(t, "CustomHotkeysResetPosition");

            t.position = Vector3.zero;

        }

        MarkDirty();

    }
    
    [MenuItem("GameObject/Custom/Reset Local Position #4")]
    private static void ResetLocalPosition()
    {

        foreach (Transform t in Selection.transforms)
        {

            Undo.RecordObject(t, "CustomHotkeysResetLocalPosition");

            t.localPosition = Vector3.zero;

        }

        MarkDirty();

    }
    
    [MenuItem("GameObject/Custom/Reset Rotation _5")]
    private static void ResetRotation()
    {

        foreach (Transform t in Selection.transforms)
        {

            Undo.RecordObject(t, "CustomHotkeysResetRotation");

            t.rotation = Quaternion.identity;

        }

        MarkDirty();

    }
    
    [MenuItem("GameObject/Custom/Reset Local Rotation #5")]
    private static void ResetLocalRotation()
    {

        foreach (Transform t in Selection.transforms)
        {

            Undo.RecordObject(t, "CustomHotkeysResetLocalRotation");

            t.localRotation = Quaternion.identity;

        }

        MarkDirty();

    }
    
    [MenuItem("GameObject/Custom/Reset Scale _6")]
    private static void ResetScale()
    {

        foreach (Transform t in Selection.transforms)
        {

            Undo.RecordObject(t, "CustomHotkeysResetScale");

            t.localScale = Vector3.one;

        }

        MarkDirty();

    }

    private static void MarkDirty()
    {

        if (!EditorApplication.isPlaying)
        {

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

        }

    }
    
}
