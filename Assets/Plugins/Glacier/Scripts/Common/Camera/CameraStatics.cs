namespace Glacier.Common.Camera
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class CameraStatics
    {
        #region variables

        private static Camera _mainCamera;
        private static Camera _uiCamera;

        #endregion

        #region properties

        public static Camera mainCamera
        {
            get
            {
                if (null == _mainCamera)
                {
                    _mainCamera = GameObject.FindWithTag("MainCamera")?.GetComponent<Camera>();
                }

                return _mainCamera;
            }
        }

        public static Camera uiCamera
        {
            get
            {
                if (null == _uiCamera)
                {
                    _uiCamera = GameObject.FindWithTag("UICamera")?.GetComponent<Camera>();
                }

                return _uiCamera;
            }
        }

        #endregion
    }
}