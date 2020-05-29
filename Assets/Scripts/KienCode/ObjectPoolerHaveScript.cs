using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolerHaveScript : MonoBehaviour
{
    public Transform Parent;
    public Unit unitPooledObject;
    private List<Unit> PooledUnit;

    public int PoolLength;

    void Awake()
    {
        PoolLength = 10;
    }

    #region text number damage
    public void InitializeUnit(int length)
    {
        PooledUnit = new List<Unit>();
        for (int i = 0; i < length; i++)
        {
            CreateUnitObjectInPool();
        }
    }

    public Unit GetUnitPooledObject()
    {
        for (int i = 0; i < PooledUnit.Count; i++)
        {
            if (!PooledUnit[i].gameObject.activeInHierarchy)
            {
                return PooledUnit[i];
            }
        }
        int indexToReturn = PooledUnit.Count;
        //create more
        CreateUnitObjectInPool();
        //will return the first one that we created
        return PooledUnit[indexToReturn];
    }


    private void CreateUnitObjectInPool()
    {
        Unit go;
        if (unitPooledObject == null)
            go = new Unit();
        else
        {
            go = Instantiate(unitPooledObject) as Unit;
        }

        go.gameObject.SetActive(false);
        PooledUnit.Add(go);
        if (Parent != null)
            go.transform.parent = this.Parent;
        else
            go.transform.parent = transform;
    }
    #endregion

}
