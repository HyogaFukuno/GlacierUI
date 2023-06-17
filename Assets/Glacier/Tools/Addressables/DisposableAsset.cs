namespace Glacier.Tools.Addressables
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IDisposableAsset<out T> : IDisposable
    {
        T value { get; }
    }

    public interface IDisposableAssets<T> : IDisposable
    {
        IList<T> value { get; }
    }

    /// <summary>
    /// Dispose可能なAddressables用アセットクラス。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DisposableAsset<T> : IDisposableAsset<T>
    {
        #region variables

        private readonly Action _disposedAction;

        #endregion

        #region properties

        public T value { get; }

        #endregion

        #region methods

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="disposedAction"></param>
        public DisposableAsset(T value, Action disposedAction)
        {
            this.value = value;
            this._disposedAction = disposedAction;
        }

        /// <summary>
        /// 破棄された際の処理。
        /// </summary>
        public void Dispose()
        {
            _disposedAction?.Invoke();
        }

        #endregion
    }

    /// <summary>
    /// Dispose可能なAddressables用アセットクラス。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DisposableAssets<T> : IDisposableAssets<T>
    {
        #region variables

        private readonly Action _disposedAction;

        #endregion

        #region properties

        public IList<T> value { get; }

        #endregion

        #region methods

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="disposedAction"></param>
        public DisposableAssets(IList<T> value, Action disposedAction)
        {
            this.value = value;
            this._disposedAction = disposedAction;
        }

        /// <summary>
        /// 破棄された際の処理。
        /// </summary>
        public void Dispose()
        {
            _disposedAction?.Invoke();
        }

        #endregion
    }
}