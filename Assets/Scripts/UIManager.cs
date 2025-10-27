using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public Slider HP;                       // 血条 Slider
    public TextMeshProUGUI hpText;          // 血量数值显示
    public TextMeshProUGUI oreText;         // 矿石数量
    public TextMeshProUGUI timerText;       // 倒计时文本
    public GameObject deathUI;              // 阵亡UI界面

    [Header("Player Stats")]
    public float maxHealth = 100f;
    private float currentHealth;
    private int oreCount = 0;

    [Header("Game Timer")]
    public float gameTime = 120f;           // 总时长
    private float timeLeft;
    private bool isGameActive = true;       // 控制计时是否运行

    [Header("Player Object")]
    public GameObject player;               // 玩家对象引用（用于销毁或动画）

    void Start()
    {
        // 初始化玩家状态
        currentHealth = maxHealth;
        timeLeft = gameTime;

        // 初始化血条
        HP.minValue = 0;
        HP.maxValue = maxHealth;
        HP.value = currentHealth;

        // 初始UI同步
        UpdateHealthUI();
        UpdateOreUI();
        UpdateTimerUI();

        if (deathUI != null)
            deathUI.SetActive(false);
    }

    void Update()
    {
        if (isGameActive)
        {
            UpdateTimer();
        }
    }

    // 玩家收到伤害
    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        UpdateHealthUI();

        // 如果血量为0
        if (currentHealth <= 0 && isGameActive)
        {
            PlayerDie();
        }
    }

    // 玩家恢复血量
    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHealthUI();
    }

    public void AddOre(int amount)
    {
        oreCount += amount;
        UpdateOreUI();
    }

    // UI更新函数
    private void UpdateHealthUI()
    {
        HP.value = currentHealth;
    }

    private void UpdateOreUI()
    {
        oreText.text = $"矿石：{oreCount}";
    }

    private void UpdateTimerUI()
    {
        timerText.text = $"时间：{Mathf.CeilToInt(timeLeft)}s";
    }

    private void UpdateTimer()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0) timeLeft = 0;
            UpdateTimerUI();
        }
        else
        {
            isGameActive = false;
            Debug.Log("时间到,可以触发BOSS战或商店逻辑");
        }
    }

    // 玩家阵亡逻辑
    void PlayerDie()
    {
        isGameActive = false;

        Debug.Log("玩家死亡！");

        // 停止玩家控制（可选）
        if (player != null)
        {
            Destroy(player);  // 直接销毁玩家
            // 如果未来想加死亡动画，用 Animator.Play("Die") 替代 Destroy
        }

        // 弹出阵亡UI
        if (deathUI != null)
        {
            deathUI.SetActive(true);
        }

        // 暂停游戏时间
        Time.timeScale = 0f;
    }
    // 重新开始游戏
    public void RestartGame()
    {
        Time.timeScale = 1f; // 恢复时间
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
