/*
AddressableWrapper

MIT License

Copyright (c) 2021 kyubuns

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

namespace Glacier.Tools.Addressables
{
    using System;
    using System.Threading;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using Cysharp.Threading.Tasks;

    /// <summary>
    /// Dispose可能なAddressablesアセットを読み込むためのラッパークラス。
    /// ・内部のアセット読み込み処理は IAddressableLoader を継承したクラスを作成することで
    /// 　読み込み方法を変更することが可能です。
    /// </summary>
    public static class AddressableWrapper
    {
        #region variables

        private static IAddressableLoader _loader;

        #endregion

        #region properties

        public static IAddressableLoader loader
        {
            get => _loader ?? (_loader = new DefaultAdressableLoader());
            set => _loader = value;
        }

        #endregion

        #region methods

        /// <summary>
        /// Dispose可能なAddressablesアセットを読み込む処理。
        /// </summary>
        /// <param name="key">読み込みたいアセットのキー</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        public static UniTask<IDisposableAsset<TObject>> LoadAssetAsync<TObject>(string key,
                                                                        CancellationToken cancellationToken)
                                                                        where TObject : UnityEngine.Object
        {
            return loader.LoadAssetAsync<TObject>(key, cancellationToken);
        }

        /// <summary>
        /// Dispose可能なAddressablesアセット達を読み込む処理。
        /// </summary>
        /// <param name="key">読み込みたいアセットのキー</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        public static UniTask<IDisposableAssets<TObject>> LoadAssetsAsync<TObject>(string key,
                                                                        CancellationToken cancellationToken)
                                                                        where TObject : UnityEngine.Object
        {
            return loader.LoadAssetsAsync<TObject>(key, cancellationToken);
        }

        #endregion
    }
}