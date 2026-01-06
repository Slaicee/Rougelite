using UnityEngine;

public class MenuEnd : MonoBehaviour
{
    public void OnQuitButtonClick()
    {
        Application.Quit();

#if UNITY_EDITOR
        Debug.Log("点击了退出按钮！");
#endif
    }
}