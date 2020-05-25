using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class RopeManager : MonoBehaviour
{
    public static RopeManager Instance;
    [SerializeField] public RopeNode[] ropeNode;

    private void Awake()
    {
        Instance = this;
    }
    public void UnUseRope(RopeNode _ropeNote)
    {
        _ropeNote.hingeJoin2D.enabled = false;
        for (int i = 0; i < _ropeNote.gRopeParent.transform.childCount; i++){
            _ropeNote.gRopeParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void UnUseRope(int _ropeIdex) {
        Debug.LogError("UnUseRope");
        StartCoroutine(HideAllRope(_ropeIdex));
    }
    IEnumerator HideAllRope(int _index)
    {
        yield return new WaitForSeconds(0.5f);
        foreach (RopeNode _rn in ropeNode) {
            if (_rn.ropeIndex == _index)
            {
                _rn.hingeJoin2D.enabled = false;
                _rn.gameObject.SetActive(false);
            }
        }
    }
    void Start()
    {
        if (GameManager.Instance != null) {
            GameManager.Instance.canUseTrail = true;
        }
    }
}
