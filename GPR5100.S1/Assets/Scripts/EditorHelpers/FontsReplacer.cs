//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using UnityEngine.UI;

//public class FontsReplacer : MonoBehaviour
//{
//}
//[CustomEditor(typeof(FontsReplacer))]
//public class FontsReplacerEditor : Editor
//{
//    public Font font;

//    public override void OnInspectorGUI()
//    {
//        font = (Font)EditorGUILayout.ObjectField(font, typeof(Font));
//        if (GUILayout.Button("Replace Font"))
//        {
//            ReplaceFonts();
//        }
//    }
//    public void ReplaceFonts()
//    {
//        FontsReplacer props = (FontsReplacer)target;
//        Text[] texts = props.transform.GetComponentsInChildren<Text>();
//        foreach (Text t in texts)
//        {
//            t.font = font;
//        }
//    }
//}
