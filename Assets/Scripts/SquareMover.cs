using UnityEngine;
using UnityEngine.InputSystem;

public class SquareMover : MonoBehaviour
{
    // Velocidade em unidades do mundo por segundo
    public float speed = 5f;

    void Start()
    {
        // Garante que exista um GameState persistente (se você preferir, pode adicionar o GameState no editor)
        if (GameState.Instance == null)
            new GameObject("GameState").AddComponent<GameState>();

        // Restaura posição salva, se houver
        if (GameState.Instance != null && GameState.Instance.hasSquarePosition)
            transform.position = GameState.Instance.squarePosition;
    }

    void Update()
    {
        Vector2 move = Vector2.zero;

        // Teclado (setas + WASD)
        if (Keyboard.current != null)
        {
            move.x = (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed ? 1f : 0f)
                   - (Keyboard.current.leftArrowKey.isPressed  || Keyboard.current.aKey.isPressed ? 1f : 0f);
            move.y = (Keyboard.current.upArrowKey.isPressed    || Keyboard.current.wKey.isPressed ? 1f : 0f)
                   - (Keyboard.current.downArrowKey.isPressed  || Keyboard.current.sKey.isPressed ? 1f : 0f);
        }

        // Gamepad (left stick)
        if (Gamepad.current != null)
        {
            move += Gamepad.current.leftStick.ReadValue();
        }

        if (move.sqrMagnitude > 0f)
            transform.Translate((Vector3)move.normalized * speed * Time.deltaTime, Space.World);
    }

    void OnDestroy()
    {
        // Salva a posição atual antes de ser destruído (por exemplo ao trocar de cena)
        if (GameState.Instance != null)
        {
            GameState.Instance.squarePosition = transform.position;
            GameState.Instance.hasSquarePosition = true;
        }
    }
}