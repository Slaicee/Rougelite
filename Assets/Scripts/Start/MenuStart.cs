// 1. 定义脚本类（核心前提）
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuStart : MonoBehaviour
{
    // 2. 定义按钮点击的响应方法
    public void OnStartButtonClick()
    {
        // 3. 核心逻辑：加载下一个场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}