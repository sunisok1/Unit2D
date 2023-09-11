using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CustomEditorTest : MonoBehaviour
{

}

[CanEditMultipleObjects, CustomEditor(typeof(CustomEditorTest))]
public class CustomEditorTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Test"))
        {

        }
    }
}