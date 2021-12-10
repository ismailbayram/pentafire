using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public enum RewardType {
    Revive,
    Module
}

public class AdService : MonoBehaviour, IUnityAdsListener  {
    [HideInInspector]
    public bool rewardedAdReady = false;

    #if UNITY_IOS
    private string gameId = "3960832";
    #elif UNITY_ANDROID
    private string gameId = "3960833";
    #endif
    private bool testMode = false;

    private string nonRewardedPlacement = "video"; 
    private string rewardedPlacement = "rewardedVideo"; 
    private string bannerPlacement = "top_banner"; 
    private RewardType rewardType = RewardType.Revive;

    void Start () {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);   
    }

    public void DisplayNonRewardedAd() {
        Advertisement.Show(this.nonRewardedPlacement);
    }

    public void DisplayRewardedAd(RewardType _rewardType) {
        this.rewardType = _rewardType;
        Advertisement.Show(this.rewardedPlacement);
    }

    public void DisplayBannerAd() {
        StartCoroutine(ShowBannerWhenReady());
    }

    IEnumerator ShowBannerWhenReady () {
        while (!Advertisement.IsReady(this.bannerPlacement)) {
            yield return new WaitForSeconds (0.5f);
        }
        Advertisement.Banner.Show(this.bannerPlacement);
    }

    public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) {
        if (placementId == this.rewardedPlacement)
            this.rewardedAdReady = false;

        if (showResult == ShowResult.Finished) {
            if (placementId == this.rewardedPlacement) {
                switch(this.rewardType) {
                    case RewardType.Revive:
                        GameManager.instance.Revive();
                        break;
                    case RewardType.Module:
                        GameManager.instance.GiveExtraModule();
                        break;
                    default:
                        break;
                }
            }
            if (placementId == this.nonRewardedPlacement) {
                GameManager.instance.RestartGame();
            }
        } else if (showResult == ShowResult.Skipped) {
            if (placementId == this.nonRewardedPlacement) {
                GameManager.instance.RestartGame();
            }
        } else if (showResult == ShowResult.Failed) {
            Debug.LogWarning ("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady (string placementId) {
        if (placementId == this.rewardedPlacement) {
            this.rewardedAdReady = true;
        }
    }

    public void OnUnityAdsDidError(string message) {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId) {
        // Optional actions to take when the end-users triggers an ad.
    } 
}
