using UnityEngine;
using UnityEditor;
//using DynamicLight2D;
using Apptouch;

namespace Water2D
{
    [InitializeOnLoad]
    public class Water2DHierarchyIcon : MonoBehaviour
    {
        static readonly Texture2D _icon;

        static Water2DHierarchyIcon()
        {
            _icon = AssetDatabase.LoadAssetAtPath(Apptouch.EditorUtils.getMainRelativepath() + "Misc/hierarchyIcon_mini.png", typeof(Texture2D)) as Texture2D;

            if (_icon == null)
            {
                return;
            }

            EditorApplication.hierarchyWindowItemOnGUI += hierarchyItemOnGUI;
            EditorApplication.RepaintHierarchyWindow();
        }

        static void hierarchyItemOnGUI(int instanceID, Rect selectionRect)
        {
            GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (go == null)
            {
                return;
            }

            if (_icon != null && go.GetComponent<Water2D_Spawner>() != null)
            {
                Rect r = new Rect(selectionRect);
                r.x = r.width - 5;

                GUI.Label(r, _icon);
                return;
            }


        }
    }
}
