using Assets.Scripts;

public class Hull : ShipPart
{
    public Hull(Ship parentShip) : base(parentShip) { }

    // Use this for initialization
    public override void InitShipPart(Ship ship)
    {
        ParentShip = ship;
        MaxNumberOfCrewMembers = 5;
        MaxHealth = 100;
        Health = MaxHealth;
        Weight = GameConfig.Instance.HullWeight;

        Description = "Place to kill some time, or people...";
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    public override bool IsOnBottom()
    {
        return true;
    }
}
