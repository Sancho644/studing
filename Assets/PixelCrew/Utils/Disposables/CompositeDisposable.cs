using System;
using System.Collections.Generic;

namespace PixelCrew.Utils.Disposables
{
    public class CompositeDisposable : IDisposable
    {
        private readonly List<IDisposable> _disposable = new List<IDisposable>();

        public void Retain(IDisposable disposable)
        {
            _disposable.Add(disposable);
        }

        public void Dispose()
        {
            foreach (var disposable in _disposable)
            {
                disposable.Dispose();
            }

            _disposable.Clear();
        }      
    }
}