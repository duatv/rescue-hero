using UnityEngine;
using UnityEditor;

namespace Water2D.Extentions
{
    [CustomEditor(typeof(ColliderFiller))]
    public class ColliderFillerEditor : Editor
    {
        private ColliderFiller _target;

        public override void OnInspectorGUI()
        {
            _target = target as ColliderFiller;
            base.OnInspectorGUI();
            //if (GUILayout.Button("Refresh")) _target.Refresh();
            if (GUILayout.Button("Fill")) {
                _target.Refresh(); _target.Fill();
            };
            if (GUILayout.Button("Clear")) _target.Clear();
        }
    }
}