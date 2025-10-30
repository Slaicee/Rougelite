﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

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
    public Animator playerAnimator;         // 可在 Inspector 手动指定（优先使用）
    public float deathUiDelay = 0.7f;       // 默认等待动画时间（秒），可按动画长度调整


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

        // 缓存Animator
        if (playerAnimator == null && player != null)
        {
            playerAnimator = player.GetComponentInChildren<Animator>();
            if (playerAnimator == null)
                Debug.LogWarning("UIManager：未在player或其子物体中找到Animator，请在Inspector指定playerAnimator。");
        }
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

        if (player != null)
        {
            // 播放死亡动画
            if (playerAnimator != null)
            {
                playerAnimator.SetBool("isDead", true);
            }

            // 禁用玩家的控制脚本（只禁用移动与射击等，不禁用UI/管理脚本）
            var movement = player.GetComponent<PlayerMovement>();
            if (movement != null) movement.enabled = false;

            var shooter = player.GetComponent<PlayerShoot>();
            if (shooter != null) shooter.enabled = false;

            // 启动协程等待动画（用不受timescale影响的等待）
            StartCoroutine(ShowDeathUIAfterAnimation());
        }
        else
        {
            // 没找到player，直接显示UI
            if (deathUI != null) deathUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private IEnumerator ShowDeathUIAfterAnimation()
    {
        yield return new WaitForSecondsRealtime(deathUiDelay);

        // 弹出死亡UI
        if (deathUI != null)
            deathUI.SetActive(true);

        // 最后暂停整个游戏
        Time.timeScale = 0f;
    }


    // 重新开始游戏
    public void RestartGame()
    {
        Time.timeScale = 1f; // 恢复时间
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
