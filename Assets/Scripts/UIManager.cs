using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public Slider HP;                       // 拖入血条 Slider
    public TextMeshProUGUI hpText;          // 拖入血量文本
    public TextMeshProUGUI oreText;         // 拖入矿石文本
    public TextMeshProUGUI timerText;       // 拖入计时文本

    [Header("Player Stats")]
    public float maxHealth = 100f;
    private float currentHealth;
    private int oreCount = 0;

    [Header("Game Timer")]
    public float gameTime = 120f;           // 倒计时起始秒数
    private float timeLeft;

    void Start()
    {
        currentHealth = maxHealth;
        timeLeft = gameTime;

        //同步血条参数
        HP.minValue = 0;
        HP.maxValue = maxHealth;
        HP.value = maxHealth;

        UpdateHealthUI();
        UpdateOreUI();
        UpdateTimerUI();
    }

    void Update()
    {
        //倒计时逻辑
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0) timeLeft = 0;
            UpdateTimerUI();
        }
    }

    //扣血
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    //回复血量
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    //增加矿石数量
    public void AddOre(int amount)
    {
        oreCount += amount;
        UpdateOreUI();
    }

    //UI 更新
    private void UpdateHealthUI()
    {
        HP.value = currentHealth;
    }

    private void UpdateOreUI()
    {
        oreText.text = "矿石：" + oreCount.ToString();
    }

    private void UpdateTimerUI()
    {
        timerText.text = "时间：" + Mathf.CeilToInt(timeLeft).ToString();
    }
}
