using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public LayerMask lmMapObject;
    public Rigidbody2D rig2d;
    private RaycastHit2D hitDown;

    Vector3 _vStartHitDown, _vEndHitDown;
    private void HitDownMapObject()
    {
        _vStartHitDown = new Vector3(transform.localPosition.x, transform.localPosition.y - 2.0f, transform.localPosition.z);
        _vEndHitDown = new Vector3(_vStartHitDown.x, _vStartHitDown.y - 0.2f, _vStartHitDown.z);
        hitDown = Physics2D.Linecast(_vStartHitDown, _vEndHitDown, lmMapObject);

        Debug.DrawLine(_vStartHitDown, _vEndHitDown, Color.red);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HitDownMapObject();
    }
}