using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public GameObject crosshairUI; // ����׼��UI����Canvas�µ�Crosshair��
    private RectTransform crosshairRect; // ׼�ĵ�RectTransform���
    private Canvas parentCanvas; // ׼�����ڵ�Canvas

    private bool isInUI = false;   // ��ǰ�Ƿ���UIģʽ

    void Start()
    {
        // ��ȡ׼�ĵ�RectTransform������Canvas
        if (crosshairUI != null)
        {
            crosshairRect = crosshairUI.GetComponent<RectTransform>();
            parentCanvas = crosshairUI.GetComponentInParent<Canvas>();
        }
        EnterGameMode(); // ��ʼ����ս��ģʽ
    }

    void Update()
    {
        // ����Tab���л�UI/��Ϸģʽ
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isInUI)
                EnterGameMode();
            else
                EnterUIMode();
        }

        // ��Ϸģʽ�£�����׼��λ��Ϊ���λ��
        if (!isInUI && crosshairRect != null && parentCanvas != null)
        {
            UpdateCrosshairPosition();
        }
    }

    // ����׼��λ�õ������Ļ����
    void UpdateCrosshairPosition()
    {
        // �������Ļ����ת��ΪCanvas�ڵ�UI����
        Vector2 mousePos;
        // ���䲻ͬCanvas��Ⱦģʽ��Screen Space - Overlay/ Camera��
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            Input.mousePosition,
            parentCanvas.worldCamera,
            out mousePos
        );

        // ����׼��λ��
        crosshairRect.anchoredPosition = mousePos;
    }

    public void EnterUIMode()
    {
        isInUI = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (crosshairUI != null) crosshairUI.SetActive(false);
        Time.timeScale = 0f;
    }

    public void EnterGameMode()
    {
        isInUI = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        if (crosshairUI != null) crosshairUI.SetActive(true);
        Time.timeScale = 1f;
    }
}
