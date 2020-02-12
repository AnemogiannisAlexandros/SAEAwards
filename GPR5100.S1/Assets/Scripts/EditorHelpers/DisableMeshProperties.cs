//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//public class DisableMeshProperties : MonoBehaviour
//{
//}

//[CustomEditor(typeof(DisableMeshProperties))]
//public class DisableMeshPropertiesEditor : Editor
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
//        DisableMeshProperties props = (DisableMeshProperties)target;
//        MeshRenderer[] renderers = props.transform.GetComponentsInChildren<MeshRenderer>();
//        foreach (MeshRenderer r in renderers)
//        {
//            r.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
//            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
//            r.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
//            r.allowOcclusionWhenDynamic = false;
//        }
//    }
//}
