using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseColliderController : MonoBehaviour
{
	CircleCollider2D col;
    void Start()
    {
		col = GetComponent<CircleCollider2D> ();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButton (0)) {
			col.enabled = true;
			col.transform.position = (Vector2) Camera.main.ScreenToWorldPoint (Input.mousePosition);
		} else {
			col.enabled = false;
		}
    }
}
