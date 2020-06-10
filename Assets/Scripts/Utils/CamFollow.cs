using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CamFollow : MonoBehaviour
{
    public GameObject objectToFollow/*,group*/;
    public Camera _myCam;
    public float speed = 2.0f;
    public float camSize = 2.5f;
    public bool beginFollow;

    Vector3 position;
    float interpolation;

    public Image BG;

    public GameObject PanelWin;

    void Update()
    {
        if (beginFollow)
        {
            //if (Vector3.Distance(transform.position, objectToFollow.transform.position) == 10)
            //{
            //    beginFollow = false;
            //}
            //else
            //{
            interpolation = speed * Time.deltaTime;
            _myCam.orthographicSize = Mathf.Lerp(_myCam.orthographicSize, camSize, interpolation);
            //var tempColor = BG.color;
            //tempColor.a = Mathf.Lerp(tempColor.a, 1, interpolation);
            //BG.color = tempColor;

            var scale = PanelWin.transform.localScale;
            scale.x = scale.y = Mathf.Lerp(scale.x, 1, interpolation * 3);
            PanelWin.transform.localScale = scale;

            position = this.transform.position;
            position.y = Mathf.Lerp(this.transform.position.y, objectToFollow.transform.position.y, interpolation);
            position.x = Mathf.Lerp(this.transform.position.x, objectToFollow.transform.position.x, interpolation);
            this.transform.position = position;
            // Debug.LogError(Vector2.Distance(transform.position, objectToFollow.transform.position));
            if (GameManager.Instance.bouderCoinFly.activeSelf || GameManager.Instance.gPanelWin.sprite == GameManager.Instance.loseSp)
                return;
            if (PanelWin.transform.localScale.x <= 1.1f)
            {
                GameManager.Instance.bouderCoinFly.SetActive(true);
            }
        }
    }

}
