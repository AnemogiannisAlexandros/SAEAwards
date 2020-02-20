//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

///
/// LightMap Size Rescale On All Children Objects
///

//public class RescaleLightMap : MonoBehaviour
//{
//}

//[CustomEditor(typeof(RescaleLightMap))]
//public class RescaleLightMapEditor : Editor
//{

//    public override void OnInspectorGUI()
//    {
//        if (GUILayout.Button("Generate Mesh Settings"))
//        {
//            GenerateMeshSettings();
//        }
//    }
//    public void GenerateMeshSettings()
//    {
//        RescaleLightMap props = (RescaleLightMap)target;

//        Renderer[] renderers = props.transform.GetComponentsInChildren<Renderer>();
//        foreach (Renderer r in renderers)
//        {
//            SerializedObject so = new SerializedObject(r);
//            so.FindProperty("m_ScaleInLightmap").floatValue = 0.2f;
//            so.ApplyModifiedProperties();
//        }
//    }
//}
