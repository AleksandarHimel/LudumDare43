using UnityEngine;
using UnityEngine.UI;

public class ShipInventory : MonoBehaviour
{
    public uint Food { get; private set; }
    public uint WoodForFuel { get; private set; }

    public Text ResourcesTextBox;
    public Button MoveEndButton;

    public ShipInventory(uint food, uint wood)
    {
        InitialiseResources(food, wood);
    }

    // Use this for initialization
    void Start()
    {
        InitialiseResources(100, 100);
        UpdateTextBox();

        MoveEndButton.onClick.AddListener(ProcessMoveEnd);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitialiseResources (uint food, uint wood)
    {
        Food = food;
        WoodForFuel = wood;
    }

    public void ProcessMoveEnd()
    {
        Debug.Log("Processing move end");
        tryRemoveAmountOfFood(1);
    }

    public uint tryRemoveAmountOfFood (uint amount)
    {
        uint retVal = Food >= amount ? amount : Food;
        Food = Food >= amount ? Food - amount : 0;

        UpdateTextBox();

        return retVal;
    }

    private void UpdateTextBox()
    {
        ResourcesTextBox.text = string.Format("Resources: food {0}, wood {1}", Food, WoodForFuel);
    }

    public uint tryRemoveAmountOfWood(uint amount)
    {
        uint retVal = WoodForFuel >= amount ? amount : WoodForFuel;
        WoodForFuel = WoodForFuel >= amount ? WoodForFuel - amount : 0;

        UpdateTextBox();

        return retVal;
    }

    public void ReduceResources(uint foodToReduce, uint woodToReduce)
    {

    }
}
