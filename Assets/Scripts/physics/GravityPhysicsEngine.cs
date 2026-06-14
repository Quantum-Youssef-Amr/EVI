using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class GravityPhysicsEngine : MonoBehaviour
{
    public static GravityPhysicsEngine Instance { get; private set; }

    [SerializeField] private List<Rigidbody2D> Planets;
    [SerializeField] private Rigidbody2D Player;
    [SerializeField] private float G = 0.001f, simTick = 0.4f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        print("there is more then 1 Physics managers");
    }

    void Start()
    {
        StartCoroutine(CalculateGravityToPlayer());
    }

    // the gravity calculation will be following the simple (G * m1 * m2/r²)
    private IEnumerator CalculateGravityToPlayer()
    {
        ApplyGravityToPlayer(GetGravityForcesInWorld(Player.position, Player.mass));

        yield return new WaitForSeconds(simTick);
        StartCoroutine(CalculateGravityToPlayer());
    }

    public Vector2 GetGravityForcesInWorld(Vector2 _currentPosition, float selfMass)
    {
        Vector2 m_forces = new();
        for (int m_planetIdx = 0; m_planetIdx < Planets.Count; m_planetIdx++)
        {
            float m_DistanceFromPlanet = Vector2.Distance(Planets[m_planetIdx].position, Player.position);
            // calculate the force in the direction of the current planet
            m_forces += G * selfMass * Planets[m_planetIdx].mass * Mathf.Pow(m_DistanceFromPlanet, 2f) * (Planets[m_planetIdx].position - _currentPosition).normalized;
        }
        return m_forces;
    }

    public Vector2 GetGravityForcesOnPlayer()
    {
        return GetGravityForcesInWorld(Player.position, Player.mass);
    }

    public Vector2 GetGravityForcesUsingPlayerMass(Vector2 _currentPos)
    {
        return GetGravityForcesInWorld(_currentPos, Player.mass);
    }
    public Vector2 GetGravityForcesDirectionVector()
    {
        return GetGravityForcesInWorld(Player.position, Player.mass).normalized;
    }

    public float GetGravityForcesMag()
    {
        return GetGravityForcesInWorld(Player.position, Player.mass).magnitude;
    }

    public float GetGravityForcesAngles()
    {
        return Vector2.SignedAngle(Vector2.right, GetGravityForcesDirectionVector());
    }

    private void ApplyGravityToPlayer(Vector2 m_SumOfGravityForces)
    {
        Player.AddForce(Time.deltaTime * m_SumOfGravityForces, ForceMode2D.Force);
    }
}
