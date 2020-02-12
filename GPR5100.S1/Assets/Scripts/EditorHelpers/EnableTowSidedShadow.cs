//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//public class EnableTowSidedShadow : MonoBehaviour
//{
//}

//[CustomEditor(typeof(EnableTowSidedShadow))]
//public class EnableTowSidedShadowEditor : Editor
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
//        EnableTowSidedShadow props = (EnableTowSidedShadow)target;
//        MeshRenderer[] renderers = props.transform.GetComponentsInChildren<MeshRenderer>();
//        foreach (MeshRenderer r in renderers)
//        {
//            r.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
//            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
//            r.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
//            r.allowOcclusionWhenDynamic = false;
//        }
//    }
//}
