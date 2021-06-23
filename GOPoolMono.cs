using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GOPoolMono: MonoBehaviour
{
    public GameObject prefab;

    private List<GameObject> list;
    private List<bool> stateList;
    private bool autoNew;
    private Transform spwnPos, parent;
    private int maxCout;

    public void Setup(GameObject prefab, uint defaultCount, int maxCout, bool autoNew, Transform patent = null, Transform spwnPos = null) 
    {
        list = new List<GameObject>();
        stateList = new List<bool>();

        this.spwnPos = spwnPos;
        this.parent = patent;
        this.prefab = prefab;
        this.autoNew = autoNew;
        this.maxCout = maxCout;
        for (int i = 0; i < defaultCount; i++) New();
    }

    private GameObject New()
    {
        GameObject go;

        if (spwnPos == null && parent == null || spwnPos != null && parent == null)
        {
            go = MonoBehaviour.Instantiate<GameObject>(prefab, Vector3.zero, Quaternion.identity);
        }
        else if (spwnPos != null && parent != null)
        {
            go = MonoBehaviour.Instantiate<GameObject>(prefab, spwnPos.position, spwnPos.rotation, parent);
        }
        else
        {
            go = MonoBehaviour.Instantiate<GameObject>(prefab, Vector3.zero, Quaternion.identity, parent);
        }

        go.gameObject.SetActive(false);
        list.Add(go);
        stateList.Add(false);
        return go;
    }

    public GameObject Spawn(Transform patent = null, bool isActive = true)
    {
        for (int i = 0; i < stateList.Count; i++)
        {
            if (!stateList[i])
            {
                stateList[i] = isActive;
                if (patent != null) list[i].gameObject.transform.SetParent(patent);
                else list[i].gameObject.transform.SetParent(this.parent);

                list[i].gameObject.SetActive(isActive);
                return list[i];
            }
        }

        if (autoNew)
        {
            GameObject go = New();
            go.gameObject.SetActive(isActive);
            Join(go, isActive);
            return go;
        }
        else
        {
            return null;
        }
    }

    public void SetActive(GameObject go)
    {
        Join(go, true);
        go.gameObject.SetActive(true);
    }

    private void Join(GameObject go, bool isActive)
    {
        int j = list.IndexOf(go);
        stateList[j] = isActive;
    }

    public void Reales(GameObject go)
    {
        go.gameObject.SetActive(false);
        Join(go, false);

        RemoveExtraGo();
    }
    public void RealesAll()
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].gameObject.SetActive(false);
            stateList[i] = false;
        }

        RemoveExtraGo();
    }

    private void RemoveExtraGo()
    {

        if (maxCout > 0 && list.Count > maxCout)
        {
            int delCount = list.Count - maxCout;
            for (int i = 0; i < list.Count; i++)
            {
                if (!stateList[i])
                {
                    Destroy(list[i].gameObject);
                    list.RemoveAt(i);
                    stateList.RemoveAt(i);
                    delCount--;
                    if (delCount <= 0) break;
                }
            }
        }
    }
}
