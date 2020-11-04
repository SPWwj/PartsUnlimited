using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartsUnlimited.Client
{
    public class ShoppingCartNotificationService
    {
        private List<Func<Task>> _subscriptions = new();

        public IDisposable Register(Func<Task> subscriber)
        {
            _subscriptions.Add(subscriber);

            return new Subscription(() => _subscriptions.Remove(subscriber));
        }

        public Task OnItemRemoved()
        {
            return Task.WhenAll(_subscriptions.Select(s => s()));
        }

        private class Subscription : IDisposable
        {
            private Func<bool> _unsubscribe;

            public Subscription(Func<bool> unsubscribe)
            {
                _unsubscribe = unsubscribe;
            }

            public void Dispose()
            {
                _unsubscribe();
            }
        }
    }
}
