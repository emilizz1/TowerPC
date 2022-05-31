using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TweenAnimator : MonoBehaviour
{
    [SerializeField] public List<TweenData> data;
    [SerializeField] public List<UnityEvent> customBeginEvents;
    [SerializeField] public List<UnityEvent> customCompleteEvents;

    const double TOLERANCE = 0.001f;
    
    int currentTween;
    int completeEventTweenIndex;

    Transform cTransform;
    RectTransform rectTransform;
    SpriteRenderer spriteRenderer;
    CanvasGroup canvasGroup;
    Image image;
    Text txt;
    
    void OnEnable()
    {
        CollectComponents();
        if (data != null)
        {
            for (int _i = 0; _i < data.Count; _i++)
            {
                if (data[_i].performOnEnable)
                {
                    PerformTween(_i);
                }
            }
        }
    }

    void CollectComponents()
    {
        rectTransform = GetComponent<RectTransform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();
        txt = GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
        cTransform = transform;
    }

    public void PerformTween(int _index)
    {
        if (IsObjectTweening() && !data[_index].doNotWaitForComplete || !gameObject.activeInHierarchy)
        {
            return;
        }

        if (data[_index].performBeginEvent)
        {
            customBeginEvents[_index]?.Invoke();
        }

        if (data[_index].doNotWaitForComplete)
        {
            LeanTween.cancel(gameObject);
            CancelTween();
        }
        
        if (data[_index].move)
        {
            if(rectTransform)
            {
                Move(data[_index].position, rectTransform.anchoredPosition, _index);
            }
            else
            {
                Move(data[_index].position, transform.localPosition, _index);
            }
        }

        if (data[_index].rotate)
        {
            Rotate(data[_index].rotation, transform.localEulerAngles, _index);
        }

        if (data[_index].changeScale)
        {
            Scale(data[_index].scale, transform.localScale, _index);
        }
        
        if (data[_index].changeColor)
        {
            ChangeColor(_index);
        }
        
        if (data[_index].changeAlpha)
        {
            ChangeAlpha(_index);
        }

        if (data[_index].performCompleteEvent)
        {
            LeanTween.cancel(completeEventTweenIndex);
            completeEventTweenIndex = LeanTween.delayedCall(data[_index].delay + data[_index].duration, _f =>
            {
                customCompleteEvents[_index]?.Invoke();
            }).id;
        }
        
        currentTween = _index;
    }

    public TweenData CollectCurrentData(int _index)
    {
        CollectComponents();
        TweenData _tweenData = new TweenData();
        if (data != null && data.Count != 0)
        {
            _tweenData = data[_index];
        }

        if (rectTransform)
        {
            _tweenData.position = rectTransform.anchoredPosition; 
        }
        else
        {
            _tweenData.position = cTransform.localPosition; 
        }

        _tweenData.rotation = cTransform.localEulerAngles;
        _tweenData.scale = cTransform.localScale;

        if (spriteRenderer)
        {
            _tweenData.color = spriteRenderer.color;
        }

        if (image)
        {
            _tweenData.alpha = image.color.a;
        }

        if (canvasGroup)
        {
            _tweenData.alpha = canvasGroup.alpha;
        }

        return _tweenData;
    }

    public int GetCurrentTweenState()
    {
        return currentTween;
    }

    public void ApplyCurrentData(int _index)
    {
        CollectComponents();

        if (data[_index].move)
        {
            if (rectTransform)
            {
                rectTransform.anchoredPosition = data[_index].position;
            }
            else
            {
                cTransform.localPosition = data[_index].position;
            }
        }

        if (data[_index].changeScale)
        {
            cTransform.localScale = data[_index].scale; 
        }
        
        if (data[_index].rotate)
        {
            cTransform.localEulerAngles = data[_index].rotation;
        }
        
        if (spriteRenderer)
        {
            if (data[_index].changeColor)
            {
                spriteRenderer.color = data[_index].color;
            }
            
            if (data[_index].changeAlpha)
            {
                Color _color = spriteRenderer.color;
                _color = new Color(_color.r, _color.g, _color.b, data[_index].alpha);
                spriteRenderer.color = _color;
            }
        }

        if (image)
        {
            if (data[_index].changeColor)
            {
                image.color = data[_index].color;
            }
            
            if (data[_index].changeAlpha)
            {
                Color _color = image.color;
                _color = new Color(_color.r, _color.g, _color.b, data[_index].alpha);
                image.color = _color;
            }
        }

        if (canvasGroup && data[_index].changeAlpha)
        {
            canvasGroup.alpha = data[_index].alpha;
        }
    }

    public void CancelTween()
    {
        LeanTween.cancel(gameObject);
    }

    public bool IsObjectTweening()
    {
        return LeanTween.isTweening(gameObject);
    }

    void ChangeColor(int _index)
    {
        RectTransform _rt = GetComponent<RectTransform>();
        SpriteRenderer _sr = GetComponent<SpriteRenderer>();
        Image _image = GetComponent<Image>();

        if (_rt && _image)
        {
            if (_image.color != data[_index].color)
            {
                LeanTween.color(_rt, data[_index].color, data[_index].duration)
                    .setEase(data[_index].easeType)
                    .setDelay(data[_index].delay)
                    .setLoopCount(data[_index].loopCount);
            }
        }
        else if (_sr)
        {
            if (_sr.color != data[_index].color)
            {
                LeanTween.color(gameObject, data[_index].color, data[_index].duration)
                    .setEase(data[_index].easeType)
                    .setDelay(data[_index].delay)
                    .setLoopCount(data[_index].loopCount);
            }
        }
    }

    void ChangeAlpha(int _index)
    {
        if (canvasGroup)
        {
            if (Math.Abs(canvasGroup.alpha - data[_index].alpha) > TOLERANCE)
            {
               LeanTween.alphaCanvas(canvasGroup, data[_index].alpha, data[_index].duration)
                    .setEase(data[_index].easeType)
                    .setDelay(data[_index].delay)
                    .setLoopCount(data[_index].loopCount);
            }
        }

        if (rectTransform && image)
        {
            if (Math.Abs(image.color.a - data[_index].alpha) > TOLERANCE)
            {
                LeanTween.alpha(rectTransform, data[_index].alpha, data[_index].duration)
                    .setEase(data[_index].easeType)
                    .setDelay(data[_index].delay)
                    .setLoopCount(data[_index].loopCount);
            }
        }
        else if (spriteRenderer)
        {
            if (Math.Abs(spriteRenderer.color.a - data[_index].alpha) > TOLERANCE)
            {
                LeanTween.alpha(gameObject, data[_index].alpha, data[_index].duration)
                    .setEase(data[_index].easeType)
                    .setDelay(data[_index].delay)
                    .setLoopCount(data[_index].loopCount);
            }
        }
        else if (txt)
        {
            if (Math.Abs(txt.color.a - data[_index].alpha) > TOLERANCE)
            {
                LeanTween.textAlpha(rectTransform, data[_index].alpha, data[_index].duration)
                    .setEase(data[_index].easeType)
                    .setDelay(data[_index].delay)
                    .setLoopCount(data[_index].loopCount);
            }
        }
    }

    void Rotate(Vector3 _targetRotation, Vector3 _currentRotation, int _index)
    {
        if (_currentRotation != _targetRotation)
        {
            LeanTween.rotateLocal(gameObject, data[_index].rotation, data[_index].duration)
                .setEase(data[_index].easeType)
                .setDelay(data[_index].delay)
                .setLoopCount(data[_index].loopCount);
        }
    }

    void Scale(Vector3 _targetScale, Vector3 _currentScale, int _index)
    {
        if (_currentScale != _targetScale)
        {
            LeanTween.scale(gameObject, _targetScale, data[_index].duration)
                .setEase(data[_index].easeType)
                .setDelay(data[_index].delay)
                .setLoopCount(data[_index].loopCount);
        }
    }

    void Move(Vector3 _targetPosition, Vector3 _currentPosition, int _index)
    {
        if (_currentPosition != _targetPosition)
        {
            if (rectTransform)
            {
                LeanTween.move(rectTransform, _targetPosition, data[_index].duration)
                    .setEase(data[_index].easeType)
                    .setDelay(data[_index].delay)
                    .setLoopCount(data[_index].loopCount);
            }
            else
            {
                LeanTween.moveLocal(gameObject, _targetPosition, data[_index].duration)
                    .setEase(data[_index].easeType)
                    .setDelay(data[_index].delay)
                    .setLoopCount(data[_index].loopCount);
            }
        }
    }
}

[Serializable]
public class TweenData
{
    public bool performBeginEvent;
    public bool performCompleteEvent;
    public bool move;
    public bool rotate;
    public bool changeScale;
    public bool changeColor;
    public bool changeAlpha;
    public bool performOnEnable;
    public bool doNotWaitForComplete;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale = Vector3.one;
    public Color32 color = new Color32(255, 255, 255, 255);
    public float alpha;
    public float duration = 1f;
    public float delay;
    public int loopCount = 1;
    public LeanTweenType easeType = LeanTweenType.easeOutQuart;
}
