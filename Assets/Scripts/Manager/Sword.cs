using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public LayerMask lmMapObject;
    public Rigidbody2D rig2d;

    private RaycastHit2D hitDown;

 //   private PlayerManager _pPLayer;
    private HostageManager _hHostage;
    private EnemyBase _eEnemy;
    Vector3 _vStartHitDown, _vEndHitDown;
    private void HitDownMapObject()
    {
        _vStartHitDown = new Vector3(transform.localPosition.x, transform.localPosition.y - 2.35f, transform.localPosition.z);
        _vEndHitDown = new Vector3(_vStartHitDown.x, _vStartHitDown.y - 0.25f, _vStartHitDown.z);
        hitDown = Physics2D.Linecast(_vStartHitDown, _vEndHitDown, lmMapObject);

        Debug.DrawLine(_vStartHitDown, _vEndHitDown, Color.red);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public bool IsCanKilling() {
        return hitDown.collider != null ? false : true;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        HitDownMapObject();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerManager.Instance.pState == PlayerManager.P_STATE.DIE)
            return;
        if (/*collision.gameObject.GetComponent<PlayerManager>() != null*/collision.tag == "BodyPlayer") {
         //   _pPLayer = collision.gameObject.GetComponent<PlayerManager>();
            //if (IsCanKilling()) {
            //    if(!/*_pPLayer*/PlayerManager.Instance.isTakeSword)
            //        /*_pPLayer*/
            //        PlayerManager.Instance.OnPlayerDie(true);
            //}
            //else
            //{
                rig2d.bodyType = RigidbodyType2D.Kinematic;

                /*_pPLayer*/PlayerManager.Instance.OnTakeSword(transform);
            //}
        }
        if (collision.gameObject.GetComponent<HostageManager>() != null)
        {
            _hHostage = collision.gameObject.GetComponent<HostageManager>();
            if (IsCanKilling())
            {
                if(!PlayerManager.Instance.isTakeSword)
                    _hHostage.OnDie_(false);
            }
        }
        if (collision.gameObject.GetComponent<EnemyBase>() != null)
        {
            _eEnemy = collision.gameObject.GetComponent<EnemyBase>();
            if (IsCanKilling())
            {
                _eEnemy.OnDie_();
            }
        }
    }
}