using System;
using System.Threading.Tasks;

namespace Mafia.Models;

public class Timer(Action onTick, int interval)
{
    private bool _isRunning;

    public async Task Start()
    {
        _isRunning = true;
        while (_isRunning)
        {
            await Task.Delay(interval);
            if (_isRunning)
            {
                onTick.Invoke();
            }
        }
    }

    public void Stop()
    {
        _isRunning = false;
    }
}