using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class FadingAnimationHUDComponent : MonoBehaviour
{
    private enum AnimationState
    {
        FadeIn,
        Visible,
        FadeOut,
        Hidden
    }

    private Dictionary<AnimationState, float> _animationStateAlpha = new Dictionary<AnimationState, float>
    {
        { AnimationState.FadeIn, 0.0f },
        { AnimationState.Visible, 1.0f },
        { AnimationState.FadeOut, 1.0f },
        { AnimationState.Hidden, 0.0f }
    };

    private Dictionary<AnimationState, float> _animationStateDuration = new Dictionary<AnimationState, float>
    {
        { AnimationState.FadeIn, 0.2f },
        { AnimationState.Visible, 3.0f },
        { AnimationState.FadeOut, 0.5f },
        { AnimationState.Hidden, 0.0f }
    };

    private AnimationState _animationState = AnimationState.Hidden;
    private float _timeSinceStateChange = 0.0f;

    private List<CanvasRenderer> _canvasRenderers = new List<CanvasRenderer>();

    public bool IsAnimating => _animationState != AnimationState.Hidden;

    void Awake()
    {
        foreach (CanvasRenderer canvasRenderer in GetComponentsInChildren<CanvasRenderer>())
        {
            canvasRenderer.SetAlpha(0.0f);
            _canvasRenderers.Add(canvasRenderer);
        }
    }

    void Update()
    {
        _timeSinceStateChange += Time.deltaTime;
        AnimateAlpha();
    }

    public void BeginFadeIn()
    {
        ChangeState(AnimationState.FadeIn);
    }

    private void ChangeState(AnimationState newState)
    {
        _animationState = newState;
        _timeSinceStateChange = 0.0f;
    }

    private void AnimateAlpha()
    {
        if (_animationState == AnimationState.Hidden)
        {
            foreach (CanvasRenderer canvasRenderer in _canvasRenderers)
            {
                canvasRenderer.SetAlpha(0.0f);
            }

            return;
        }

        AnimationState nextState = _animationState + 1;
        float alpha = Mathf.Lerp(_animationStateAlpha[_animationState], _animationStateAlpha[nextState], _timeSinceStateChange / _animationStateDuration[_animationState]);
        foreach (CanvasRenderer canvasRenderer in _canvasRenderers)
        {
            canvasRenderer.SetAlpha(alpha);
        }

        if (_timeSinceStateChange >= _animationStateDuration[_animationState])
        {
            ChangeState(nextState);
        }
    }
}
