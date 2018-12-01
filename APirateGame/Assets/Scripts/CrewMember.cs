using UnityEngine;

public class CrewMember : MonoBehaviour {

    public ShipPart CurrentShipPart;
    public int Health;
    public bool IsUnderPlague;
    public int ResourceConsumption;

    public bool IsDead { get; private set; }

    public Ship ship;

    public CrewMember(ShipPart StartingShipPart)
    {
        Health = 10;
        IsDead = false;
        CurrentShipPart = StartingShipPart;
    }

    public void ReduceHealth(int damage)
    {
        Health -= damage;
        if (Health <= 0)
            IsDead = true;
    }
}
