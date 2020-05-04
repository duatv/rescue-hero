/*
 * A script for draw gizmos from ColliderFiller.
 * 
 * Author: andrewdiden@mail.ru
 */
#if UNITY_EDITOR

using UnityEngine;

namespace Water2D.Extentions
{
    [RequireComponent(typeof(ColliderFiller))]
    public class ColliderFillerGizmos : MonoBehaviour
    {
        [Header("Main")]
        public ColliderFiller colliderFiller;

        [Header("Gizmos")]
        [SerializeField] private bool showGizmosAlways = false;
        [SerializeField] private bool showGizmos = true;
        [SerializeField] private bool showBounds = true;
        [SerializeField] private bool showInside = true;
        [SerializeField] private bool showOutside = false;
        [SerializeField] private bool showPolygonPoints = true;
        [SerializeField] private bool showPolygonPointsText = false;

        #region UnityMethods

        private void OnValidate()
        {
            if (colliderFiller == null) colliderFiller = GetComponent<ColliderFiller>();
        }

        private void OnDrawGizmosSelected()
        {
            if (!showGizmosAlways) DrawGizmos(colliderFiller.collider);
        }

        private void OnDrawGizmos()
        {
            if (showGizmosAlways) DrawGizmos(colliderFiller.collider);
        }

        #endregion UnityMethods

        #region Gizmos

        private void DrawGizmos(Collider2D collider)
        {
            if (colliderFiller == null) return;

            if (showGizmos == false) return;

            Vector3[] pointsInside = colliderFiller.PointsInside;
            Vector3[] pointsOutside = colliderFiller.PointsOutside;

            if (showBounds && collider)
                DrawWithColor(Color.yellow, () => Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size));
            if (showInside && pointsInside != null && pointsInside.Length > 0)
                DrawPoints(Color.green, pointsInside, true, false);
            if (showOutside && pointsOutside != null && pointsOutside.Length > 0)
                DrawPoints(Color.red, pointsOutside, true, false);

            if (collider != null && (showPolygonPoints || showPolygonPointsText))
            {
                Vector2[] points = null;
                if (collider is BoxCollider2D)
                    points = ColliderFiller.GetBoxPoints(collider as BoxCollider2D);
                else if (collider is CircleCollider2D)
                    points = ColliderFiller.GetCirclePoints(collider as CircleCollider2D, colliderFiller.circleDetailPoints);
                else if (collider is PolygonCollider2D)
                    points = ColliderFiller.GetPolygonPoints(collider as PolygonCollider2D);
                if (points != null)
                    DrawPoints(Color.blue, points, showPolygonPoints, showPolygonPointsText);
            }
        }

        private void DrawPoints(Color color, Vector2[] points, bool drawPoint, bool drawText)
        {
            Vector3[] newPoints = System.Array.ConvertAll<Vector2, Vector3>(points, V2ToV3);
            DrawPoints(color, newPoints, drawPoint, drawText);
        }

        private void DrawPoints(Color color, Vector3[] points, bool drawPoint, bool drawText)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (drawPoint) DrawWithColor(color, () => Gizmos.DrawWireSphere(points[i], colliderFiller.Radius));
                if (drawText) UnityEditor.Handles.Label(points[i], i.ToString());
            }
        }

        private void DrawWithColor(Color color, System.Action action)
        {
            Color colorOld = Gizmos.color;
            Gizmos.color = color;
            action?.Invoke();
            Gizmos.color = colorOld;
        }

        #endregion Gizmos

        #region Helps

        public static Vector3 V2ToV3(Vector2 v2) { return new Vector3(v2.x, v2.y, 0); }

        #endregion Helps

    }
}
#endif