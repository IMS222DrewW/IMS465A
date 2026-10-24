using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Block", fileName = "new Block")]
public class Block : ScriptableObject
{
    public string BlockName;
    public GameObject BlockObject;
    public string ItemsNeededForBuildingBlock;
    public int AmmountOfItemNeeded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
