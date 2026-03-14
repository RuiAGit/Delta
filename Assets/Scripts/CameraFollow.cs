using UnityEngine;

public class CameraFollow : MonoBehaviour {
    // Arraste o transform do quadrado aqui no Inspector (ou marque o quadrado com tag "Player")
    public Transform target;

    // Offset relativo ao alvo. Por padrão mantém a câmera em z = -10 (boa para 2D)
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    // Se true, suaviza o movimento (lerp). Se quer "travada" use false.
    public bool smooth = false;
    public float smoothSpeed = 10f;

    void LateUpdate() {
        if (target == null) {
            GameObject maybe = GameObject.FindGameObjectWithTag("Player");
            if (maybe != null) target = maybe.transform;
            else return; // nada para seguir
        }

        Vector3 desired = new Vector3(target.position.x + offset.x, target.position.y + offset.y, offset.z);

        if (smooth)
            transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
        else
            transform.position = desired;
    }
}