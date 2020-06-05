using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeNode : MonoBehaviour
{
    public Rigidbody2D _rig;
    [SerializeField]public HingeJoint2D hingeJoin2D;
    [SerializeField] public int ropeIndex;
    public GameObject gRopeParent;

    //public void OnMouseDown()
    //{
    //    Debug.LogError("====wtf====");
    //    hingeJoin2D.enabled = false;
    //    if (SoundManager.Instance != null)
    //    {
    //        SoundManager.Instance.PlaySound(SoundManager.Instance.acCutRope);
    //    }
    //    RopeManager.Instance.UnUseRope(this);
    //}
    //public void OnMouseOver()
    //{
    //    Debug.LogError("====wtf====");
    //    hingeJoin2D.enabled = false;
    //    if (SoundManager.Instance != null)
    //    {
    //        SoundManager.Instance.PlaySound(SoundManager.Instance.acCutRope);
    //    }
    //    RopeManager.Instance.UnUseRope(this);

    //}
}
