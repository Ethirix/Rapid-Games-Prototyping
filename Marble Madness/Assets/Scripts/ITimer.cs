using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimer
{
    bool IsRunning { get; }
    void start();
    void stop();
    void reset();
}
