using System.Collections;
using UnityEngine;

namespace cards
{
    public sealed class Coroutines : MonoBehaviour
    {
        public static Coroutine StartRoutine(IEnumerator enumerator)
        {
            return instance.StartCoroutine(enumerator);
        }

        public static void StopRoutine(Coroutine routine)
        {
            instance.StopCoroutine(routine);
        }

        private static Coroutines instance
        {
            get
            {
                if (m_instance == null)
                {
                    var routineObject = new GameObject("Coroutine");
                    m_instance = routineObject.AddComponent<Coroutines>();
                }
                return m_instance;
            }
        }


        private static Coroutines m_instance;
    }
}
