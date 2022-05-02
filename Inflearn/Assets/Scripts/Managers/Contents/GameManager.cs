using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager 
{
    GameObject _player;
    HashSet<GameObject> _monsters = new HashSet<GameObject>();
    // Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
    // 서버랑 연동해야 한다면 아이디와 오브젝트를 짝지어서 Dictionary로 저장한다

    public Action<int> OnSpawnEvent; 

    public GameObject GetPlayer() { return _player; }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Monster:
                _monsters.Add(go);
                if (OnSpawnEvent != null)   
                    OnSpawnEvent.Invoke(1); 
                break;
            case Define.WorldObject.Player:
                _player = go;
                break;
        }

        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
            return Define.WorldObject.Unknown;

        return bc.WorldObjectType;
    }

    public void Despawn(GameObject go)
    {
        Define.WorldObject type = GetWorldObjectType(go);

        switch (type)
        {
            case Define.WorldObject.Monster:
                {
                    if (_monsters.Contains(go))     
                    {
                        _monsters.Remove(go);
                        if (OnSpawnEvent != null)   
                            OnSpawnEvent.Invoke(-1); 
                    }
                }
                break;
            case Define.WorldObject.Player:
                {
                    if (_player == go)
                        _player = null;
                }
                break;
        }

        Managers.Resource.Destroy(go);
    }
}
