namespace Glacier.Common.MonoBehaviour
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// 各プロパティのキャッシュや独自の処理を実装したMonoBehaviourクラス。
    /// </summary>
    public class GlacierBehaviour : MonoBehaviour
    {
        #region variables

        private GameObject _gameObjectCache;

        private Transform _transformCache;

        #endregion

        #region properties

        public new GameObject gameObject
        {
            get
            {
                if (null == _gameObjectCache)
                {
                    _gameObjectCache = base.gameObject;
                }

                return _gameObjectCache;
            }
        }

        public new Transform transform
        {
            get
            {
                if (null == _transformCache)
                {
                    _transformCache = base.transform;
                }

                return _transformCache;
            }
        }

        #endregion
    }
}