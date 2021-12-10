using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    public GameObject revivePanel;
    public GameObject restartPanel;
    public GameObject statistics;
    public Text scoreText;
    public Text doubleKillCountText;
    public Text doubleKillPointText;
    public Text tripleKillCountText;
    public Text tripleKillPointText;
    public Text quadraKillCountText;
    public Text quadraKillPointText;
    public Text pentaKillCountText;
    public Text pentaKillPointText;
    public Text masterKillCountText;
    public Text masterKillPointText;
    public GameObject bestScoreText;

    public Animator statsAnimator;

    public AudioClip statisticEffect;
    public AudioClip bonusEffect;

    private void OnEnable() {
        this.restartPanel.GetComponent<CanvasGroup>().alpha = 0f;
        if (GameManager.instance.alreadyRevived || !GameManager.instance.adService.rewardedAdReady) {
            this.PassReviveAd();
        } else {
            this.revivePanel.SetActive(true);
            this.statistics.SetActive(false);
        }
    }

    public void PassReviveAd() {
        this.revivePanel.SetActive(false);
        this.statistics.SetActive(true);
        this.scoreText.text =LocaleHelper.GetText("score") + GameManager.instance.score.ToString();
        this.doubleKillCountText.text = GameManager.instance.doubleKillCount.ToString() + " X";
        this.doubleKillPointText.text = "+" + (GameManager.instance.doubleKillCount * 10).ToString();
        this.tripleKillCountText.text = GameManager.instance.tripleKillCount.ToString() + " X";
        this.tripleKillPointText.text = "+" + (GameManager.instance.tripleKillCount * 20).ToString();
        this.quadraKillCountText.text = GameManager.instance.quadraKillCount.ToString() + " X";
        this.quadraKillPointText.text = "+" + (GameManager.instance.quadraKillCount * 30).ToString();
        this.pentaKillCountText.text = GameManager.instance.pentaKillCount.ToString() + " X";
        this.pentaKillPointText.text = "+" + (GameManager.instance.pentaKillCount * 40).ToString();
        this.masterKillCountText.text = GameManager.instance.masterKillCount.ToString() + " X";
        this.masterKillPointText.text = "+" + (GameManager.instance.masterKillCount * 50).ToString();
        StartCoroutine(StatisticEffect());
    }

    IEnumerator StatisticEffect() {
        for (int i = 0; i < 6; i++) {
            GameManager.instance.audioSource.PlayOneShot(this.statisticEffect);
            yield return new WaitForSecondsRealtime(0.4f);
        }
        StartCoroutine(AddBonusScore());
    }

    IEnumerator AddBonusScore() {
        yield return new WaitForSecondsRealtime(0.5f);
        int bonus = 0;
        int targetBonusValue = 0;
        targetBonusValue += GameManager.instance.doubleKillCount * 10;
        targetBonusValue += GameManager.instance.tripleKillCount * 20;
        targetBonusValue += GameManager.instance.quadraKillCount * 30;
        targetBonusValue += GameManager.instance.pentaKillCount * 40;
        targetBonusValue += GameManager.instance.masterKillCount * 50;

        if(targetBonusValue == 0)
            yield return null;
        
        GameManager.instance.audioSource.PlayOneShot(this.bonusEffect);
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.02f);

        while (bonus <= targetBonusValue) {
            bonus += 10;
            this.scoreText.text = LocaleHelper.GetText("score") + (GameManager.instance.score + bonus).ToString();
            yield return wait;
        }

        yield return new WaitForSecondsRealtime(0.5f);
        if (PlayerPrefs.GetInt("BestScore") < (GameManager.instance.score + bonus)) {
            statsAnimator.Play("StatsBestScore");
            GameManager.instance.audioSource.PlayOneShot(this.statisticEffect);
            PlayerPrefs.SetInt("BestScore", GameManager.instance.score + bonus);
        }
        
        this.restartPanel.GetComponent<CanvasGroup>().alpha = 1f;
    }

    public void Revive() {
        GameManager.instance.adService.DisplayRewardedAd(RewardType.Revive);
    }

    public void Restart() {
        this.bestScoreText.GetComponent<CanvasGroup>().alpha = 0f;
        PlayerPrefs.SetInt("AdCount", PlayerPrefs.GetInt("AdCount") + 1);
        if (PlayerPrefs.GetInt("AdCount") % 3 == 2) {
            GameManager.instance.adService.DisplayNonRewardedAd();
        } else {
            GameManager.instance.RestartGame();
        }
    }
}
