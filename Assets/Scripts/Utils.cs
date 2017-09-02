using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RectBounds
{
    public float LeftBound;
    public float RightBound;
    public float UpperBound;
    public float LowerBound;
}

public static class Utils
{
    public static System.Random Rand = new System.Random();

    public static void Shift(this Transform transform, float xShift, float yShift, float zShift)
    {
        transform.position = new Vector3(transform.position.x + xShift, transform.position.y + yShift, transform.position.z + zShift);
    }

    public static void SetX(this Transform transform, float newX)
    {
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    public static void SetY(this Transform transform, float newY)
    {
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public static void SetZ(this Transform transform, float newZ)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }

    public static Vector3 SetX(this Vector3 vec, float newX)
    {
        return new Vector3(newX, vec.y, vec.z);
    }

    public static Vector3 SetY(this Vector3 vec, float newY)
    {
        return new Vector3(vec.x, newY, vec.z);
    }

    public static Vector3 SetZ(this Vector3 vec, float newZ)
    {
        return new Vector3(vec.x, vec.y, newZ);
    }

    public static RectBounds GetCamWorldBounds(this Camera cam, float padding = 0.0f)
    {
        RectBounds bounds = new RectBounds()
        {
            UpperBound = cam.orthographicSize + cam.transform.position.x - padding,
            RightBound = cam.orthographicSize * cam.aspect + cam.transform.position.y - padding
        };

        bounds.LeftBound = -bounds.RightBound + padding;
        bounds.LowerBound = -bounds.UpperBound + padding;
        return bounds;
    }

    public static T GetRandom<T>(this IList<T> list)
    {
        if (list.Count == 0)
        {
            return default(T);
        }

        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Utils.Rand.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}


/// <summary>
/// A timer that can be used to track cooldowns and 
/// durations of things.
/// </summary>
public class CooldownTimer
{
    /// <summary>
    /// The goal of the timer. Once this time is reached,
    /// the timer is "expired" (ie goes "ding!").
    /// </summary>
    private float baseLine = 0.0f;

    private float currentTime = 0.0f;

    public event EventHandler OnCooldownExpired;

    /// <summary>
    /// Ctor for creating an inactive timer. Set new baseline with
    /// SetBaseline method.
    /// </summary>
    public CooldownTimer()
    {
        this.baseLine = 0.0f;
    }

    /// <summary>
    /// Ctor for creating an active timer with timeout
    /// occurring after time reaches 'baseLine'.
    /// </summary>
    /// <param name="baseLine">How long the timer must tick 
    /// before cooldown expires</param>
    public CooldownTimer(float baseLine, bool startsExpired = true)
    {
        this.baseLine = baseLine;
        if (startsExpired)
        {
            this.currentTime = this.baseLine;
        }
    }

    /// <summary>
    /// Ticks the timer by some amount. Usually goes
    /// in the Update loop using Time.deltaTime, but
    /// delta can be anything. If delta is null, 
    /// Time.deltaTime is used. Returns a reference to
    /// this object, so you can do stuff like
    /// if (timer.Tick().IsExpired) and other such things.
    /// 
    /// Only ticks if the cooldown is active (not expired and
    /// non-zero baseLine). Immediately after expiry, the
    /// OnCooldownExpired event is fired.
    /// </summary>
    /// <param name="delta"></param>
    /// <returns></returns>
    public CooldownTimer Tick(float? delta = null)
    {
        if (this.IsActive)
        {
            this.currentTime += delta ?? Time.deltaTime;
            if (this.IsExpired)
            {
                // On the exact tick after expiry, this event fires.
                if (this.OnCooldownExpired != null)
                {
                    this.OnCooldownExpired(this, new EventArgs());
                }
            }
        }

        return this;
    }

    /// <summary>
    /// Whether or not the timer reached its goal
    /// </summary>
    public bool IsExpired { get { return this.currentTime >= baseLine; } }

    /// <summary>
    /// Whether or not this timer should tick.
    /// </summary>
    public bool IsActive { get { return this.baseLine > 0.0f && !this.IsExpired; } }

    /// <summary>
    /// Resets the timer to be used again.
    /// </summary>
    public void Reset()
    {
        this.currentTime = 0.0f;
    }

    /// <summary>
    /// Changes the target time for the timer.
    /// </summary>
    /// <param name="baseLine"></param>
    public void SetBaseline(float baseLine)
    {
        this.baseLine = baseLine;
    }

    public void SetInactive()
    {
        this.currentTime = this.baseLine;
    }
}

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return instance;
        }

        protected set
        {
            instance = value;
        }
    }
}

