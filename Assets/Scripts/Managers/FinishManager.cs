using System.Collections;
using UnityEngine;

public class FinishManager : MonoBehaviour
{
    private PlayerController player = null;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var p) && !player)
        {
            player = p;
            GameManager.Ins.Finish(p.isKeyboard ? "WASD PLAYER" : "ARROWS PLAYER");
        }
    }
}
