using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Helper {
    /// <summary>
    /// Finds a Gameobject with the tag in a parent gameobject
    /// </summary>
    /// <param name="parent">Parent to search for tag</param>
    /// <param name="tag">Tag to search for</param>
    /// <returns>Child object with the tag</returns>
    public static GameObject FindChildWithTag(this GameObject parent, string tag) {
        foreach (Transform child in parent.transform) {  //search parent's childs
            if (child.CompareTag(tag)) {
                return child.gameObject;    //when found    
            } else if (child.childCount > 0) {    //if not found, we search the childs children
                GameObject temp = child.gameObject.FindChildWithTag(tag);
                if (temp) {
                    return temp;    //if childs children have it
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Same as the Gameobject version, but with Transform instead
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static Transform FindChildWithTag(this Transform parent, string tag) {
        foreach (Transform child in parent) {  //search parent's childs
            if (child.CompareTag(tag)) {
                return child;    //when found    
            } else if (child.childCount > 0) {    //if not found, we search the childs children
                Transform temp = child.FindChildWithTag(tag);
                if (temp) {
                    return temp;    //if childs children have it
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Returns the component T in the child with tag
    /// </summary>
    /// <typeparam name="T">Component</typeparam>
    /// <param name="parent">Parent object</param>
    /// <param name="tag">Tag to search for</param>
    /// <returns>Component in a child of parent</returns>
    public static T FindComponentOfChildWithTag<T>(this GameObject parent, string tag) where T : Component {
        GameObject childWithTag = parent.FindChildWithTag(tag);
        if (childWithTag) {
            return childWithTag.GetComponent<T>();
        }
        return default(T);
    }

    /// <summary>
    /// Changes all the shaders of a transform
    /// </summary>
    /// <param name="T"></param>
    /// <param name="shd"></param>
    public static void ChangeShader(this Transform T, Shader shd) {
        foreach (Transform child in T) {
            Renderer rd = child.GetComponent<Renderer>();
            if (rd) {
                foreach (Material mat in rd.materials) {
                    mat.shader = shd;
                }
            }
            child.ChangeShader(shd);
        }
    }

    public static IEnumerator KillSelf(this GameObject _gameObject, float _ttl) {
        yield return new WaitForSeconds(_ttl);
        MonoBehaviour.Destroy(_gameObject);   
    }

    public static bool ArrContains<T>(this T[] _arr, T _item) {
        foreach(T item in _arr) {
            if (item.Equals(_item)) {
                return true;
            }
        }
        return false;
    }

    public static T FindComponentInScene<T>(string tag) {
        GameObject go = GameObject.FindWithTag(tag);
        if (go) return go.GetComponent<T>();
        return default(T);
    }

    /// <summary>
    /// Stops all particle systems in transform and children
    /// </summary>
    public static void StopParticleSystem(Transform _t) {
        ParticleSystem ps = _t.GetComponent<ParticleSystem>();
        if (ps) ps.Stop();
        foreach(Transform child in _t) {
            StopParticleSystem(child);
        }
    }
}

public static class Layers {
    //contains all the layer numbers DO NOT CHANGE UNLESS LAYERS CHANGE TOO
    public const int
        Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,
        Water = 4,
        UI = 5,
        PostProcessing = 8,
        Enemy = 9,
        Player = 10,
        Terrain = 11,
        Obstacles = 1,
        CreatedObjects = 15;
}

//For aStar path finding
public class PiorityQueue<T> where T : IComparable {
    //member function
    private List<T> m_queue = new List<T>();

    //properties
    public int Count { get { return m_queue.Count; } }
    public T Head {
        get {
            if (m_queue.Count > 0) {
                return m_queue[0];
            }
            return default(T);
        }
    }

    public PiorityQueue() { } //default 

    /// <summary>
    /// Push new object in
    /// </summary>
    /// <param name="_input"></param>
    /// <returns></returns>
    public void Enqueue(T _input) {
        m_queue.Add(_input);
        m_queue.Sort();
    }

    /// <summary>
    /// Remove head of m_queue
    /// </summary>
    public T Dequeue() {
        if (m_queue.Count > 0) {
            T temp = m_queue[0];
            m_queue.RemoveAt(0);
            return temp;
        }
        return default(T);
    }

    /// <summary>
    /// Checks if _input is in the queue
    /// </summary>
    public bool Contains(T _input) {
        return m_queue.Contains(_input);
    }
}

//player class mainly
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static bool m_shuttingDown = false;
    private static object m_lock = new object();
    private static T m_instance;

    public static T Instance {
        get {
            if (m_shuttingDown) return default(T);

            //lock (m_lock) {
                if (!m_instance) {
                    m_instance = FindObjectOfType<T>();
                    if (!m_instance) {
                        GameObject singletonObj = new GameObject();
                        m_instance = singletonObj.AddComponent<T>();
                        singletonObj.name = typeof(T).ToString() + " Singleton";
                    }
                }
                return m_instance;
           // }
        }
    }

    private void OnApplicationQuit() {
        m_shuttingDown = true;
    }

    private void OnDestroy() {
        //WHAT THE FUCK
        //m_shuttingDown = true;
    }
}
