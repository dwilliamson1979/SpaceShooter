using System;
using System.Threading;
using UnityEngine;

namespace com.dhcc.framework
{
    public class AsyncTimer
    {
        private System.Action callback;
        private float interval;
        private bool loop;
        private float firstDelay;

        private bool isRunning;

        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

        public AsyncTimer(System.Action callback, float interval, bool loop = false, float firstDelay = 0)
        {
            this.callback = callback;
            this.interval = interval;
            this.loop = loop;
            this.firstDelay = firstDelay;
            
            cancellationTokenSource = new();
            cancellationToken = cancellationTokenSource.Token;
        }

        public void Reset(System.Action callback, float interval, bool loop = false, float firstDelay = 0)
        {
            Stop();

            this.callback = callback;
            this.interval = interval;
            this.loop = loop;
            this.firstDelay = firstDelay;            
        }

        public async void Start()
        {
            isRunning = true;

            try
            {
                if (!loop)
                {
                    await Awaitable.WaitForSecondsAsync(interval, cancellationToken);
                    callback?.Invoke();
                }
                else
                {
                    await Awaitable.WaitForSecondsAsync(firstDelay, cancellationToken);
                    callback?.Invoke();

                    while (isRunning)
                    {
                        await Awaitable.WaitForSecondsAsync(interval, cancellationToken);
                        callback?.Invoke();
                    }
                }
            }
            catch(OperationCanceledException ex)
            {
                Debug.Log($"AsyncTimer cancelled: {ex.Message}");
            }
            finally
            {
                isRunning = false;
            }
        }

        public void Stop()
        {
            if(cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
                cancellationTokenSource.Cancel();
        }
    }

    public class AsyncTimer<T>
    {
        private System.Action<T> callback;
        private T payload;
        private float interval;
        private bool loop;
        private float firstDelay;

        private bool isRunning;

        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

        public AsyncTimer(System.Action<T> callback, T payload, float interval, bool loop = false, float firstDelay = 0)
        {
            this.callback = callback;
            this.payload = payload;
            this.interval = interval;
            this.loop = loop;
            this.firstDelay = firstDelay;

            cancellationTokenSource = new();
            cancellationToken = cancellationTokenSource.Token;
        }

        public void Reset(System.Action<T> callback, float interval, bool loop = false, float firstDelay = 0)
        {
            Stop();

            this.callback = callback;
            this.interval = interval;
            this.loop = loop;
            this.firstDelay = firstDelay;
        }

        public async void Start()
        {
            isRunning = true;

            try
            {
                if (!loop)
                {
                    await Awaitable.WaitForSecondsAsync(interval, cancellationToken);
                    callback?.Invoke(payload);
                }
                else
                {
                    await Awaitable.WaitForSecondsAsync(firstDelay, cancellationToken);
                    callback?.Invoke(payload);

                    while (isRunning)
                    {
                        await Awaitable.WaitForSecondsAsync(interval, cancellationToken);
                        callback?.Invoke(payload);
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                Debug.Log($"AsyncTimer cancelled: {ex.Message}");
            }
            finally
            {
                isRunning = false;
            }
        }

        public void Stop()
        {
            if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
                cancellationTokenSource.Cancel();
        }
    }
}