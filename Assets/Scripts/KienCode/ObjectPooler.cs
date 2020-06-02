using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

    public class ObjectPooler : MonoBehaviour
    {

        public Transform Parent;
        public GameObject PooledObject;
        private List<GameObject> PooledObjects;

        public int PoolLength;

        private Type[] componentsToAdd;
        void Awake()
        {
            PoolLength = 10;
        }


        public void Initialize(int length)
        {
            PooledObjects = new List<GameObject>();
            for (int i = 0; i < length; i++)
            {
                CreateObjectInPool();
            }
        }

      public  void DisableAllObject()
        {
            foreach (GameObject go in PooledObjects)
            {
                if (go.activeInHierarchy) go.SetActive(false);
            }
        }

        public void Initialize(int length, params Type[] componentsToAdd)
        {
            this.componentsToAdd = componentsToAdd;
            Initialize(length);
        }

        public GameObject GetPooledObject()
        {
            for (int i = 0; i < PooledObjects.Count; i++)
            {
                if (!PooledObjects[i].activeInHierarchy)
                {
                    return PooledObjects[i];
                }
            }
            int indexToReturn = PooledObjects.Count;
            //create more
            CreateObjectInPool();
            //will return the first one that we created
            return PooledObjects[indexToReturn];
        }

        public bool CheckPoolerObjectActive()
        {
            foreach (GameObject go in PooledObjects)
            {
                if (go.activeInHierarchy) return true;
            }
            return false;
        }

        private void CreateObjectInPool()
        {
            //if we don't have a prefab set, instantiate a new gameobject
            //else instantiate the prefab
            GameObject go;
            if (PooledObject == null)
                go = new GameObject(this.name + " PooledObject");
            else
            {
                go = Instantiate(PooledObject) as GameObject;
            }

            //set the new object as inactive and add it to the list
            go.SetActive(false);
            PooledObjects.Add(go);

            //if we have components to add
            //add them
            if (componentsToAdd != null)
                foreach (Type itemType in componentsToAdd)
                {
                    go.AddComponent(itemType);
                }

            //if we have set the parent, assign it as the new object's parent
            if (Parent != null)
                go.transform.parent = this.Parent;
            else
                go.transform.parent = transform;
        }



}