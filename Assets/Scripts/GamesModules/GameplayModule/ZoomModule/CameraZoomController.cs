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

        [SerializeField, Range(.1f, 5f)] private float movementSpeed = 1f;
        #endregion ----Fields----

        #region ----Methods----
        public void Init(RectTransform _boardTransform)
        {
            boardTransform = _boardTransform;
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

                if (distance > 3 && distance < 100)
                {
                    float addPivotX = (primaryTouch - prevPrimaryTouch).x * (.0005f * movementSpeed * (1 / boardTransform.localScale.x));
                    if (boardTransform.pivot.x - addPivotX < 0)
                        boardTransform.pivot = new Vector2(0, boardTransform.pivot.y);
                    else if (boardTransform.pivot.x - addPivotX > 1)
                        boardTransform.pivot = new Vector2(1, boardTransform.pivot.y);
                    else
                        boardTransform.pivot -= new Vector2(addPivotX, 0);

                    float addPivotY = (primaryTouch - prevPrimaryTouch).y * (.0005f * movementSpeed * (1 / boardTransform.localScale.y));
                    if (boardTransform.pivot.y - addPivotY < 0)
                        boardTransform.pivot = new Vector2(boardTransform.pivot.x, 0);
                    else if (boardTransform.pivot.y - addPivotY > 1)
                        boardTransform.pivot = new Vector2(boardTransform.pivot.x, 1);
                    else
                        boardTransform.pivot -= new Vector2(0, addPivotY);

                    isMoving = true;
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
                    if (distanceFactor > 1.0002f || distanceFactor < .9998f)
                    {
                        if (boardTransform.localScale.x * distanceFactor < 1)
                            boardTransform.localScale = Vector2.one;
                        else if (boardTransform.localScale.x * distanceFactor > 4)
                            boardTransform.localScale = Vector2.one * 4;
                        else
                        {
                            boardTransform.localScale *= distanceFactor;
                            isZooming = true;
                        }
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
