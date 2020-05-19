using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeNode : MonoBehaviour
{
    public Rigidbody2D _rig;
    [SerializeField]public HingeJoint2D hingeJoin2D;
    [SerializeField] public int ropeIndex;
}
