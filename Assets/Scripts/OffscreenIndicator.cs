using UnityEngine;
using UnityEngine.UI;

public class OffscreenIndicator : MonoBehaviour
{
    // Alvo a ser indicado (arraste ou use tag "Triangle")
    public Transform target;

    // O RectTransform do UI Image que será o indicador (seta/círculo)
    public RectTransform indicator;

    // Canvas que contém o indicador (opcional, será encontrado automaticamente)
    public Canvas canvas;

    // Espaçamento em pixels da borda da tela
    public float padding = 20f;

    // Câmera usada para projeção (se null usa Camera.main)
    public Camera cam;

    RectTransform canvasRect;

    void Awake()
    {
        if (canvas == null) canvas = GetComponentInParent<Canvas>();
        if (cam == null) cam = Camera.main;
        if (canvas != null) canvasRect = canvas.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (indicator == null) return;

        if (target == null)
        {
            GameObject t = GameObject.FindGameObjectWithTag("Triangle");
            if (t != null) target = t.transform;
            else
            {
                indicator.gameObject.SetActive(false);
                return;
            }
        }

        Vector3 viewportPos = cam.WorldToViewportPoint(target.position);
        bool onScreen = viewportPos.z > 0f && viewportPos.x >= 0f && viewportPos.x <= 1f && viewportPos.y >= 0f && viewportPos.y <= 1f;

        // Oculta indicador quando target visível
        indicator.gameObject.SetActive(!onScreen);
        if (onScreen) return;

        // Ponto em tela do target
        Vector3 screenPos = cam.WorldToScreenPoint(target.position);

        // Se target estiver atrás da câmera, inverte para posicionar o indicador corretamente
        if (viewportPos.z < 0f)
            screenPos = new Vector3(Screen.width - screenPos.x, Screen.height - screenPos.y, screenPos.z);

        float x = Mathf.Clamp(screenPos.x, padding, Screen.width - padding);
        float y = Mathf.Clamp(screenPos.y, padding, Screen.height - padding);
        Vector2 clampedScreenPos = new Vector2(x, y);

        // Posiciona o indicador conforme o tipo de Canvas
        if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            // ScreenSpace-Camera ou World: converte para posição local do Canvas
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, clampedScreenPos, canvas.worldCamera, out localPoint);
            indicator.anchoredPosition = localPoint;
        }
        else
        {
            // ScreenSpace-Overlay
            indicator.position = clampedScreenPos;
        }

        // Rotaciona o indicador para apontar para o target.
        // Ajuste o -90f se seu sprite aponta para outra direção por padrão.
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 dir = (clampedScreenPos - screenCenter).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        indicator.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}