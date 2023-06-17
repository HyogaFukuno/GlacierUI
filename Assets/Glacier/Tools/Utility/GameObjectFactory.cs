namespace Glacier.Tools.Utility
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    //TODO: 生成周りに必要なメソッドを洗い出して最適化すること

    /// <summary>
    /// GameObjectの生成周りを扱うユーティリティクラス。
    /// </summary>
    public static class GameObjectFactory
    {
        #region methods

        /// <summary>
        /// 新たなGameObjectを生成する処理。
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static (GameObject gameObject, Transform transform, T component) CreateAndAddComponent<T>(GameObject prefab, string name) where T : Component
        {
            (GameObject gameObject, Transform transform) = GameObjectFactory.Create(prefab, name);
            T component = gameObject.AddComponent<T>();

            return (gameObject, transform, component);
        }

        /// <summary>
        /// 新たなGameObjectを生成する処理。
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static (GameObject gameObject, Transform transform, T component) CreateAndGetComponent<T>(GameObject prefab, string name) where T : Component
        {
            (GameObject gameObject, Transform transform) = GameObjectFactory.Create(prefab, name);
            T component = gameObject.GetComponent<T>();

            return (gameObject, transform, component);
        }

        /// <summary>
        /// 新たなGameObjectを生成する処理。
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public static (GameObject gameObject, Transform transform, T component) CreateAndAddComponent<T>(GameObject prefab) where T : Component
        {
            (GameObject gameObject, Transform transform) = GameObjectFactory.Create(prefab);
            T component = gameObject.AddComponent<T>();

            return (gameObject, transform, component);
        }

        /// <summary>
        /// 新たなGameObjectを生成する処理。
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public static (GameObject gameObject, Transform transform, T component) CreateAndGetComponent<T>(GameObject prefab) where T : Component
        {
            (GameObject gameObject, Transform transform) = GameObjectFactory.Create(prefab);
            T component = gameObject.GetComponent<T>();

            return (gameObject, transform, component);
        }

        /// <summary>
        /// 新たなGameObjectを生成する処理。
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static (GameObject gameObject, Transform transform, T component) CreateAndAddComponent<T>(string name) where T : Component
        {
            (GameObject gameObject, Transform transform) = GameObjectFactory.Create(name);
            T component = gameObject.AddComponent<T>();

            return (gameObject, transform, component);
        }

        /// <summary>
        /// 新たなGameObjectを生成する処理。
        /// </summary>
        /// <returns></returns>
        public static (GameObject gameObject, Transform transform, T component) CreateAndAddComponent<T>() where T : Component
        {
            (GameObject gameObject, Transform transform) = GameObjectFactory.Create();
            T component = gameObject.AddComponent<T>();

            return (gameObject, transform, component);
        }

        /// <summary>
        /// 新たなGameObjectを生成する処理。
        /// </summary>
        /// <returns></returns>
        public static (GameObject gameObject, Transform transform) Create(GameObject prefab, string name)
        {
            GameObject gameObject = GameObject.Instantiate(prefab);
            gameObject.name = name;

            Transform transform = gameObject.transform;

            return (gameObject, transform);
        }

        /// <summary>
        /// 新たなGameObjectを生成する処理。
        /// </summary>
        /// <returns></returns>
        public static (GameObject gameObject, Transform transform) Create(GameObject prefab)
        {
            GameObject gameObject = GameObject.Instantiate(prefab);
            Transform transform = gameObject.transform;

            return (gameObject, transform);
        }

        /// <summary>
        /// 新たなGameObjectを生成する処理。
        /// </summary>
        /// <returns></returns>
        public static (GameObject gameObject, Transform transform) Create(string name)
        {
            GameObject gameObject = new GameObject(name);
            Transform transform = gameObject.transform;

            return (gameObject, transform);
        }

        /// <summary>
        /// 新たなGameObjectを生成する処理。
        /// </summary>
        /// <returns></returns>
        public static (GameObject gameObject, Transform transform) Create()
        {
            GameObject gameObject = new GameObject();
            Transform transform = gameObject.transform;

            return (gameObject, transform);
        }

        #endregion
    }
}