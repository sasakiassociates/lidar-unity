// Pcx - Point cloud importer & renderer for Unity
// https://github.com/keijiro/Pcx

using UnityEngine;
using UnityEditor;

namespace Pcx
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(PointCloudRenderer))]
    public class PointCloudRendererInspector : Editor
    {
        SerializedProperty _sourceData;
        SerializedProperty _pointTint;
        SerializedProperty _pointSize;
        SerializedProperty _pointShader;
        SerializedProperty _diskShader;

        void OnEnable()
        {
            _sourceData = serializedObject.FindProperty("_sourceData");
            _pointTint = serializedObject.FindProperty("_pointTint");
            _pointSize = serializedObject.FindProperty("_pointSize");
            _pointShader = serializedObject.FindProperty("_pointShader");
            _diskShader = serializedObject.FindProperty("_diskShader");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_sourceData);
            EditorGUILayout.PropertyField(_pointTint);
            EditorGUILayout.PropertyField(_pointSize);
            EditorGUILayout.PropertyField(_pointShader);
            EditorGUILayout.PropertyField(_diskShader);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
