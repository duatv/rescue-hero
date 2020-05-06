using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private Vector3 mousePos;
    
    void Update()
    {
        mousePos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
            RaycastHit2D[] hit = Physics2D.RaycastAll(screenPos, Vector2.zero, 1000);
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider.gameObject.tag.Contains(Utils.TAG_STICKBARRIE))
                {
                    hit[i].collider.gameObject.GetComponent<StickBarrier>().beginMove = true;
                }
            }
        }
    }
}
