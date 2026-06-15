using UnityEngine;
using System.Collections;
using System;
using UnityEngine.InputSystem.Interactions;

public class DrawTrajectoryPredictionLine : MonoBehaviour
{
    [SerializeField] private LineRenderer TrajectoryLine;
    [SerializeField] private Rigidbody2D ShipRB;
    [SerializeField] private int LineUpdateRate = 4, lineLength = 10;

    private Vector2 _TrajectoryPoint;
    private bool _isGonnaHit;

    void Start() => StartCoroutine(UpdateTrajectoryLine());


    private IEnumerator UpdateTrajectoryLine()
    {
        if (TrajectoryLine.positionCount < lineLength)
            setUpLineRenderer();

        _TrajectoryPoint = ShipRB.position;
        TrajectoryLine.SetPosition(0, _TrajectoryPoint);
        Vector2 m_simLinearVelocity = ShipRB.linearVelocity;

        _isGonnaHit = false;
        TrajectoryLine.endColor = TrajectoryLine.startColor;


        for (int m_linePoint = 1; m_linePoint < lineLength; m_linePoint++)
        {
            if (_isGonnaHit)
            {
                TrajectoryLine.positionCount = m_linePoint + 1;
                TrajectoryLine.endColor = Color.red;
                break;
            }

            //  F = ma => a = F/m
            // v1 = v0 + F/m
            Vector2 m_gravityForce = GravityPhysicsEngine.Instance.GetGravityForcesUsingPlayerMass(_TrajectoryPoint) * Time.deltaTime;
            m_simLinearVelocity += m_gravityForce / ShipRB.mass;
            _TrajectoryPoint += m_simLinearVelocity;
            TrajectoryLine.SetPosition(m_linePoint, _TrajectoryPoint);

            if (Physics2D.CircleCast(_TrajectoryPoint, 0.1f, Vector2.zero).collider != null)
            {
                _isGonnaHit = true;
            }
        }

        yield return new WaitForSeconds(1f / LineUpdateRate);
        StartCoroutine(UpdateTrajectoryLine());
    }

    private void setUpLineRenderer() => TrajectoryLine.positionCount = lineLength;
}
