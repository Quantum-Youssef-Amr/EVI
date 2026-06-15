using UnityEngine;
using UnityEngine.U2D.Animation;
using System.Collections;
using System;

[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(SpriteResolver)), RequireComponent(typeof(SpriteLibrary))]
public class PlanetSpriteAnimator : MonoBehaviour
{
    [SerializeField] private SpriteResolver spriteResolver;
    [SerializeField] private int AnimatingRate, StopFrame;

    private SpriteRenderer _spR;
    void Start()
    {
        _spR = GetComponent<SpriteRenderer>();
        StartCoroutine(AnimatePlanet());
    }

    private int _currentFrameNumber;
    private IEnumerator AnimatePlanet()
    {
        if (_currentFrameNumber >= StopFrame) _currentFrameNumber = 0;
        spriteResolver.SetCategoryAndLabel("0", $"{_spR.sprite.texture.name}_{_currentFrameNumber}");

        yield return new WaitForSeconds(1f / AnimatingRate);
        _currentFrameNumber++;
        StartCoroutine(AnimatePlanet());
    }
}
