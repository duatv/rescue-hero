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
    public LayerMask lmTouch;
    Transform trailTransform;
    public Camera thisCamera;
    #endregion
    TrailRenderer trail;
    private Vector3 mousePos;

    void Start()
    {
        GameObject trailObj = new GameObject("Mouse Trail");
        trailTransform = trailObj.transform;
         trail = trailObj.AddComponent<TrailRenderer>();
        trail.time = -1f;
        MoveTrailToCursor(Input.mousePosition);
        trail.time = trailTime;
        trail.startWidth = startWidth;
        trail.endWidth = endWidth;
        trail.numCapVertices = 2;
        trail.sharedMaterial = new Material(Shader.Find("Unlit/Color"));
        trail.sharedMaterial.color = trailColor;
        trail.Clear();  
        trailTransform.gameObject.SetActive(false);

    }

    void Update()
    {
        if (GameManager.Instance.gameState != GameManager.GAMESTATE.PLAYING)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            trail.Clear();
            cutOn = true;
            currentMouse = oldMouse = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Ray2D ray = new Ray2D(oldMouse, currentMouse - oldMouse);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, (currentMouse - oldMouse).magnitude, lmTouch);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == Utils.TAG_STICKBARRIE)
                {

                        StickBarrier sb = hit.collider.gameObject.GetComponent<StickBarrier>();
                        if (!sb.beginMove)
                        {
                            if (SoundManager.Instance != null)
                            {
                                if (sb._moveType == StickBarrier.MOVETYPE.FREE)
                                {
                                    SoundManager.Instance.PlaySound(SoundManager.Instance.acMoveStickClick);
                                    if (GameManager.Instance.mapLevel.lstAllStick.Contains(sb))
                                        GameManager.Instance.mapLevel.lstAllStick.Remove(sb);
                                }
                                else
                                    SoundManager.Instance.PlaySound(SoundManager.Instance.acMoveStick);
                            }


                        sb.beginMove = true;
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (GameManager.Instance.canUseTrail)
            {
                trail.Clear();
                trailTransform.gameObject.SetActive(false);
            }
            cutOn = false;

        }
        if (cutOn)
        {
            currentMouse = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(oldMouse, currentMouse - oldMouse);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, (currentMouse - oldMouse).magnitude, lmTouch);
            oldMouse = currentMouse;
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Rope")
                {
                    var ropeNode = hit.collider.gameObject.GetComponent<RopeNode>();
                    if (ropeNode)
                    {
                        ropeNode.hingeJoin2D.enabled = false;
                        if (SoundManager.Instance != null)
                        {
                            SoundManager.Instance.PlaySound(SoundManager.Instance.acCutRope);
                        }
                        RopeManager.Instance.UnUseRope(ropeNode);
                    }
                }
                else if (hit.collider.gameObject.tag == Utils.TAG_STICKBARRIE)
                {
                    StickBarrier sb = hit.collider.gameObject.GetComponent<StickBarrier>();
                    if (!sb.beginMove)
                    {
                        if (SoundManager.Instance != null)
                        {
                            if (sb._moveType == StickBarrier.MOVETYPE.FREE)
                            {
                                SoundManager.Instance.PlaySound(SoundManager.Instance.acMoveStickClick);
                                if (GameManager.Instance.mapLevel.lstAllStick.Contains(sb))
                                    GameManager.Instance.mapLevel.lstAllStick.Remove(sb);
                            }
                            else
                                SoundManager.Instance.PlaySound(SoundManager.Instance.acMoveStick);
                        }
                    }

                    sb.beginMove = true;
                }
            }

            if (GameManager.Instance.canUseTrail)
            {
                trailTransform.gameObject.SetActive(true);
                MoveTrailToCursor(Input.mousePosition);
            }
        }

    }
    void MoveTrailToCursor(Vector3 screenPosition)
    {
        trailTransform.position = thisCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, distanceFromCamera));
    }



    private bool cutOn;
    private Vector3 oldMouse;
    private Vector3 currentMouse;


}
