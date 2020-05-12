using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeManager : MonoBehaviour
{
    public static RopeManager Instance;
    public RopeNode[] ropeNode;

    private void Awake()
    {
        Instance = this;
    }
    public void UnUseRope() {
        //foreach (RopeNode _rn in ropeNode) {
        //    _rn.hingeJoin2D.enabled = false;
        //    _rn.gameObject.SetActive(false);
        //}
        StartCoroutine(HideAllRope());
    }
    IEnumerator HideAllRope()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (RopeNode _rn in ropeNode) {
            _rn.hingeJoin2D.enabled = false;
            _rn.gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
