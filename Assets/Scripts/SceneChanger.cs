using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {
    // Chame este método pelo evento OnClick do Button
    public void LoadWorldScene() {
        // Por nome:
        SceneManager.LoadScene("world");
    }
}