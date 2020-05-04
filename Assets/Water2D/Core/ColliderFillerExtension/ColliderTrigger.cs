/*
 * A script to check the filling with water using standard triggered colliders.
 * 
 * Author: andrewdiden@mail.ru
 */

using System.Collections.Generic;
using UnityEngine;

namespace Water2D.Extentions
{
    public class ColliderTrigger : MonoBehaviour
    {
        [Header("Main")]
        public ColliderFiller colliderFiller;
        public List<string> tags = new List<string>() { "Metaball_liquid" };
        public bool check = false;

        [Header("Events")]
        public UnityEngine.Events.UnityEvent onTrue;
        public UnityEngine.Events.UnityEvent onFalse;

        private List<Transform> inArea;

        private void Start()
        {
            if (inArea == null)
                inArea = new List<Transform>();
            Apply(false);
        }

        private void Apply(bool active)
        {
            if (active)
                onTrue?.Invoke();
            else
                onFalse?.Invoke();
        }

        private bool CheckFilled()
        {
            if (inArea == null)
                inArea = new List<Transform>();
            if (inArea != null && colliderFiller != null)
                return inArea.Count >= colliderFiller.InsidePointsCount;
            return false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (check == false) return;
            // Check tag
            if (tags.Contains(collision.tag) == false) return;
            // Init list
            if (inArea == null)
                inArea = new List<Transform>();
            // Add obj to list
            if (inArea.Contains(collision.transform) == false)
            {
                inArea.Add(collision.transform);
                Apply(CheckFilled());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (check == false) return;
            // Check tag
            if (tags.Contains(collision.tag) == false) return;
            // Init list
            if (inArea == null)
                inArea = new List<Transform>();
            // Remove obj from list
            if (inArea.Contains(collision.transform))
            {
                inArea.Remove(collision.transform);
                Apply(CheckFilled());
            }
        }
    }
}