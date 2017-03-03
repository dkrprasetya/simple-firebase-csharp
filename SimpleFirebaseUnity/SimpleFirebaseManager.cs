﻿/*

Class: FirebaseManager.cs
==============================================
Last update: 2016-07-31 (by Dikra)
==============================================

Copyright (c) 2016  M Dikra Prasetya

 * MIT LICENSE
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

*/

using UnityEngine;

namespace SimpleFirebaseUnity
{
    [ExecuteInEditMode]
    public class SimpleFirebaseManager : MonoBehaviour
    {

        private static SimpleFirebaseManager _instance;
        private static object _lock = new object();

        protected SimpleFirebaseManager() { }

        public static SimpleFirebaseManager Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    Debug.LogWarning("[Firebase Manager] Instance already destroyed on application quit." +
                        " Won't create again - returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        SimpleFirebaseManager[] managers = FindObjectsOfType<SimpleFirebaseManager>();

                        _instance = (managers.Length > 0) ? managers[0] : null;

                        if (managers.Length > 1)
                        {
                            Debug.LogError("[Firebase Manager] Something went really wrong " +
                                " - there should never be more than 1 Firebase Manager!" +
                                " Reopening the scene might fix it.");

                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<SimpleFirebaseManager>();
                            singleton.name = "Firebase Manager [Singleton]";

                            DontDestroyOnLoad(singleton);

                            Debug.Log("[Firebase Manager] Instance '" + singleton +
                                "' was generated in the scene with DontDestroyOnLoad.");
                        }
                        else {
                            Debug.Log("[Firebase Manager] Using instance already created: " +
                                _instance.gameObject.name);
                        }
                    }

                    return _instance;
                }
            }
        }

        private static bool applicationIsQuitting = false;
        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        ///   it will create a buggy ghost object that will stay on the Editor scene
        ///   even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        public void OnDestroy()
        {
            if (Application.isPlaying)
                applicationIsQuitting = true;
        }

    }
}
