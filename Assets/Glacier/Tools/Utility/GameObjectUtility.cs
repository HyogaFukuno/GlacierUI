namespace Glacier.Tools.Utility
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// GameObjectを扱うユーティリティクラス。
    /// </summary>
    public static class GameObjectUtility
    {
        #region methods

        /// <summary>
        /// 指定したGameObjectが持つTranformと、指定のComponentを取得する処理。
        /// </summary>
        /// <param name="gameObject">情報を取得したいGameObjectのインスタンス</param>
        /// <typeparam name="T">取得したいコンポーネント</typeparam>
        /// <returns></returns>
        public static (Transform transform, T component) Get<T>(GameObject gameObject) where T : Component
        {
            Transform transform = gameObject.transform;
            T component = gameObject.GetComponent<T>();

            return (transform, component);
        }

        #endregion
    }
}