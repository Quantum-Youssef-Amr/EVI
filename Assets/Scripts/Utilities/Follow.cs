using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] protected Transform Target;
    [SerializeField] protected Vector3 Offset;
    [SerializeField] protected float Speed;
    [SerializeField, Range(0, 1)] protected float Weight;
    protected Transform _followerTransform;

    protected void Start()
    {
        _followerTransform = transform;
    }

    void FixedUpdate()
    {
        TrackTarget();
    }

    public virtual void TrackTarget()
    {
        if (Weight == 0) _followerTransform.position = Target.position + Offset;
        else _followerTransform.position = Vector3.Lerp(_followerTransform.position, Target.position + Offset, Time.deltaTime * Mathf.Max(Weight, 0.1f) * Speed);
    }

}
