using JiufenGames.MineSweeperAlike.InputModule;
using JiufenPackages.ServiceLocator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{
    public class CameraZoomController : MonoBehaviour
    {
        #region ----Fields----
        public bool isMoving;
        public bool isZooming;

        private RectTransform boardTransform;

        private MineSweeperALikeInputs inputs;
        private Coroutine checkZoomCoroutine;
        private Coroutine checkMovementCoroutine;

        private Vector2 prevPrimaryTouch = Vector2.zero, initSecondaryTouch = Vector2.zero;
        private float previousDistance = 0f;

        private Vector2 initBoardPosition;
        private Vector2 boardSize;
        #endregion ----Fields----

        #region ----Methods----
        public void Init(RectTransform _boardTransform)
        {
            boardTransform = _boardTransform;
            initBoardPosition = boardTransform.anchoredPosition;
            Vector2 tempSize = boardTransform.GetChild(0).GetComponent<RectTransform>().rect.size;
            boardSize = new Vector2(tempSize.x * .0625f, tempSize.y * .125f);
            inputs = ServiceLocator.m_Instance.GetService<IInputManager>().inputs;

            inputs.UI.SecondaryTouchContact.started += TwoFingersTouchStart;
            inputs.UI.SecondaryTouchContact.canceled += TwoFingerTouchEnd;

            inputs.UI.Click.started += MovementStart;
            inputs.UI.Click.canceled += MovementEnd;
        }
        private void OnDisable()
        {
            inputs.UI.SecondaryTouchContact.started -= TwoFingersTouchStart;
            inputs.UI.SecondaryTouchContact.canceled -= TwoFingerTouchEnd;

            inputs.UI.Click.started -= MovementStart;
            inputs.UI.Click.canceled -= MovementEnd;
        }

        public void MovementStart(CallbackContext context) { checkMovementCoroutine = StartCoroutine(CheckMovement()); }
        public void MovementEnd(CallbackContext context)
        {
            if (checkMovementCoroutine != null)
                StopCoroutine(checkMovementCoroutine);
            isMoving = false;
        }

        IEnumerator CheckMovement()
        {
            prevPrimaryTouch = inputs.UI.PrimaryTouch.ReadValue<Vector2>();
            while (true)
            {
                Vector2 primaryTouch = inputs.UI.PrimaryTouch.ReadValue<Vector2>();
                float distance = Vector2.Distance(primaryTouch, prevPrimaryTouch);

                Vector2 differenceInitCurrentAnchor = (boardTransform.anchoredPosition + ((primaryTouch - prevPrimaryTouch))) - initBoardPosition;

                if (distance > 3 && distance < 100)
                {
                    Debug.Log("Hello" + distance);
                    if (differenceInitCurrentAnchor.x > -boardSize.x && differenceInitCurrentAnchor.x < boardSize.x)
                    {
                        boardTransform.anchoredPosition += new Vector2((primaryTouch - prevPrimaryTouch).x, 0);
                        boardTransform.pivot -= new Vector2((primaryTouch - prevPrimaryTouch).x, 0) * (.5f / boardSize.x);
                        isMoving = true;
                    }
                    if (differenceInitCurrentAnchor.y > -boardSize.y && differenceInitCurrentAnchor.y < boardSize.y)
                    {
                        boardTransform.anchoredPosition += new Vector2(0, (primaryTouch - prevPrimaryTouch).y);
                        boardTransform.pivot -= new Vector2(0, (primaryTouch - prevPrimaryTouch).y) * (.5f / boardSize.y);
                        isMoving = true;
                    }
                }

                prevPrimaryTouch = primaryTouch;
                yield return null;
            }
        }

        private void TwoFingersTouchStart(CallbackContext context) { checkZoomCoroutine = StartCoroutine(CheckZoom()); }
        private void TwoFingerTouchEnd(CallbackContext context)
        {
            if (checkZoomCoroutine != null)
                StopCoroutine(checkZoomCoroutine);
            isZooming = false;
        }

        IEnumerator CheckZoom()
        {
            prevPrimaryTouch = inputs.UI.PrimaryTouch.ReadValue<Vector2>();
            initSecondaryTouch = inputs.UI.SecondaryTouch.ReadValue<Vector2>();
            previousDistance = Vector2.Distance(inputs.UI.PrimaryTouch.ReadValue<Vector2>(), inputs.UI.SecondaryTouch.ReadValue<Vector2>());

            while (true)
            {
                Vector2 primaryTouch = inputs.UI.PrimaryTouch.ReadValue<Vector2>();
                Vector2 secondaryTouch = inputs.UI.SecondaryTouch.ReadValue<Vector2>();

                Vector2 deltaPrimaryTouch = primaryTouch - prevPrimaryTouch;
                Vector2 deltaSecondaryTouch = secondaryTouch - initSecondaryTouch;

                float dotProductPrimarySecondary = Vector2.Dot(deltaPrimaryTouch, deltaSecondaryTouch);

                if (dotProductPrimarySecondary < 0.3f)
                {
                    float distance = Vector2.Distance(inputs.UI.PrimaryTouch.ReadValue<Vector2>(), inputs.UI.SecondaryTouch.ReadValue<Vector2>());
                    float distanceFactor = distance / previousDistance;
                    if ((distanceFactor > 1.0002f || distanceFactor < .9992f) && boardTransform.localScale.magnitude > 0.01f)
                    {
                        boardTransform.localScale *= distanceFactor;
                        isZooming = true;
                    }

                }
                previousDistance = Vector2.Distance(inputs.UI.PrimaryTouch.ReadValue<Vector2>(), inputs.UI.SecondaryTouch.ReadValue<Vector2>());
                prevPrimaryTouch = inputs.UI.PrimaryTouch.ReadValue<Vector2>();
                initSecondaryTouch = inputs.UI.SecondaryTouch.ReadValue<Vector2>();
                yield return null;
            }
        }
        #endregion ----Methods----
    }
}
