using System;
using System.Threading;

namespace TMS.ClientServices.TimerGlobalService
{
    
    public class ActiveTaskTimerService
    {
        public long? ActiveTaskId { get; private set; }
        private DateTime? _sessionStartTime;
        private TimeSpan _initialElapsedTime = TimeSpan.Zero;

        public event Action? OnTick;
        public event Action? OnStateChange;

        private Timer? _timer;

        public void StartOrResumeTimer(long taskId, TimeSpan existingElapsedTime)
        {
            ActiveTaskId = taskId;
            _initialElapsedTime = existingElapsedTime;
            _sessionStartTime = DateTime.UtcNow;

            _timer?.Dispose();
            _timer = new Timer(TimerCallback, null, 0, 1000);

            OnStateChange?.Invoke();
        }

        public TimeSpan StopTimer()
        {
            var finalTime = GetElapsedTime(ActiveTaskId ?? 0, _initialElapsedTime);

            _timer?.Dispose();
            _timer = null;
            ActiveTaskId = null;
            _sessionStartTime = null;
            _initialElapsedTime = TimeSpan.Zero;

            OnStateChange?.Invoke();
            return finalTime;
        }

        private void TimerCallback(object? state)
        {
            if (_sessionStartTime.HasValue)
            {
                OnTick?.Invoke();
            }
        }
        public TimeSpan GetElapsedTime(long taskId, TimeSpan dbElapsedTime)
        {
            if (ActiveTaskId == taskId && _sessionStartTime.HasValue)
            {
                return _initialElapsedTime + (DateTime.UtcNow - _sessionStartTime.Value);
            }

            return dbElapsedTime;
        }
    }
}
