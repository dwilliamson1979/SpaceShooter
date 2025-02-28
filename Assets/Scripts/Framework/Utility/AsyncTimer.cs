using System.Threading.Tasks;
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

        public AsyncTimer(System.Action callback, float interval, bool loop = false, float firstDelay = 0)
        {
            this.callback = callback;
            this.interval = interval;
            this.loop = loop;
            this.firstDelay = firstDelay;
        }

        public async void Start()
        {
            if (!loop)
            {
                await Awaitable.WaitForSecondsAsync(interval);
                callback?.Invoke();
            }
            else
            {
                await Awaitable.WaitForSecondsAsync(firstDelay);
                callback?.Invoke();

                while (isRunning)
                {
                    await Awaitable.WaitForSecondsAsync(interval);
                    callback?.Invoke();
                }
            }

            isRunning = false;
        }

        public void Stop() => isRunning = false;

        public void SetInterval(float interval) => this.interval = interval;
    }

    public class AsyncTimer<T>
    {
        private System.Action<T> callback;
        private T payload;
        private float interval;
        private bool loop;
        private float firstDelay;

        private bool isRunning;

        public AsyncTimer(System.Action<T> callback, T payload, float interval, bool loop = false, float firstDelay = 0)
        {
            this.callback = callback;
            this.payload = payload;
            this.interval = interval;
            this.loop = loop;
            this.firstDelay = firstDelay;
        }

        public async void Start()
        {
            if (!loop)
            {
                await Awaitable.WaitForSecondsAsync(interval);
                callback?.Invoke(payload);
            }
            else
            {
                await Awaitable.WaitForSecondsAsync(firstDelay);
                callback?.Invoke(payload);

                while (isRunning)
                {
                    await Awaitable.WaitForSecondsAsync(interval);
                    callback?.Invoke(payload);
                }
            }

            isRunning = false;
        }

        public void Stop()
        {
            isRunning = false;
        }
    }
}