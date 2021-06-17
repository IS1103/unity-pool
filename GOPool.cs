using System.Collections.Generic;
using UnityEngine;

public class GOPool<T> where T : MonoBehaviour
{
    private T prefab;
    private List<T> list;
    private List<bool> stateList;
    private bool autoNew;
    private Transform spwnPos, parent;
    private int maxCout;

    public GOPool(T prefab, uint defaultCount, int maxCout, bool autoNew, Transform patent = null, Transform spwnPos = null)
    {
        list = new List<T>();
        stateList = new List<bool>();

        this.spwnPos = spwnPos;
        this.parent = patent;
        this.prefab = prefab;
        this.autoNew = autoNew;
        this.maxCout = maxCout;
        for (int i = 0; i < defaultCount; i++) New();
    }

    private T New()
    {
        T go;

        if (spwnPos == null&& parent ==null|| spwnPos != null && parent == null)
        {
            go = MonoBehaviour.Instantiate<T>(prefab, Vector3.zero, Quaternion.identity);
        }
        else if (spwnPos != null && parent != null)
        {
            go = MonoBehaviour.Instantiate<T>(prefab, spwnPos.position, spwnPos.rotation, parent);
        }
        else 
        {
            go = MonoBehaviour.Instantiate<T>(prefab, Vector3.zero, Quaternion.identity, parent);
        }

        go.gameObject.SetActive(false);
        list.Add(go);
        stateList.Add(false);
        return go;
    }

    public T Spawn(Transform patent = null)
    {
        for (int i = 0; i < stateList.Count; i++)
        {
            if (!stateList[i])
            {
                stateList[i] = true;
                if (patent != null) list[i].gameObject.transform.SetParent(patent);
                else list[i].gameObject.transform.SetParent(this.parent);

                list[i].gameObject.SetActive(true);
                return list[i];
            }
        }

        if (autoNew)
        {
            T go = New();
            go.gameObject.SetActive(true);
            Join(go, true);
            return go;
        }
        else
        {
            return null;
        }
    }

    private void Join(T go, bool isActive)
    {
        int j = list.IndexOf(go);
        stateList[j] = isActive;
    }

    public void Reales(T go)
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
                    MonoBehaviour.Destroy(list[i].gameObject);
                    list.RemoveAt(i);
                    stateList.RemoveAt(i);
                    delCount--;
                    if (delCount <= 0) break;
                }
            }
        }
    }
}
