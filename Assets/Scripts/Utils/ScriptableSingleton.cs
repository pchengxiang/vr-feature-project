using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
{
    static T m_instance;
    public static T instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = (T) new ScriptableObject();
            }
            return m_instance;
        }
        private set { }
    }
}
