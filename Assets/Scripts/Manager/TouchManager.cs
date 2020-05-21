using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    #region Trail Eff
    public Color trailColor = new Color(1, 0, 0.38f);
    public float distanceFromCamera = 5;
    public float startWidth = 0.01f;
    public float endWidth = 0f;
    public float trailTime = 0.14f;
    Transform trailTransform;
    public Camera thisCamera;
    #endregion

    private Vector3 mousePos;

    void Start()
    {
        GameObject trailObj = new GameObject("Mouse Trail");
        trailTransform = trailObj.transform;
        TrailRenderer trail = trailObj.AddComponent<TrailRenderer>();
        trail.time = -1f;
        MoveTrailToCursor(Input.mousePosition);
        trail.time = trailTime;
        trail.startWidth = startWidth;
        trail.endWidth = endWidth;
        trail.numCapVertices = 2;
        trail.sharedMaterial = new Material(Shader.Find("Unlit/Color"));
        trail.sharedMaterial.color = trailColor;
        trailTransform.gameObject.SetActive(false);
    }
    
    void Update()
    {
        mousePos = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
            RaycastHit2D[] hit = Physics2D.RaycastAll(screenPos, Vector2.zero, 1000);
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider.gameObject.tag.Contains(Utils.TAG_STICKBARRIE))
                {
                    if (!hit[i].collider.gameObject.GetComponent<StickBarrier>().beginMove)
                    {
                        if (SoundManager.Instance != null)
                        {
                            SoundManager.Instance.PlaySound(SoundManager.Instance.acMoveStick);
                        }
                        Debug.LogError("1111");
                    }
                    
                    hit[i].collider.gameObject.GetComponent<StickBarrier>().beginMove = true;
                }
                if (hit[i].collider.gameObject.GetComponent<RopeNode>() != null) {
                    var ropeNode = hit[i].collider.gameObject.GetComponent<RopeNode>();
                    if (ropeNode)
                    {
                        ropeNode.hingeJoin2D.enabled = false;
                        RopeManager.Instance.UnUseRope(ropeNode.ropeIndex);
                    }
                }
            }


            if (GameManager.Instance.canUseTrail) {
                trailTransform.gameObject.SetActive(true);
                MoveTrailToCursor(mousePos);
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            if (GameManager.Instance.canUseTrail)
                trailTransform.gameObject.SetActive(false);
        }
    }
    void MoveTrailToCursor(Vector3 screenPosition)
    {
        trailTransform.position = thisCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, distanceFromCamera));
    }
}
