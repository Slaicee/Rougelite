using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public Slider HP;                       // 血条 Slider
    public TextMeshProUGUI hpText;          // 血量数值显示
    public TextMeshProUGUI oreText;         // 矿石数量
    public TextMeshProUGUI timerText;       // 倒计时文本

    [Header("Player Stats")]
    public float maxHealth = 100f;
    private float currentHealth;
    private int oreCount = 0;

    [Header("Game Timer")]
    public float gameTime = 120f;           // 总时长
    private float timeLeft;
    private bool isGameActive = true;       // 控制计时是否运行

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
    }

    void Update()
    {
        if (isGameActive)
        {
            UpdateTimer();
        }
    }

    // 玩家状态控制
    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        UpdateHealthUI();

        // 如果血量为0可以在这里触发死亡逻辑
        if (currentHealth <= 0)
        {
            isGameActive = false;
            Debug.Log("玩家死亡！");
        }
    }

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
}
