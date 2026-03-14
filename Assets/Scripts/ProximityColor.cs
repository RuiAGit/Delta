using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ProximityColor : MonoBehaviour {
    // Alvo fixo (triangle). Arraste no Inspector ou marque o triangle com tag "Triangle".
    public Transform triangleTarget;

    // Distância (unidades do mundo) para considerar "perto"
    public float range = 2f;

    // Cores quando perto / longe
    public Color colorNear = Color.red;
    public Color colorFar = Color.white;

    // Componentes do triangle suportados
    SpriteRenderer triangleSpriteRenderer;
    Renderer triangleMeshRenderer;
    Image triangleUIImage;

    void Awake() {
        // não cacheia até ter triangleTarget; será resolvido em Update/CacheRenderers
    }

    void Update() {
        if (triangleTarget == null) {
            GameObject t = GameObject.FindGameObjectWithTag("Triangle");
            if (t != null) triangleTarget = t.transform;
            else return; // sem alvo conhecido
        }

        // Cacheia os componentes do triangle na primeira vez que o alvo for conhecido
        if (triangleSpriteRenderer == null && triangleMeshRenderer == null && triangleUIImage == null) {
            CacheRenderers();
        }

        float dist = Vector2.Distance(transform.position, triangleTarget.position);
        bool isNear = dist <= range;

        Color targetColor = isNear ? colorNear : colorFar;

        if (triangleSpriteRenderer != null) {
            triangleSpriteRenderer.color = targetColor;
        }
        else if (triangleUIImage != null) {
            triangleUIImage.color = targetColor;
        }
        else if (triangleMeshRenderer != null) {
            // Acessar triangleMeshRenderer.material instancia o material para esse renderer,
            // evitando alterar o material compartilhado entre objetos.
            triangleMeshRenderer.material.color = targetColor;
        }

        // Se estiver perto, escuta a tecla Espaço (novo Input System) ou botão do gamepad
        if (isNear) {
            bool pressed = false;

            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                pressed = true;

            if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
                pressed = true;

            if (pressed) {
                // Troca para a cena "cardGame"
                SceneManager.LoadScene("cardGame");
            }
        }
    }

    void CacheRenderers() {
        if (triangleTarget == null) return;

        triangleSpriteRenderer = triangleTarget.GetComponent<SpriteRenderer>();
        triangleMeshRenderer = triangleTarget.GetComponent<Renderer>();
        triangleUIImage = triangleTarget.GetComponent<Image>();
    }

    void OnDrawGizmosSelected() {
        if (triangleTarget != null) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(triangleTarget.position, range);
        }
    }
}