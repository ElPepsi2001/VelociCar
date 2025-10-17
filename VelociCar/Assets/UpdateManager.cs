using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    private static List<IUpdateObserver> _observers = new List<IUpdateObserver>();
    private static List<IUpdateObserver> _pendingObservers = new List<IUpdateObserver>();
    private static int _currentIndex;

    private void Update()
    {
        for (_currentIndex = _observers.Count -1;  _currentIndex >= 0; _currentIndex--)
        {
            _observers[_currentIndex].ObservedUpdate();
        }

        _observers.AddRange (_pendingObservers);
        _pendingObservers.Clear ();
    }

    public static void RegisterObserver(IUpdateObserver observer)
    {
        _pendingObservers.Add (observer);
    }

    public static void UnregisterObserver(IUpdateObserver observer)
    {
        _pendingObservers.Remove (observer);
        _currentIndex--;
    }
}
public class FixedUpdateManager : MonoBehaviour
{
    private static List<IFixedUpdateObserver> _observers = new List<IFixedUpdateObserver>();
    private static List<IFixedUpdateObserver> _pendingObservers = new List<IFixedUpdateObserver>();
    private static int _currentIndex;

    private void FixedUpdate()
    {
        for (_currentIndex = _observers.Count -1;  _currentIndex >= 0; _currentIndex--)
        {
            _observers[_currentIndex].ObservedFixedUpdate();
        }

        _observers.AddRange (_pendingObservers);
        _pendingObservers.Clear ();
    }

    public static void RegisterObserver(IFixedUpdateObserver observer)
    {
        _pendingObservers.Add (observer);
    }

    public static void UnregisterObserver(IFixedUpdateObserver observer)
    {
        _pendingObservers.Remove (observer);
        _currentIndex--;
    }
}
public class LateUpdateManager : MonoBehaviour
{
    private static List<ILateUpdateObserver> _observers = new List<ILateUpdateObserver>();
    private static List<ILateUpdateObserver> _pendingObservers = new List<ILateUpdateObserver>();
    private static int _currentIndex;

    private void FixedUpdate()
    {
        for (_currentIndex = _observers.Count -1;  _currentIndex >= 0; _currentIndex--)
        {
            _observers[_currentIndex].ObservedLateUpdate();
        }

        _observers.AddRange (_pendingObservers);
        _pendingObservers.Clear ();
    }

    public static void RegisterObserver(ILateUpdateObserver observer)
    {
        _pendingObservers.Add (observer);
    }

    public static void UnregisterObserver(ILateUpdateObserver observer)
    {
        _pendingObservers.Remove (observer);
        _currentIndex--;
    }
}

public interface IUpdateObserver
{
    public void ObservedUpdate(); 
}
public interface IFixedUpdateObserver
{
    public void ObservedFixedUpdate(); 
}
public interface ILateUpdateObserver
{
    public void ObservedLateUpdate(); 
}
