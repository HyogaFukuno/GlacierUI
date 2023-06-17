namespace Glacier.Tools.Addressables
{
    using System;
    using System.Threading;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using Cysharp.Threading.Tasks;

    public interface IAddressableLoader
    {
        UniTask<IDisposableAsset<TObject>> LoadAssetAsync<TObject>(string key,
                                                                   CancellationToken cancellationToken)
                                                                   where TObject : UnityEngine.Object;

        UniTask<IDisposableAssets<TObject>> LoadAssetsAsync<TObject>(string key,
                                                                   CancellationToken cancellationToken)
                                                                   where TObject : UnityEngine.Object;
    }

    /// <summary>
    /// デフォルトで使用されるAddressables読み込みクラス。
    /// </summary>
    public class DefaultAdressableLoader : IAddressableLoader
    {
        #region methods

        /// <summary>
        /// Dispose可能なAddressablesアセットを読み込む処理。
        /// </summary>
        /// <param name="key">読み込みたいアセットのキー</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        public async UniTask<IDisposableAsset<TObject>> LoadAssetAsync<TObject>(string key,
                                                                                CancellationToken cancellationToken)
                                                                                where TObject : UnityEngine.Object
        {
            var handle = Addressables.LoadAssetAsync<TObject>(key);
            await handle.ToUniTask(cancellationToken: cancellationToken);

            if (AsyncOperationStatus.Succeeded != handle.Status)
            {
                throw new Exception($"Failed to asset load! Key: {key}, Status: {handle.Status}");
            }

            return new DisposableAsset<TObject>(handle.Result, () => Addressables.Release(handle));
        }

        /// <summary>
        /// Dispose可能なAddressablesアセットを読み込む処理。
        /// </summary>
        /// <param name="key">読み込みたいアセットのキー</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        public async UniTask<IDisposableAssets<TObject>> LoadAssetsAsync<TObject>(string key,
                                                                                CancellationToken cancellationToken)
                                                                                where TObject : UnityEngine.Object
        {
            var handle = Addressables.LoadAssetsAsync<TObject>(key, null);
            await handle.ToUniTask(cancellationToken: cancellationToken);

            if (AsyncOperationStatus.Succeeded != handle.Status)
            {
                throw new Exception($"Failed to asset load! Key: {key}, Status: {handle.Status}");
            }

            return new DisposableAssets<TObject>(handle.Result, () => Addressables.Release(handle));
        }

        #endregion

    }
}