using System;

namespace Features.Interaction.Helpers
{
    public sealed class InteractionSubscriptionDisposable : IDisposable
    {
        private readonly Action _onDispose;
        private          bool   _isDisposed;

        public InteractionSubscriptionDisposable(Action onDispose)
        {
            _onDispose = onDispose ?? throw new ArgumentNullException(nameof(onDispose));
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            _onDispose();
        }
    }
}
