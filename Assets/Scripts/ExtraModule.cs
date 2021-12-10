using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ExtraModule : MonoBehaviour {
    public Image slider;
    public float duration = 2f;
    public Text moduleText;

    [Header("Modules")]
    public Module speedModule;
    public Module reflectorModule;
    public Module ballsModule;

    [HideInInspector]
    public Module rewardedModule;

    private void OnEnable() {
        switch(Random.Range(0,2)) {
            case 0:
                this.rewardedModule = this.speedModule;
                this.moduleText.text = LocaleHelper.GetText("speed");
                break;
            case 1:
                this.rewardedModule = this.reflectorModule;
                this.moduleText.text = LocaleHelper.GetText("reflector");
                break;
            case 2:
                this.rewardedModule = this.ballsModule;
                this.moduleText.text = LocaleHelper.GetText("balls");
                break;
            default:
                break;
        }
        StartCoroutine(Timer(this.duration));
    }

    IEnumerator Timer(float expireDuration) {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        while(expireDuration > 0f) {
            this.slider.fillAmount = expireDuration / this.duration;
            expireDuration -= 0.05f;
            yield return wait;
        }
        this.gameObject.SetActive(false);
        GameManager.instance.anyActiveModuleExists = false;
    }

    public void TakeExtraModule() {
        this.gameObject.SetActive(false);
        Time.timeScale = 0f;
        GameManager.instance.adService.DisplayRewardedAd(RewardType.Module);
    }
}
