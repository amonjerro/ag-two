using UnityEngine;
using ExplorationMap;
using System.Collections.Generic;

[CreateAssetMenu(menuName=Constants.ScriptableObjectsPath+"TileMovementCost")]
public class TileMovementSO : ScriptableObject
{
    [System.Serializable]
    public class TypeCostMap
    {
        public ConnectionType type;
        [Tooltip("Time in ticks to walk through")]
        public int value;
        public bool walkableByDefault;
        public float probability;
    }


    public List<TypeCostMap> typeCosts = new List<TypeCostMap>();
    public Dictionary<ConnectionType, TypeCostMap> tileTypeData;


    public void SetUpTileTypeData()
    {
        tileTypeData = new Dictionary<ConnectionType, TypeCostMap>();
        foreach (TypeCostMap typeCostMap in typeCosts)
        {
           tileTypeData.Add(typeCostMap.type, typeCostMap);
        }
    }

    public float GetOdds(ConnectionType type)
    {
        return tileTypeData[type].probability;
    }
}
