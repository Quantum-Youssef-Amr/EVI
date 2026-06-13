using System;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private float ShipSpeed, ShipTurningSpeed, ShipFuelConsumption, FuelTankSize;
    [SerializeField, Space(8)] private ParticleSystem EngineFlames;
    private float _remainingFuel, _currentFuelAmount, _timeScaler = 100f;
    private Rigidbody2D _rb;
    private Transform _t;

    public Action OnFuelRefill;

    #region InputSystem
    private NewInputSystem _inputs;
    void Awake() => _inputs = new();
    void OnEnable() => _inputs.Enable();
    void OnDisable() => _inputs.Disable();
    #endregion

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _t = transform;

        // TODO pull values from save system when done
        _currentFuelAmount = FuelTankSize;

        OnFuelRefill += () => _currentFuelAmount = FuelTankSize;
    }

    void Update()
    {
        if (_inputs.Player.Move.IsInProgress())
        {
            Vector2 m_movementVector = _inputs.Player.Move.ReadValue<Vector2>();
            MoveShip(m_movementVector.y);
            RotateShip(m_movementVector.x);
            ConsumeFuel(ShipFuelConsumption);

            EngineFlames.Play();
            return;
        }

        EngineFlames.Stop();
    }

    private void ConsumeFuel(float shipFuelConsumption)
    {
        _currentFuelAmount -= shipFuelConsumption * Time.deltaTime;
    }

    private void RotateShip(float x)
    {
        if (_currentFuelAmount <= 0) return;

        _rb.AddTorque(ShipTurningSpeed * Time.deltaTime * _timeScaler * -x, ForceMode2D.Force);
    }

    private void MoveShip(float y)
    {
        if (_currentFuelAmount <= 0) return;

        _rb?.AddForce(ShipSpeed * Time.deltaTime * _timeScaler * y * _t.up, ForceMode2D.Force);
    }
}
