namespace GlacierUI.ScreenDesign.Presenter.Scene
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using UnityEngine;
    using Cysharp.Threading.Tasks;
    using GlacierUI.ScreenDesign.Presenter.Window;
    using Glacier.Tools.Addressables;
    using Glacier.Tools.Utility;


    public interface IScenePresenter
    {
        UniTask<T> OpenWindow<T>() where T : WindowPresenterBase;
        UniTask CloseWindow();
    }

    public abstract class ScenePresenterBase : MonoBehaviour, IScenePresenter
    {
        #region variables

        private Stack<WindowPresenterBase> _windows;

        #endregion

        #region methods

        /// <summary>
        /// 生成時に呼ばれる処理。
        /// </summary>
        protected virtual void Awake()
        {
            _windows = new Stack<WindowPresenterBase>();
        }

        /// <summary>
        /// 破棄時に呼ばれる処理。
        /// </summary>
        protected virtual void Destroy()
        {
            _windows.Clear();
            _windows = null;
        }

        /// <summary>
        /// 開始時に呼ばれる処理。
        /// </summary>
        protected virtual void Start()
        {
            this.Initialize().Forget();
        }

        /// <summary>
        /// 初期化処理。
        /// </summary>
        /// <returns></returns>
        public virtual async UniTask Initialize()
        {
            await this.OnInitialize(this.destroyCancellationToken);
        }

        /// <summary>
        /// 指定のウィンドウを開く処理。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<T> OpenWindow<T>() where T : WindowPresenterBase
        {
            string key = typeof(T).Name;
            return await OpenWindow<T>(key, this.destroyCancellationToken);
        }

        /// <summary>
        /// 指定のウィンドウを開く処理。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual async UniTask<T> OpenWindow<T>(string key, CancellationToken cancellationToken) where T : WindowPresenterBase
        {
            // 開きたいスクリーンが既に存在している場合は例外を投げる
            if (null != _windows.Where(window => (window is T)).FirstOrDefault())
            {
                throw new System.Exception($"{typeof(T).Name} is already opened!");
            }

            // Addressablesから指定のWindowを読み込む
            var handle = await AddressableWrapper.LoadAssetAsync<GameObject>(key, cancellationToken);

            // 読み込みに失敗した場合は例外を投げる
            if (null == handle || null == handle.value)
            {
                throw new System.Exception($"Failed to load window prefab!");
            }

            // 読み込んだプレハブを指定してWindowを生成する
            var (instance, transform, window) = GameObjectFactory.CreateAndGetComponent<T>(handle.value, name: key);

            // 生成が完了した為、AddressableAssetを破棄する
            handle.Dispose();

            // 生成したWindowを自身の子に設定する
            transform.SetParent(this.transform, false);

            // Windowの初期化処理を実行する
            await window.Initialize(this);

            // Windowを開く
            await window.Open();

            // Windowの処理が終了したのでStackにプッシュする
            _windows.Push(window);

            return window;
        }

        /// <summary>
        /// 先頭のウィンドウを閉じる処理。
        /// </summary>
        /// <returns></returns>
        public async UniTask CloseWindow()
        {
            await CloseWindow(this.destroyCancellationToken);
        }

        /// <summary>
        /// 先頭のウィンドウを閉じる処理。
        /// </summary>
        /// <returns></returns>
        protected virtual async UniTask CloseWindow(CancellationToken cancellationToken)
        {
            // 所有しているスクリーンの先頭を取得出来ない場合は以降の処理をしない
            if (!_windows.TryPop(out var window))
            {
                throw new System.Exception($"Failed Stack.TryPop() screens is not stacked!");
            }

            // 先頭のスクリーンを非表示にする
            await window.Close();

            // 無事にスクリーンを非表示にした為、破棄する
            Object.Destroy(window.gameObject);
            window = null;

            // 以前に表示していたスクリーンが存在する場合は
            // そのスクリーンが現在先頭に表示される為、開く
            if (_windows.TryPeek(out window))
            {
                await window.Activate();
            }
        }

        /// <summary>
        /// 初期化処理。
        /// </summary>
        /// <returns></returns>
        protected abstract UniTask OnInitialize(CancellationToken cancellationToken);

        #endregion
    }
}