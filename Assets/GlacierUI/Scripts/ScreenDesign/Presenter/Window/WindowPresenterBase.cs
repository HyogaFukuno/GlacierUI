namespace GlacierUI.ScreenDesign.Presenter.Window
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using UnityEngine;
    using UnityEngine.Events;
    using Cysharp.Threading.Tasks;
    using GlacierUI.ScreenDesign.Presenter.Scene;
    using GlacierUI.ScreenDesign.Presenter.Screen;
    using Glacier.Tools.Addressables;
    using Glacier.Tools.Utility;


    public interface IWindowPresenter
    {
        UniTask<T> OpenScreen<T>() where T : ScreenPresenterBase;
        UniTask CloseScreen();
    }

    public abstract class WindowPresenterBase : MonoBehaviour, IWindowPresenter
    {
        #region variables

        private IScenePresenter _parentScene;

        private Stack<ScreenPresenterBase> _screens;

        #endregion

        #region properties

        public IScenePresenter parentScene => this._parentScene;

        #endregion

        #region methods

        /// <summary>
        /// 生成時に呼ばれる処理。
        /// </summary>
        protected virtual void Awake()
        {
            _screens = new Stack<ScreenPresenterBase>();
        }

        /// <summary>
        /// 破棄時に呼ばれる処理。
        /// </summary>
        protected virtual void Destroy()
        {
            _screens.Clear();
            _screens = null;
        }

        /// <summary>
        /// 初期化処理。
        /// </summary>
        /// <returns></returns>
        public virtual async UniTask Initialize(IScenePresenter parentScene)
        {
            _parentScene = parentScene;

            await this.OnInitialize(this.destroyCancellationToken);
        }

        /// <summary>
        /// 自身を開く際に呼ばれる処理。
        /// </summary>
        /// <returns></returns>
        public virtual async UniTask Open()
        {
            await this.OnOpen(this.destroyCancellationToken);
        }

        /// <summary>
        /// 自身を閉じる際に呼ばれる処理。
        /// </summary>
        /// <returns></returns>
        public virtual async UniTask Close()
        {
            await this.OnClose(this.destroyCancellationToken);
        }

        /// <summary>
        /// 他のウィンドウが閉じる事により自身を開く際に呼ばれる処理。
        /// </summary>
        /// <returns></returns>
        public virtual async UniTask Activate()
        {
            await this.OnActivate(this.destroyCancellationToken);
        }

        /// <summary>
        /// 他のウィンドウが開く事により自身を閉じる際に呼ばれる処理。
        /// </summary>
        /// <returns></returns>
        public virtual async UniTask Deactivate()
        {
            await this.OnDeactivate(this.destroyCancellationToken);
        }

        /// <summary>
        /// 指定のスクリーンを開く処理。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<T> OpenScreen<T>() where T : ScreenPresenterBase
        {
            string key = typeof(T).Name;
            return await OpenScreen<T>(key, this.destroyCancellationToken);
        }

        /// <summary>
        /// 指定のスクリーンを開く処理。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual async UniTask<T> OpenScreen<T>(string key, CancellationToken cancellationToken) where T : ScreenPresenterBase
        {
            // 開きたいスクリーンが既に存在している場合は例外を投げる
            if (null != _screens.Where(screen => (screen is T)).FirstOrDefault())
            {
                throw new System.Exception($"{typeof(T).Name} is already opened!");
            }

            // Addressablesから指定のScreenを読み込む
            var handle = await AddressableWrapper.LoadAssetAsync<GameObject>(key, cancellationToken);

            // 読み込みに失敗した場合は例外を投げる
            if (null == handle || null == handle.value)
            {
                throw new System.Exception($"Failed to load screen prefab!");
            }

            // 読み込んだプレハブを指定してScreenを生成する
            var (instance, transform, newScreen) = GameObjectFactory.CreateAndGetComponent<T>(handle.value, name: key);

            // 生成が完了した為、AddressableAssetを破棄する
            handle.Dispose();

            // 生成したScreenを自身の子に設定する
            transform.SetParent(this.transform, false);

            // 現在表示しているスクリーンが存在する場合は閉じる
            if (_screens.TryPeek(out var screen))
            {
                await screen.Deactivate();
            }

            // Screenの初期化処理を実行する
            await newScreen.Initialize(this);

            // Screenを開く
            await newScreen.Open();

            // Screenの処理が終了したのでStackにプッシュする
            _screens.Push(newScreen);

            return newScreen;
        }

        /// <summary>
        /// 先頭のスクリーンを閉じる処理。
        /// </summary>
        /// <returns></returns>
        public async UniTask CloseScreen()
        {
            await CloseScreen(this.destroyCancellationToken);
        }

        /// <summary>
        /// 先頭のスクリーンを閉じる処理。
        /// </summary>
        /// <returns></returns>
        protected virtual async UniTask CloseScreen(CancellationToken cancellationToken)
        {
            // 所有しているスクリーンの先頭を取得出来ない場合は以降の処理をしない
            if (!_screens.TryPop(out var screen))
            {
                throw new System.Exception($"Failed Stack.TryPop() screens is not stacked!");
            }

            // 先頭のスクリーンを非表示にする
            await screen.Close();

            // 無事にスクリーンを非表示にした為、破棄する
            Object.Destroy(screen.gameObject);
            screen = null;

            // 以前に表示していたスクリーンが存在する場合は
            // そのスクリーンが現在先頭に表示される為、開く
            if (_screens.TryPeek(out screen))
            {
                await screen.Activate();
            }
        }

        #region abstract methods

        /// <summary>
        /// 初期化処理。
        /// </summary>
        /// <returns></returns>
        protected abstract UniTask OnInitialize(CancellationToken cancellationToken);

        /// <summary>
        /// 自身を開く際に呼ばれる処理。
        /// </summary>
        /// <returns></returns>
        protected abstract UniTask OnOpen(CancellationToken cancellationToken);

        /// <summary>
        /// 自身を閉じる際に呼ばれる処理。
        /// </summary>
        /// <returns></returns>
        protected abstract UniTask OnClose(CancellationToken cancellationToken);

        /// <summary>
        /// 他のウィンドウが閉じる事により自身を開く際に呼ばれる処理。
        /// </summary>
        /// <returns></returns>
        protected abstract UniTask OnActivate(CancellationToken cancellationToken);

        /// <summary>
        /// 他のウィンドウが開く事により自身を閉じる際に呼ばれる処理。
        /// </summary>
        /// <returns></returns>
        protected abstract UniTask OnDeactivate(CancellationToken cancellationToken);

        #endregion

        #endregion
    }
}