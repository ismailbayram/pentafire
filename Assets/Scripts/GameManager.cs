using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    
    [Header("Variables")]
    public float energy = 100f;
    public int score = 0;

    [Header("UI")]
    public Slider slider;
    public Text energyText;
    public Text scoreText;
    public NotificationUI notificationUI;
    public GameObject startCanvas;
    public GameObject playCanvas;
    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;

    [Header("Pools")]
    public ObjectPool enemyRedPool;
    public ObjectPool enemyOrangePool;
    public ObjectPool enemyBrownPool;
    public ObjectPool bulletPool;
    public ObjectPool missilePool;
    public ObjectPool energyPool;

    [Header("Modules")]
    public GameObject speedyModule;
    public GameObject reflectorModule;
    public GameObject ballsModule;
    public ExtraModule extraModule;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip energyClip;
    public AudioClip playerShotClip;
    public AudioClip moduleClip;

    [Header("Stats")]
    public int enemyDeathCount = 0;
    public int doubleKillCount = 0;
    public int tripleKillCount = 0;
    public int quadraKillCount = 0;
    public int pentaKillCount = 0;
    public int masterKillCount = 0;

    [HideInInspector]
    public bool anyActiveModuleExists = false;
    [HideInInspector]
    public bool alreadyRevived = false;

    [Header("Others")]
    public Player player;
    public CameraController cameraController;
    public AdService adService;
    
    [HideInInspector]
    public int activeEnemy = 0;
    
    private GameObject enemyToSpawn;
    private Vector3 spawnPoint;

    private float targetEnergyValue = 100f;
    private int comboEnemyCount = 0;
    private bool playerUntargetable = false;
    private int maxEnemyCount = 5;

    private void Awake() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Start() {
        this.targetEnergyValue = this.energy;
    }

    private void Update() {
        if (this.energy != this.targetEnergyValue) {
            this.energy = Mathf.MoveTowards(this.energy, this.targetEnergyValue, 100f * Time.deltaTime);
            this.slider.value = this.energy;
            this.energyText.text = ((int)this.energy).ToString() + "/100";
        }
    }

    public void StartGame() {
        this.scoreText.text = LocaleHelper.GetText("score") + + this.score;
        this.adService.DisplayBannerAd();
        InvokeRepeating("SpawnEnemy", 1f, 1);
        InvokeRepeating("ShowExtraModule", 8f, 1);
        this.startCanvas.SetActive(false);
        this.playCanvas.SetActive(true);
    }

    public void PauseGame() {
        this.pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ContinueGame() {
        this.pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame() {
        Time.timeScale = 1f;
        this.energy = 100f;
        this.targetEnergyValue = this.energy;
        this.slider.value = this.energy;
        this.energyText.text = ((int)this.energy).ToString() + "/100";
        this.player.Reset();
        this.score = 0;
        this.scoreText.text = LocaleHelper.GetText("score") + + this.score;
        this.enemyDeathCount = 0;
        this.comboEnemyCount = 0;
        this.doubleKillCount = 0;
        this.tripleKillCount = 0;
        this.quadraKillCount = 0;
        this.pentaKillCount = 0;
        this.masterKillCount = 0;
        this.activeEnemy = 0;
        this.maxEnemyCount = 5;
        this.notificationUI.Reset();
        this.reflectorModule.SetActive(false);
        this.speedyModule.SetActive(false);
        this.ballsModule.SetActive(false);
        this.extraModule.gameObject.SetActive(false);
        this.energyPool.RemoveAllObjects();
        this.enemyRedPool.RemoveAllObjects();
        this.enemyOrangePool.RemoveAllObjects();
        this.enemyBrownPool.RemoveAllObjects();
        this.cameraController.Reset();
        this.alreadyRevived = false;
        this.pauseCanvas.SetActive(false);
        this.gameOverCanvas.SetActive(false);
        this.playerUntargetable = false;
        foreach(GameObject bullet in this.bulletPool.pooledObjects) {
            bullet.GetComponent<Bullet>().Deactivate();
        }
        foreach(GameObject missile in this.missilePool.pooledObjects) {
            missile.GetComponent<Bullet>().Deactivate();
        }
    }

    IEnumerator GameOver() {
        this.playerUntargetable = true;
        yield return new WaitForSeconds(0.5f);
        this.cameraController.Reset();
        Time.timeScale = 0f;
        this.gameOverCanvas.SetActive(true);
        this.extraModule.gameObject.SetActive(false);
    }

    public void Revive() {
        this.alreadyRevived = true;
        this.gameOverCanvas.SetActive(false);
        Time.timeScale = 1f;
        StartCoroutine(RemovePlayerUntargetable());
    }

    IEnumerator RemovePlayerUntargetable() {
        this.player.Revive(1.5f);
        yield return new WaitForSeconds(1.75f);
        this.playerUntargetable = false;
    }

    public void ChangeEnergy(float value) {
        if (value > 0) {
            this.audioSource.PlayOneShot(this.energyClip);
        }
        this.targetEnergyValue += value;
        this.targetEnergyValue = Mathf.Clamp(this.targetEnergyValue, 0f, 100f);
    }

    private void SpawnEnemy() {
        if (this.activeEnemy == this.maxEnemyCount)
            return;
        if (this.score < 100) {
            this.enemyToSpawn = this.enemyRedPool.GetObjectFromPool();
        } else {
            int i = Random.Range(0, 3);
            switch(i) {
                case 0:
                    this.enemyToSpawn = this.enemyRedPool.GetObjectFromPool();
                    break;
                case 1:
                    this.enemyToSpawn = this.enemyOrangePool.GetObjectFromPool();
                    break;
                case 2:
                    this.enemyToSpawn = this.enemyBrownPool.GetObjectFromPool();
                    break;
                default:
                    break;
            }
        }
        if (this.enemyToSpawn == null)
            return;
        this.spawnPoint.x = Random.Range(-13, 12);
        this.spawnPoint.z = Random.Range(-12, 13);
        this.enemyToSpawn.transform.position = this.spawnPoint;
        this.enemyToSpawn.SetActive(true);
        this.activeEnemy++;
        this.enemyToSpawn = null;
    }

    public void AddScore(int value) {
        this.score += value;
        this.scoreText.text = LocaleHelper.GetText("score") + + this.score;
        this.maxEnemyCount = Mathf.Min(5 + this.score / 100, 20);
    }

    public void GiveExtraModule() {
        this.ContinueGame();
        this.extraModule.rewardedModule.ActivateModule();
    }

    private void ShowExtraModule() {
        if (Random.Range(0, 9) < 2 && !this.anyActiveModuleExists && this.adService.rewardedAdReady) {
            this.anyActiveModuleExists = true;
            this.extraModule.gameObject.SetActive(true);
        }
    }

    public void CheckPlayerDeath() {
        if (this.playerUntargetable)
            return;
        Time.timeScale = .5f;
        StartCoroutine(SlowMotion(1f));
        this.audioSource.PlayOneShot(this.playerShotClip);
        this.cameraController.ShakeCamera(1f);
        if (this.energy >= 50f) {
            this.ChangeEnergy(-50f);
            this.player.arch.SetActive(true);
            StartCoroutine(DeactivatePlayerArch());
        } else {
            StartCoroutine(GameOver());
        }
    }

    IEnumerator SlowMotion(float duration) {
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1f;
    }

    IEnumerator DeactivatePlayerArch() {
        yield return new WaitForSeconds(1f);
        this.player.arch.SetActive(false);
    }

    public void EnemyDied(Vector3 diedPoint) {
        this.activeEnemy--;
        this.enemyDeathCount++;
        StartCoroutine(CheckCombo(this.comboEnemyCount));
        this.comboEnemyCount++;

        float random = Random.Range(0, 100);
        if (random <= 10f && !this.anyActiveModuleExists) {
            this.anyActiveModuleExists = true;
            if (random <= 0f) {
                this.speedyModule.transform.position = diedPoint + Vector3.forward;
                this.speedyModule.SetActive(true);
            } else if(random <= 6f) {
                this.reflectorModule.transform.position = diedPoint + Vector3.forward;
                this.reflectorModule.SetActive(true);
            } else if(random <= 10f) {
                this.ballsModule.transform.position = diedPoint + Vector3.forward;
                this.ballsModule.SetActive(true);
            } else {
                this.anyActiveModuleExists = true;
            }
        }
    }

    IEnumerator CheckCombo(int enemyDied) {
        yield return new WaitForSeconds(1f);
        if (enemyDied != this.comboEnemyCount)
            yield return null;
        switch(this.comboEnemyCount) {
            case 0:
                break;
            case 1:
                break;
            case 2:
                this.doubleKillCount++;
                this.notificationUI.AddNotificationItem(NotificationType.DoubleKill);
                break;
            case 3:
                this.tripleKillCount++;
                this.notificationUI.AddNotificationItem(NotificationType.TripleKill);
                break;
            case 4:
                this.quadraKillCount++;
                this.notificationUI.AddNotificationItem(NotificationType.QuadraKill);
                break;
            case 5:
                this.pentaKillCount++;
                this.notificationUI.AddNotificationItem(NotificationType.PentaKill);
                break;
            default:
                this.masterKillCount++;
                this.notificationUI.AddNotificationItem(NotificationType.MasterKill);
                break;
        }
        this.comboEnemyCount = 0;
    }
}
