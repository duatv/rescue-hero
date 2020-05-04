/*
 * A script for filling standard trigger colliders with water.
 * 
 * Author: andrewdiden@mail.ru
 */

using System.Collections.Generic;
using UnityEngine;

namespace Water2D
{
    [ExecuteInEditMode]
    public class ColliderFiller : MonoBehaviour
    {
        [Header("Main")]
        public new Collider2D collider;
        public Water2D_Spawner water2D_Spawner;

        [HideInInspector]
        [Tooltip("Water2DSpawner overlaps radius multiplying ColliderSize and Size")]
        [Range(0.05f, 1)]
        public float radius = 1;

       [HideInInspector][Range(3, 100)]
        public int circleDetailPoints = 10;

        public bool autoRefresh = false;

        private Vector3[] pointsInside;
        private Vector3[] pointsOutside;

        public Vector3[] PointsInside { get { return pointsInside; } }
        public Vector3[] PointsOutside { get { return pointsOutside; } }

        public int InsidePointsCount { get { return pointsInside != null ? pointsInside.Length : 0; } }
        public int OutsidePointsCount { get { return pointsOutside != null ? pointsOutside.Length : 0; } }
        
        public float Radius { get { return water2D_Spawner != null ? water2D_Spawner.ColliderSize * water2D_Spawner.size : radius; } }
        [HideInInspector]public bool Masked;

        #region UnityMethods

        Collider2D _lastCollider;

        private void Update()
        {
            //if (autoRefresh)
            // Refresh();

            if (_lastCollider != collider)
            {
                _lastCollider = collider;

                Refresh();
                Fill();
            }
        }

        #endregion UnityMethods

        #region Main

        [ContextMenu("Refresh")]
        public void Refresh()
        {
            if (collider == null) return;

            Vector3[] pointsInside;
            Vector3[] pointsOutside;
            float zPos = collider.transform.position.z;
            float radius = this.radius;
            if (water2D_Spawner) radius = water2D_Spawner.ColliderSize * water2D_Spawner.size;
            GetPointsInCollider(collider, radius, out pointsInside, out pointsOutside, zPos, Masked);
            this.pointsInside = pointsInside;
            this.pointsOutside = pointsOutside;
            water2D_Spawner.DropsUsed = PointsInside.Length;
        }

        [ContextMenu("Fill")]
        public void Fill()
        {
            if (water2D_Spawner == null)
            {
                Debug.LogError("Water2D Spawner is NULL!");
                return;
            }

            //water2D_Spawner.DropCount = Mathf.Max(pointsInside.Length, water2D_Spawner.DropCount);
            
            water2D_Spawner.SetupParticles();

            MetaballParticleClass currentMetaball;
            for (int i = 0; i < pointsInside.Length; i++)
            {

                if (i >= water2D_Spawner.WaterDropsObjects.Length)
                {
                    Debug.LogError("Points in the shape exceed the number of drops. You should increment to " + pointsInside.Length + " drops in the spawner");
                    return;

                }
                water2D_Spawner.WaterDropsObjects[i].transform.position = pointsInside[i];
                currentMetaball = water2D_Spawner.WaterDropsObjects[i].GetComponent<MetaballParticleClass>();
                if (currentMetaball)
                {
                    currentMetaball.Active = true;
                }
            }
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            if (water2D_Spawner == null)
            {
                Debug.LogError("Water2D Spawner is NULL!");
                return;
            }
            MetaballParticleClass currentMetaball;
            for (int i = 0; i < pointsInside.Length; i++)
            {
                water2D_Spawner.WaterDropsObjects[i].transform.position = pointsInside[i];
                currentMetaball = water2D_Spawner.WaterDropsObjects[i].GetComponent<MetaballParticleClass>();
                if (currentMetaball)
                {
                    currentMetaball.Active = true;
                }
            }
            water2D_Spawner.Restore();
        }

        #endregion Main

        #region Points & Colliders

        /// <summary>
        /// Get all point at Collider bounds!
        /// </summary>
        /// <param name="collider">the collider we work with</param>
        /// <param name="r">radius of circle</param>
        /// <param name="_pointsInside">result: inside points</param>
        /// <param name="_pointsOutside">result: outside points</param>
        /// <param name="zPos">z position, if needed, default = 0</param>
        private void GetPointsInCollider(Collider2D collider, float r, out Vector3[] _pointsInside, out Vector3[] _pointsOutside, float zPos = 0, bool masked = false)
        {
            if (collider == null)
            {
                Debug.LogError("Collider is NULL!");
                _pointsInside = null;
                _pointsOutside = null;
                return;
            }

            List<Vector3> pointsInside = new List<Vector3>();
            List<Vector3> pointsOutside = new List<Vector3>();
            
            bool isBox = collider is BoxCollider2D;
            bool isCircle = collider is CircleCollider2D;
            bool isPolygon = collider is PolygonCollider2D;
            
            Vector2[] polygonPoints = null;

            if (isBox)
                polygonPoints = GetBoxPoints(collider as BoxCollider2D);
            else if (isCircle)
                polygonPoints = GetCirclePoints(collider as CircleCollider2D, circleDetailPoints);
            else if (isPolygon)
                polygonPoints = GetPolygonPoints(collider as PolygonCollider2D);

            System.Action<Vector3> action = (Vector3 currentPos) =>
            {
                if (isBox || isCircle || isPolygon)
                {
                    bool inPolygon = IsPointInPolygon(currentPos, polygonPoints, masked);
                    if (inPolygon)
                        pointsInside.Add(currentPos);
                    else
                        pointsOutside.Add(currentPos);
                }
            };

            FillByGrid(collider, r, action, zPos);

            _pointsInside = pointsInside.ToArray();
            _pointsOutside = pointsOutside.ToArray();
        }

        /// <summary>
        /// Get points by grid and invoke action on every point
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="r"></param>
        /// <param name="action"></param>
        /// <param name="zPos"></param>
        private void FillByGrid(Collider2D collider, float r, System.Action<Vector3> action, float zPos = 0)
        {
            Bounds bounds = collider.bounds;
            float d = r * 2;
            int countX = (int)(bounds.size.x / d);
            int countY = (int)(bounds.size.y / d);
            Vector3 halfSize = bounds.size / 2;

            Vector3 currentPos = Vector3.zero;
            for (int y = 0; y < countY; y++)
            {
                for (int x = 0; x < countX; x++)
                {
                    currentPos = bounds.center + new Vector3(x * d + r, y * d + r, zPos) - halfSize;
                    action?.Invoke(currentPos);
                }
            }
        }

        /// <summary>
        /// Get points from BoxCollider2D
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static Vector2[] GetBoxPoints(BoxCollider2D box)
        {
            return new Vector2[]
                {
                    box.transform.TransformPoint(new Vector3(-box.size.x, box.size.y) * 0.5f),
                    box.transform.TransformPoint(new Vector3(box.size.x, box.size.y) * 0.5f),
                    box.transform.TransformPoint(new Vector3(box.size.x, -box.size.y) * 0.5f),
                    box.transform.TransformPoint(new Vector3(-box.size.x, -box.size.y) * 0.5f),
                };
        }

        /// <summary>
        /// Get points from CircleCollider2D
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static Vector2[] GetCirclePoints(CircleCollider2D circle, int count)
        {
            Vector2[] result = new Vector2[count];
            for (int i = 0; i < count; i++)
            {
                float angle = i * Mathf.PI * 2 / count;
                result[i] = circle.transform.TransformPoint(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * circle.radius);
            }
            return result;
        }

        /// <summary>
        /// Get points from PolygonCollider2D
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static Vector2[] GetPolygonPoints(PolygonCollider2D polygon)
        {
            int count = polygon.points.Length;
            Vector2[] result = new Vector2[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = polygon.transform.TransformPoint(polygon.points[i]);
            }
            return result;
        }

        /// <summary>
        /// From: https://codereview.stackexchange.com/questions/108857/point-inside-polygon-check
        /// </summary>
        public static bool IsPointInPolygon(Vector2 point, Vector2[] polygon, bool masked = false)
        {
            int polygonLength = polygon.Length, i = 0;
            bool inside = masked;
            float pointX = point.x, pointY = point.y;
            float startX, startY, endX, endY;
            Vector2 endPoint = polygon[polygonLength - 1];
            endX = endPoint.x;
            endY = endPoint.y;
            while (i < polygonLength)
            {
                startX = endX; startY = endY;
                endPoint = polygon[i++];
                endX = endPoint.x; endY = endPoint.y;
                inside ^= (endY > pointY ^ startY > pointY) && ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
            }
            return inside;
        }

        #endregion Points & Colliders
    }
}