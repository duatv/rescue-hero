using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public Text txtVersion;
    public GameObject gQualityOn, gQualityOff, gSoundOn, gSoundOff, gMusicOn, gMusicOff, gVibrateOn, gVibrateOff;
    private void OnEnable()
    {
        anim.Play("PopupAnim");
        ChangeSprite();
    }
    public Animator anim;
    private void OnValidate()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
    }
    private void Start()
    {
        txtVersion.text = "Version " + Application.version;
    }
    private void ChangeSprite() {
        ChangeQualitySprite();
        ChangeSoundSprite();
        ChangeMusicSprite();
        ChangeVibrateSprite();
    }

    public void ChangeQuality() {
        Utils.useMediumImage = !Utils.useMediumImage;
        ChangeSprite();
        Utils.SaveImageSeting();
    }
    public void ChangeSound() {
        Utils.isSoundOn = !Utils.isSoundOn;
        ChangeSoundSprite();
        Utils.ChangeSound();


        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.CheckSound();
        }
    }
    public void ChangeMusic() {
        Utils.isMusicOn = !Utils.isMusicOn;
        ChangeMusicSprite();
        Utils.ChangeMusic();

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.CheckBGMusic();
        }
    }
    public void ChangeVibrate() {
        Utils.isVibrateOn = !Utils.isVibrateOn;
        ChangeVibrateSprite();
        Utils.ChangeVibrate();
    }
    public void OnClose() {

        anim.Play("PopUpAnimClose");
        //   gameObject.SetActive(false);
    }
    public void OnRestorePurchase() {
    }


    private void ChangeVibrateSprite() {
        gVibrateOff.SetActive(!Utils.isVibrateOn);
        gVibrateOn.SetActive(Utils.isVibrateOn);
    }
    private void ChangeMusicSprite() {
        gMusicOff.SetActive(!Utils.isMusicOn);
        gMusicOn.SetActive(Utils.isMusicOn);
    }
    private void ChangeSoundSprite() {
        gSoundOff.SetActive(!Utils.isSoundOn);
        gSoundOn.SetActive(Utils.isSoundOn);
    }
    private void ChangeQualitySprite() {
        gQualityOff.SetActive(!Utils.useMediumImage);
        gQualityOn.SetActive(Utils.useMediumImage);
    }
}
