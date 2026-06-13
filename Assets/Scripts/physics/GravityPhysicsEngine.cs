using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class GravityPhysicsEngine : MonoBehaviour
{
    [SerializeField] private List<Rigidbody2D> Planets;
    [SerializeField] private Rigidbody2D Player;
    [SerializeField] private float G = 0.001f, simTick = 0.4f;

    void Start()
    {
        StartCoroutine(CalculateGravityToPlayer());
    }

    // the gravity calculation will be following the simple (G * m1 * m2/r²)
    private IEnumerator CalculateGravityToPlayer()
    {
        Vector2 m_SumOfGravityForces = Vector2.zero;
        for (int m_planetIdx = 0; m_planetIdx < Planets.Count; m_planetIdx++)
        {
            float m_DistanceFromPlanet = Vector2.Distance(Planets[m_planetIdx].position, Player.position);
            m_SumOfGravityForces += G * Player.mass * Planets[m_planetIdx].mass * Mathf.Pow(m_DistanceFromPlanet, 2f) * (Planets[m_planetIdx].position - Player.position).normalized;
        }

        ApplyGravityToPlayer(m_SumOfGravityForces);

        yield return new WaitForSeconds(simTick);
        StartCoroutine(CalculateGravityToPlayer());
    }

    private void ApplyGravityToPlayer(Vector2 m_SumOfGravityForces)
    {
        Player.AddForce(Time.deltaTime * m_SumOfGravityForces, ForceMode2D.Force);
    }
}
