//
//  TouchCameraMove.cs
//  TundraUtils
//
//  Created on 01/04/2017.
//  Copyright © 2017 Tundra Mobile. All rights reserved.
//
using UnityEngine;
using UnityEngine.EventSystems;


namespace TundraUtils.Behaviours.CameraUtils
{
    public class TouchCameraMove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        #region Private

        private Vector3 _touchStarted;
        private Vector3 _origin;
        private Vector3 _prevFrame;

        private bool _pressed = false;
        private float _currentSizeDelta = 0;
        private float _maxSize = 1.5f;
        private float _minSize = 0.6f;
        private float _defaultSize = 5;

        #endregion

        public const float MoveScale = 1;
        public float WheelScale = 3;
        public Camera UICamera;

        private bool TouchStarted
        {
            get
            {
                return _pressed;
            }
        }

        #region Monobehaviour

        private void Start()
        {
            _origin = transform.position;
            _defaultSize = Camera.main.orthographicSize;
            _maxSize = _maxSize * _defaultSize;
            _minSize = _minSize * _defaultSize;
        }

        private void LateUpdate()
        {
            if (TouchStarted)
            {
                if (Input.touchCount >= 2)
                {
                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                    float size = Camera.main.orthographicSize;
                    size += deltaMagnitudeDiff * 0.04f;

                    size = Mathf.Clamp(size, _minSize, _maxSize);
                    Camera.main.orthographicSize = size;
                }
                else
                {
                    Vector3 currentPos = UICamera.TouchPosition();

                    float z = Camera.main.transform.position.z;

                    Vector3 delta = _prevFrame - currentPos;
                    Camera.main.transform.position += delta * MoveScale;
                    _prevFrame = currentPos;
                }
            }

            var d = Input.GetAxis("Mouse ScrollWheel");
            float sz = Camera.main.orthographicSize;
            sz += d * WheelScale;
            sz = Mathf.Clamp(sz, _minSize, _maxSize);
            Camera.main.orthographicSize = sz;
        }

        #endregion

        #region EventSystems

        public void OnPointerDown(PointerEventData eventData)
        {
            _touchStarted = UICamera.TouchPosition();
            _origin = Camera.main.transform.position;

            _prevFrame = _touchStarted;
            _pressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pressed = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        #endregion
    }
}
