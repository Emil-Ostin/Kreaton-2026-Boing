using UnityEngine;

public class SavePoint : MonoBehaviour
{
    SaveManager saveManager;
    PlayerCamera playerCamera;

    bool saved;
    void Start()
    {
        saveManager = FindFirstObjectByType<SaveManager>();
        playerCamera = FindFirstObjectByType<PlayerCamera>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (saved || !saveManager.savePoints.Contains(this)) return;
        if (other.tag != "Player") return;

        saveManager.currentSavePoint = this;
        saveManager.SavePointCleaner();
        saved = true;

        playerCamera.ShakeTrigger(0.2f, 1);
    }
}
