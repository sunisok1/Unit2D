using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class CustomEditorTest : MonoBehaviour
{
    public BehaviorTree bt;
}

[CanEditMultipleObjects, CustomEditor(typeof(CustomEditorTest))]
public class CustomEditorTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Test"))
        {
            var Target = target as CustomEditorTest;
            Target.bt.SendEvent("Res");
        }
    }
}