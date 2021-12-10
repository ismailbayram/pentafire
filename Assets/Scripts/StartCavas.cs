using UnityEngine;
using UnityEngine.UI;

public class StartCavas : MonoBehaviour {
    public GameObject scorePanel;
    public Text scoreText;

    void Start() {
        if (PlayerPrefs.GetInt("BestScore") > 0) {
            this.scorePanel.SetActive(true);
            this.scoreText.text = LocaleHelper.GetText("best_score") + PlayerPrefs.GetInt("BestScore");
        } else {
            this.scorePanel.SetActive(false);
        }
    }
}
