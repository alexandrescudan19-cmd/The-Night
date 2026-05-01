using UnityEngine;
using UnityEngine.UI;

public class FirstPersonMouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CreateCrosshair();
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // controlează camera (sus-jos)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // rotește corpul jucătorului (stânga-dreapta)
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void CreateCrosshair()
    {
        if (GameObject.Find("CrosshairCanvas") != null)
            return;

        GameObject canvasGO = new GameObject("CrosshairCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        Color crosshairColor = Color.red; // schimbă aici culoarea

        // Linie orizontală
        GameObject horizontalLine = new GameObject("HorizontalLine");
        horizontalLine.transform.SetParent(canvasGO.transform);
        Image hImage = horizontalLine.AddComponent<Image>();
        hImage.color = crosshairColor;

        RectTransform hRect = horizontalLine.GetComponent<RectTransform>();
        hRect.sizeDelta = new Vector2(20, 2);
        hRect.anchorMin = hRect.anchorMax = new Vector2(0.5f, 0.5f);
        hRect.anchoredPosition = Vector2.zero;

        // Linie verticală
        GameObject verticalLine = new GameObject("VerticalLine");
        verticalLine.transform.SetParent(canvasGO.transform);
        Image vImage = verticalLine.AddComponent<Image>();
        vImage.color = crosshairColor;

        RectTransform vRect = verticalLine.GetComponent<RectTransform>();
        vRect.sizeDelta = new Vector2(2, 20);
        vRect.anchorMin = vRect.anchorMax = new Vector2(0.5f, 0.5f);
        vRect.anchoredPosition = Vector2.zero;
    }
}
