using Assets.Scripts;

public class Hull : ShipPart
{
    public Hull(Ship parentShip) : base(parentShip) { }

    // Use this for initialization
    public override void InitShipPart(Ship ship)
    {
        ParentShip = ship;
        MaxNumberOfCrewMembers = 0;
        MaxHealth = 2;
        Health = MaxHealth;
        Weight = GameConfig.Instance.HullWeight;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override bool IsOnBottom()
    {
        return true;
    }
}
