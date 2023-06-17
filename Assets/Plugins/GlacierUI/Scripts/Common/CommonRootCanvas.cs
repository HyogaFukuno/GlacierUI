namespace GlacierUI.Common
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Glacier.Common.Camera;

    public class CommonRootCanvas : MonoBehaviour
    {
        #region variables

        [SerializeField] private Canvas _rootCanvas;

        #endregion

        #region methods

        /// <summary>
        /// 開始時に呼ばれる処理。
        /// </summary>
        protected virtual void Start()
        {
            _rootCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            _rootCanvas.worldCamera = CameraStatics.uiCamera;
        }

        #endregion
    }
}