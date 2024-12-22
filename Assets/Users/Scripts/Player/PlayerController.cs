using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    Movement m_Movement;
    Collison m_Collison;
    SetAnimation m_SetAnimation;
    PlayerSpriteRenderer m_PlayerSpriteRenderer;

    [SerializeField] RectTransform joyPanel;
    [SerializeField] RectTransform joyStick;
    [SerializeField] RectTransform[] joys;
    [SerializeField] Transform directionArrow;

    [SerializeField] LayerMask enemyLayer;

    Camera mainCamera;

    Touch touch;
    Vector2 inputTouchPos;
    Vector2 startTouchPos;
    Vector2 endTouchPos;

    bool isTouchDown;

    public float maxPower;
    float angle, power, fill;

    float dragTimer = 0;

    void Start()
    {
        m_Movement = GetComponent<Movement>();
        m_Collison = GetComponent<Collison>();
        m_SetAnimation = GetComponent<SetAnimation>();
        m_PlayerSpriteRenderer = GetComponent<PlayerSpriteRenderer>();
        mainCamera = Camera.main;
    }
    void Update()
    {
        if (!GameManager.Inst.onPlay) return;
        Dragment();
    }
    
    #region Drag
    void Dragment()
    {
        var isTouch = Input.GetMouseButton(0);

        if (m_Movement.count <= 0 && m_Collison.onCollision)
        {
            dragTimer += Time.deltaTime;
            if (dragTimer < 0.15f)
                return;

            if (TutorialManager.Inst && TutorialManager.Inst.isWaitTrigger)
            {
                m_SetAnimation.Ready(false);
                isTouchDown = false;
                SetUIActive(false);
                return;
            }

            if (isTouch)
            {
                var camZ = -Camera.main.transform.position.z;
                inputTouchPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camZ));

                if (!isTouchDown && !EventSystem.current.IsPointerOverGameObject())
                {
                    UIManager.Inst.SwapUI(true, 0.4f);
                    SetUIActive(true);
                    m_SetAnimation.Ready(true);
                    startTouchPos = inputTouchPos;
                    isTouchDown = true;

                    SetDistance();
                    SetJoyArrow();

                    var isFlipX = m_Collison.onDown || m_Collison.onUp;
                    m_PlayerSpriteRenderer.SetVectorFlip(startTouchPos, endTouchPos, isFlipX);
                }
                else if(isTouchDown)
                {
                    SetDistance();
                    SetJoyArrow();

                    var isFlipX = m_Collison.onDown || m_Collison.onUp;
                    m_PlayerSpriteRenderer.SetVectorFlip(startTouchPos, endTouchPos, isFlipX);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (maglen <= 0.4f)
                {
                    UIManager.Inst.SwapUI(false, 0.4f);
                    SetUIActive(false);
                    isTouchDown = false;
                    m_SetAnimation.Ready(false);
                }
                else
                {
                    m_Movement.Dash(power, angle);
                    SetUIActive(false);
                    isTouchDown = false;
                    m_SetAnimation.Ready(false);
                }
            }
        }
        else
        {
            dragTimer = 0;
        }
    }
    float maglen = 0;
    void SetDistance()
    {
        endTouchPos = inputTouchPos;

        Vector3 offset = startTouchPos - endTouchPos;
        maglen = offset.magnitude;
        power = (int)((maglen - 0.4f) * maxPower);
        if (power > maxPower)
            power = maxPower;

        UIManager.Inst.SetPower(power);

        SetAngle();
    }
    void SetAngle()
    {
        angle = Mathf.Atan2(endTouchPos.y - startTouchPos.y, endTouchPos.x - startTouchPos.x) * Mathf.Rad2Deg;

        if (m_Collison.onDown && m_Collison.onLeft)
        {
            angle = angle <= -177 && angle >= 0 ? -177 : angle;
            angle = angle >= -93 && angle < 0 ? -93 : angle;
        }
        else if (m_Collison.onDown && m_Collison.onRight)
        {
            angle = angle >= -3 && angle <= 180 ? -3 : angle;
            angle = angle <= -87 && angle > -180 ? -87 : angle;
        }
        else if (m_Collison.onUp && m_Collison.onLeft)
        {
            angle = angle <= 93 && angle >= -90 ? 93 : angle;
            angle = angle >= 177 || angle < -90 ? 177 : angle;
        }
        else if (m_Collison.onUp && m_Collison.onRight)
        {
            angle = angle >= 87 && angle <= -135 ? 87 : angle;
            angle = angle <= 3 && angle > -135 ? 3 : angle;
        }
        else if (m_Collison.onUp)
        {
            angle = angle <= 3 && angle >= -90 ? 3 : angle;
            angle = angle >= 177 || angle < -90 ? 177 : angle;
        }
        else if (m_Collison.onDown)
        {
            angle = angle >= -3 && angle <= 90 ? -3 : angle;
            angle = angle <= -177 || angle > 90 ? -177 : angle;
        }
        else if (m_Collison.onLeft)
        {
            angle = angle <= 93 && angle >= 0 ? 93 : angle;
            angle = angle >= -93 && angle < 0 ? -93 : angle;
        }
        else if (m_Collison.onRight)
        {
            angle = angle >= 87 && angle <= 180 ? 87 : angle;
            angle = angle <= -87 && angle > -180 ? -87 : angle;
        }
    }
    void SetJoyArrow()
    {
        var inputDir = endTouchPos - startTouchPos;
        var inputMag = inputDir.magnitude;

        directionArrow.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        directionArrow.transform.localScale = new Vector3(1, maglen <= 0.4f ? 0 : (inputMag <= 1.501f ? inputMag : 1.5f) * 2);

        var clampedDir = inputMag <= 1.5f ? inputDir : inputDir.normalized * 1.501f;
        joyPanel.position = startTouchPos;
        joyStick.position = clampedDir + startTouchPos;

        var dividedIndex = 1f / joys.Length;
        for (int i = 0; i < joys.Length; i++)
        {
            joys[i].position = Vector2.Lerp(startTouchPos, clampedDir + startTouchPos, i * dividedIndex);
        }

        var ray = m_Collison.rayOffset;
        for (int i = 0; i < ray.Length; i++)
        {
            var hit = Physics2D.Raycast(transform.position + ray[i] * 0.95f, -inputDir.normalized, 100, enemyLayer);
            if (hit)
                hit.transform.GetComponent<EnemySprite>().HitRay();
        }
    }
    void SetUIActive(bool value)
    {
        joyPanel.gameObject.SetActive(value);
        directionArrow.gameObject.SetActive(value);
    }
    #endregion

    public bool IsPointOverAndroid()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Ended)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
