using UnityEngine;

public class SkyShaderController : MonoBehaviour
{
    [SerializeField] private Material SkyMaterial;
    [SerializeField] private Rigidbody2D ShipRb;
    [SerializeField] private float SkyMovementSpeed = 0.01f;

    private Vector2 _offset;
    void LateUpdate()
    {
        _offset += SkyMovementSpeed * Time.deltaTime * ShipRb.linearVelocity;
        SkyMaterial.SetVector("_sky_offset", _offset);
    }
}
