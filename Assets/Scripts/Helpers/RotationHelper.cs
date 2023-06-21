using UnityEngine;

public class RotationHelper : MonoBehaviour
{
    public Vector3 rotation;
    private void Start()
    {
        transform.localRotation = Quaternion.Euler(rotation);
    }
}
