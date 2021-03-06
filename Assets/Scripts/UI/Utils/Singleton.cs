﻿/// <summary>
/// http://wiki.unity3d.com/index.php/Singleton
/// </summary>

using Gamelogic.Extensions;
using UnityEngine;

namespace FlyBattle.Utils
{
    /// <inheritdoc />
    /// <summary>Singleton class</summary>
    /// <typeparam name="T">Type of the singleton</typeparam>
    public abstract class Singleton<T> : GLMonoBehaviour where T : Singleton<T>
    {
        private static T _instance;

        private static object _lock = new object();

        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                                     "' already destroyed on application quit." +
                                     " Won't create again - returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T) FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError("[Singleton] Something went really wrong " +
                                           " - there should never be more than 1 singleton!" +
                                           " Reopening the scene might fix it.");
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<T>();
                            DontDestroyOnLoad(singleton);
                        }
                    }

                    return _instance;
                }
            }
        }

        private static bool applicationIsQuitting = false;

        private void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }

        private void OnDestroy()
        {
            applicationIsQuitting = true;
        }
    }
}