namespace GlacierUI.ScreenDesign.Presenter.Screen
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using UnityEngine;
    using Cysharp.Threading.Tasks;
    using GlacierUI.ScreenDesign.Presenter.Window;

    public abstract class ScreenPresenterBase : MonoBehaviour
    {
        #region variables

        private IWindowPresenter _parentWindow;

        #endregion

        #region properties

        public IWindowPresenter parentWindow => this._parentWindow;

        #endregion

        #region methods

        /// <summary>
        /// 初期化処理。
        /// </summary>
        /// <returns></returns>
        public virtual async UniTask Initialize(IWindowPresenter parentWindow)
        {
            _parentWindow = parentWindow;

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
        /// 他のスクリーンが閉じる事により自身を開く際に呼ばれる処理。
        /// </summary>
        /// <returns></returns>
        public virtual async UniTask Activate()
        {
            await this.OnActivate(this.destroyCancellationToken);
        }

        /// <summary>
        /// 他のスクリーンが開く事により自身を閉じる際に呼ばれる処理。
        /// </summary>
        /// <returns></returns>
        public virtual async UniTask Deactivate()
        {
            await this.OnDeactivate(this.destroyCancellationToken);
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
        /// 他のスクリーンが閉じる事により自身を開く際に呼ばれる処理。
        /// </summary>
        /// <returns></returns>
        protected abstract UniTask OnActivate(CancellationToken cancellationToken);

        /// <summary>
        /// 他のスクリーンが開く事により自身を閉じる際に呼ばれる処理。
        /// </summary>
        /// <returns></returns>
        protected abstract UniTask OnDeactivate(CancellationToken cancellationToken);

        #endregion

        #endregion
    }
}