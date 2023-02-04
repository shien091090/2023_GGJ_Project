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
        {
            var terrainGroup = _terrainGroups[i];
            terrainGroup.id = i;
            terrainGroup.IsOnwItem = false;
        }
    }

    private void OnDestroy()
    {
        TerrainManager.Instances = null;
    }

    public bool GetTerrain(out (int, Vector2) data)
    {
        var terrainGroups = _terrainGroups.Where(s=>s.IsOnwItem == false).ToList();

        if (terrainGroups.Count > 0)
        {
            int randIndex = Random.Range(0, terrainGroups.Count);
            var terrain = terrainGroups[randIndex];
            terrain.IsOnwItem = true;
            data = (terrain.id, terrain.GetRandPos());
            return true;
        }

        data = (0, Vector2.zero);
        return false;
    }

    public void ReleaseTerrain(int id)
    {
        foreach (var terrainGroup in _terrainGroups)
        {
            if (terrainGroup.id == id)
            {
                terrainGroup.IsOnwItem = false;
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
    public bool IsOnwItem;

    public SpriteRenderer sr;

    public Vector2 GetRandPos()
    {
        Vector2 pos = sr.transform.position;
        pos.y += sr.bounds.size.y * 0.5f;
        var dir = Random.Range(0 , 100) > 50 ? 1 : -1;
        pos.x += sr.bounds.size.x * Random.Range(0f, 0.4f) * dir;
        return pos;
    }
}