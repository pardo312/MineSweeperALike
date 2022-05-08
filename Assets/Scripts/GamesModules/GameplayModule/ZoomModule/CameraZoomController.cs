using JiufenGames.MineSweeperAlike.InputModule;
using JiufenPackages.ServiceLocator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{
    public class CameraZoomController : MonoBehaviour
    {
        #region ----Fields----
        private RectTransform boardTransform;

        private MineSweeperALikeInputs inputs;
        private Coroutine checkZoomCoroutine;

        private Vector2 initPrimaryTouch = Vector2.zero, initSecondaryTouch = Vector2.zero;
        private float previousDistance = 0f;
        public bool isZooming;
        #endregion ----Fields----

        #region ----Methods----
        public void Init(RectTransform _boardTransform)
        {
            boardTransform = _boardTransform;
            inputs = ServiceLocator.m_Instance.GetService<IInputManager>().inputs;

            //inputs.UI.SecondaryTouchContact.started += _ => checkZoomCoroutine = StartCoroutine(CheckZoom());
            //inputs.UI.SecondaryTouchContact.canceled += _ => StopCoroutine(checkZoomCoroutine);

            inputs.UI.Click.started += _ => MovementStart();
            inputs.UI.Click.canceled += _ => MovementEnd();
        }
        private void OnDisable()
        {
            //inputs.UI.SecondaryTouchContact.started -= _ => TwoFingersTouchStart();
            //inputs.UI.SecondaryTouchContact.canceled -= _ => TwoFingerTouchEnd();
        }
        public void MovementStart()
        {
            checkZoomCoroutine = StartCoroutine(CheckMovement());
        }

        public void MovementEnd()
        {
            StopCoroutine(checkZoomCoroutine);
        }

        IEnumerator CheckMovement()
        {
            initPrimaryTouch = inputs.UI.PrimaryTouch.ReadValue<Vector2>();
            while (true)
            {
                Vector2 primaryTouch = inputs.UI.PrimaryTouch.ReadValue<Vector2>();
                if (Vector2.Distance(primaryTouch, initPrimaryTouch) > .5f)
                    boardTransform.position += (Vector3)(primaryTouch - initPrimaryTouch) ;

                initPrimaryTouch = inputs.UI.PrimaryTouch.ReadValue<Vector2>();
                yield return null;
            }
        }

        IEnumerator CheckZoom()
        {
            initPrimaryTouch = inputs.UI.PrimaryTouch.ReadValue<Vector2>();
            initSecondaryTouch = inputs.UI.SecondaryTouch.ReadValue<Vector2>();
            previousDistance = Vector2.Distance(inputs.UI.PrimaryTouch.ReadValue<Vector2>(), inputs.UI.SecondaryTouch.ReadValue<Vector2>());

            while (true)
            {
                Vector2 primaryTouch = inputs.UI.PrimaryTouch.ReadValue<Vector2>();
                Vector2 secondaryTouch = inputs.UI.SecondaryTouch.ReadValue<Vector2>();

                //---ZOOM DETECTION---
                float distance = Vector2.Distance(inputs.UI.PrimaryTouch.ReadValue<Vector2>(), inputs.UI.SecondaryTouch.ReadValue<Vector2>());

                //---MOVE DETECTION---
                Vector2 deltaPrimaryTouch = primaryTouch - initPrimaryTouch;
                Vector2 deltaSecondaryTouch = secondaryTouch - initSecondaryTouch;

                float dotProductPrimarySecondary = Vector2.Dot(deltaPrimaryTouch, deltaSecondaryTouch);

                //Moving same directions
                if (dotProductPrimarySecondary > 0.7f)
                {
                    boardTransform.position += (Vector3)(primaryTouch - initPrimaryTouch);
                }
                else if (dotProductPrimarySecondary < 0.3f)
                {
                    float distanceFactor = distance / previousDistance;
                    if (distanceFactor > 1.0002f || distanceFactor < .9992f)
                        boardTransform.localScale *= (distanceFactor /** 0.9999f*/);

                }
                previousDistance = Vector2.Distance(inputs.UI.PrimaryTouch.ReadValue<Vector2>(), inputs.UI.SecondaryTouch.ReadValue<Vector2>());
                initPrimaryTouch = inputs.UI.PrimaryTouch.ReadValue<Vector2>();
                initSecondaryTouch = inputs.UI.SecondaryTouch.ReadValue<Vector2>();
                yield return null;
            }
        }
        #endregion ----Methods----
    }
}
