using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Vector2Int ZoomBounds = new(8, 16);
    [SerializeField] private float Sensitivity = 2;
    private PixelPerfectCamera _camera;


    #region InputSystem
    private NewInputSystem _inputs;
    void Awake() => _inputs = new();
    void OnEnable() => _inputs.Enable();
    void OnDisable() => _inputs.Disable();
    #endregion

    void Start()
    {
        Application.targetFrameRate = 60;
        _camera = GetComponent<PixelPerfectCamera>();
    }

    void Update()
    {
        float m_scrollDelta = _inputs.Player.zoom.ReadValue<Vector2>().y;
        _camera.assetsPPU = Mathf.RoundToInt(m_scrollDelta * Sensitivity + _camera.assetsPPU);
        _camera.assetsPPU = Mathf.Clamp(_camera.assetsPPU, ZoomBounds.x, ZoomBounds.y);
    }
}
