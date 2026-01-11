using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public Slider HP;                       // 血条 Slider
    public TextMeshProUGUI hpText;          // 血量数值显示
    public TextMeshProUGUI oreText;         // 矿石数量
    // 游戏过程中显示的时长文本
    public TextMeshProUGUI playTimeText;
    // 阵亡UI里显示的时长文本
    public TextMeshProUGUI deathPlayTimeText;
    public GameObject deathUI;              // 阵亡UI界面
    public Button exitGameBtn;

    // 游戏进行时长
    private float gamePlayTime = 0f;        // 累计游戏时长（秒），从0开始递增
    private bool isGameActive = true;       // 控制计时是否运行
    public bool IsPlayerDead => isGameActive == false;

    [Header("Player Object")]
    public GameObject player;               // 玩家对象引用
    public Animator playerAnimator;         // 玩家动画器
    public float deathUiDelay = 1.25f;      // 阵亡UI延迟显示时间

    [Header("Cursor Manager")]
    public CursorManager cursorManager;

    [Header("Player State")]
    public PlayerState playerState;

    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;

        // 自动查找 PlayerState
        if (playerState == null && player != null)
            playerState = player.GetComponent<PlayerState>();

        if (playerState == null)
        {
            Debug.LogError("UIManager：找不到 PlayerState，无法显示血量！");
            return;
        }

        // 初始化血条
        HP.minValue = 0;
        HP.maxValue = playerState.maxHealth;
        HP.value = playerState.currentHealth;

        UpdateHealthUI();
        UpdateOreUI();
        // 初始化游戏时长
        gamePlayTime = 0f;
        UpdatePlayTimeUI();

        if (deathUI != null)
            deathUI.SetActive(false);

        // 缓存Animator
        if (playerAnimator == null && player != null)
        {
            playerAnimator = player.GetComponentInChildren<Animator>();
        }

        if (exitGameBtn != null)
        {
            exitGameBtn.onClick.AddListener(ExitGame);
        }
        else
        {
            Debug.LogWarning("UIManager：exitGameBtn 未赋值！请在Inspector中拖入退出游戏按钮");
        }
    }

    void Update()
    {
        if (!isGameActive || playerState == null) return;

        // 实时同步UI
        UpdateHealthUI();
        UpdateOreUI();

        // 更新游戏进行时长（核心）
        UpdateGamePlayTime();
    }

    // 累计并更新游戏进行时长
    private void UpdateGamePlayTime()
    {
        // 检查游戏是否暂停（兼容PauseManager）
        bool isPaused = false;
        var pauseMgr = FindObjectOfType<PauseManager>();
        if (pauseMgr != null) isPaused = pauseMgr.IsPaused;

        // 只有游戏活跃且未暂停时，累计时长
        if (isGameActive && !isPaused)
        {
            gamePlayTime += Time.deltaTime;
            UpdatePlayTimeUI(); // 实时更新时长显示
        }
    }

    private void UpdatePlayTimeUI()
    {
        if (playTimeText != null)
        {
            string formattedTime = FormatTime(gamePlayTime);
            playTimeText.text = $"游戏时长：{formattedTime}";
        }
    }

    private string FormatTime(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);
        return $"{minutes}分{seconds}秒"; 
    }

    // 玩家收到伤害
    public void TakeDamage(float amount)
    {
        if (playerState == null) return;

        if (playerState.isInvincible)
        {
            Debug.Log("光之守护生效！免疫伤害：" + amount);
            return;
        }

        playerState.currentHealth = Mathf.Clamp(playerState.currentHealth - amount, 0, playerState.maxHealth);
        UpdateHealthUI();

        if (playerState.currentHealth <= 0 && isGameActive)
        {
            PlayerDie();
        }
    }

    // 玩家恢复血量
    public void Heal(float amount)
    {
        if (playerState == null) return;

        playerState.currentHealth = Mathf.Clamp(playerState.currentHealth + amount, 0, playerState.maxHealth);
        UpdateHealthUI();
    }

    public void AddOre(int amount)
    {
        if (playerState == null) return;

        playerState.ore += amount;
        UpdateOreUI();
    }

    // UI更新函数
    public void UpdateHealthUI()
    {
        if (playerState == null) return;

        HP.maxValue = playerState.maxHealth;
        HP.value = playerState.currentHealth;
    }

    public void UpdateOreUI()
    {
        if (playerState != null && oreText != null)
            oreText.text = $"矿石：{playerState.ore}";
    }

    // 玩家阵亡逻辑
    void PlayerDie()
    {
        isGameActive = false;
        Debug.Log("玩家死亡！");

        if (player != null)
        {
            // 播放死亡动画
            if (playerAnimator != null)
            {
                playerAnimator.SetBool("isDead", true);
            }

            // 禁用 PlayerController
            var playerCtrl = player.GetComponent<PlayerController>();
            if (playerCtrl != null)
                playerCtrl.enabled = false;

            // 启动协程等待固定时间
            StartCoroutine(ShowDeathUIAfterDelay(deathUiDelay));
        }
        else
        {
            // 没找到player，直接显示UI并设置阵亡时长
            if (deathUI != null) deathUI.SetActive(true);
            SetDeathPlayTimeUI();
            Time.timeScale = 0f;
        }
    }

    private IEnumerator ShowDeathUIAfterDelay(float delay)
    {
        // 等待固定时间
        yield return new WaitForSecondsRealtime(delay);

        // 弹出死亡UI
        if (deathUI != null)
            deathUI.SetActive(true);

        // 在阵亡UI显示最终时长
        SetDeathPlayTimeUI();

        // 切换鼠标状态
        if (cursorManager != null)
            cursorManager.EnterUIMode();

        // 最后暂停游戏
        Time.timeScale = 0f;
    }

    // 设置阵亡UI的时长显示
    private void SetDeathPlayTimeUI()
    {
        if (deathPlayTimeText != null)
        {
            string formattedTime = FormatTime(gamePlayTime);
            deathPlayTimeText.text = $"本局时长：{formattedTime}";
        }
    }

    // 重新开始游戏
    public void RestartGame()
    {
        Time.timeScale = 1f; // 恢复时间
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        // 恢复游戏时间缩放（避免暂停状态下退出异常）
        Time.timeScale = 1f;

        // 区分编辑器和打包后的逻辑：
        // 1. 编辑器中测试：停止游戏运行
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        // 2. 打包后（PC/移动端）：退出游戏
#else
        Application.Quit();
#endif

        Debug.Log("退出游戏！");
    }
}