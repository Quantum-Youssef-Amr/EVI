using System;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private float ShipFullSpeed, ShipTurningSpeed, ShipFuelConsumption, FuelTankSize;
    [SerializeField, Space(8)] private ParticleSystem EngineFlames;
    private float _currentFuelAmount, _timeScaler = 100f, _shipCurrentSpeed = 1;
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
        float m_enginePowerInput = _inputs.Player.enginePower?.ReadValue<float>() ?? 0f;
        ChangeShipSpeed(m_enginePowerInput * 0.1f);

        if (_inputs.Player.Move.IsInProgress())
        {
            Vector2 m_movementVector = _inputs.Player.Move.ReadValue<Vector2>();
            MoveShip(m_movementVector);
            RotateShip(m_movementVector.normalized);
            ConsumeFuel(ShipFuelConsumption);

            EngineFlames.Play();
            var m_engineFlamesParticles = EngineFlames.emission;
            m_engineFlamesParticles.rateOverTime = Mathf.RoundToInt(200f * _shipCurrentSpeed);
            return;
        }

        EngineFlames.Stop();
    }

    private void ConsumeFuel(float shipFuelConsumption)
    {
        _currentFuelAmount -= shipFuelConsumption * Time.deltaTime;
    }

    private void RotateShip(Vector2 MoveDirection)
    {
        _t.rotation = Quaternion.RotateTowards(_t.rotation, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, MoveDirection)), ShipTurningSpeed);
    }

    private void MoveShip(Vector2 MoveVec)
    {
        _rb.AddForce(_shipCurrentSpeed * ShipFullSpeed * _timeScaler * Time.deltaTime * MoveVec, ForceMode2D.Force);
    }

    private void ChangeShipSpeed(float changeDirection)
    {
        _shipCurrentSpeed += changeDirection;
        _shipCurrentSpeed = Mathf.Clamp01(_shipCurrentSpeed);
    }
}
