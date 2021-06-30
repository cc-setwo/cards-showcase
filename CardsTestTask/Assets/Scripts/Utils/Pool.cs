using System.Collections.Generic;
using UnityEngine;

namespace CardsTeskTask.Utils
{
    public class Pool<T> where T : MonoBehaviour
    {
        private readonly Queue<T> poolObjectsQueue;
        private readonly T initalObject;
        private readonly int initialCount;

        public Pool(T initObject, int prewarmCount)
        {
            initialCount = prewarmCount;
            initalObject = initObject;
            poolObjectsQueue = new Queue<T>();

            for (int i = 0; i < prewarmCount; i++)
            {
                T newObject = CreateObject();
                newObject.gameObject.SetActive(false);
                poolObjectsQueue.Enqueue(newObject);
            }
        }

        public T Get()
        {
            if (poolObjectsQueue.Count > 0)
            {
                T objectFromQueue = poolObjectsQueue.Dequeue();
                
                if (objectFromQueue == null || objectFromQueue.gameObject == null)
                {
                    return Get();
                }

                objectFromQueue.gameObject.SetActive(true);
                return objectFromQueue;
            }

            T newObject = CreateObject();
            newObject.gameObject.SetActive(true);
            return newObject;
        }

        public void Return(T objectToReturn)
        {
            if (poolObjectsQueue.Count > initialCount)
            {
                Object.Destroy(objectToReturn.gameObject);
            }
            else
            {
                if (objectToReturn == null || objectToReturn.gameObject == null)
                {
                    return;
                }

                objectToReturn.gameObject.SetActive(false);
                poolObjectsQueue.Enqueue(objectToReturn);
            }
        }

        private T CreateObject()
        {
            T newObject = Object.Instantiate(initalObject);

            return newObject;
        }
    }
}