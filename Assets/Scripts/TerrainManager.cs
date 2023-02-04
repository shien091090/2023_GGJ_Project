using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager Instances { get; private set; }

    [SerializeField]
    private List<TerrainGroup> _terrainGroups = null;

    private void Awake()
    {
        Instances = this;

        for (int i = 0; i < _terrainGroups.Count; i++)
            _terrainGroups[i].id = i;
    }

    private void OnDestroy()
    {
        TerrainManager.Instances = null;
    }

    public bool GetTerrain(out Vector2 pos)
    {
        var findTerrain = _terrainGroups.FirstOrDefault(s=>s.IsOwnItem == false);
        if (findTerrain != null)
        {
            pos = findTerrain.sr.transform.position;
            findTerrain.IsOwnItem = true;
            return transform;
        }
        pos =Vector2.zero;
        return false;
    }

    public void ReleaseTerrain(int id)
    {
        foreach (var terrainGroup in _terrainGroups)
        {
            if (terrainGroup.id==id)
            {
                terrainGroup.IsOwnItem = false;
            }
        }
    }
    
}

[System.Serializable]
public class TerrainGroup
{
    [HideInInspector]
    public int id;

    [HideInInspector]
    public bool IsOwnItem = false;

    public SpriteRenderer sr;
}