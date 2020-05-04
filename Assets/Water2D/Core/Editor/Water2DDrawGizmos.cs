namespace Water2D
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;

    public class DynamicLightDrawGizmo
    {

        [DrawGizmo(GizmoType.NonSelected | GizmoType.NotInSelectionHierarchy)]
        private static void drawGizmoNow(Water2D_Spawner w, GizmoType gizmoType)
        {

            PostProcessMethods.checkIconTexture();

            Gizmos.DrawIcon(w.transform.position, "w2d_gizmos.png", false);

        }

    }
}
