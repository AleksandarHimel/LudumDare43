using UnityEngine;

public class CrewMember : MonoBehaviour {

    public ShipPart CurrentShipPart;
    public int Health;
    public bool IsUnderPlague;
    public int ResourceConsumption;

    public bool IsDead { get; private set; }

    public Ship ship;

    public static string[] CrewMemberColors =
    {
        "red",
        "green",
        "blue"
    };

    void Start()
    {
        var sprite = GetComponent<SpriteRenderer>();

        var color = CrewMemberColors[new System.Random().Next(3)];
        Debug.Log(color);
        sprite.sprite = Resources.Load<Sprite>(string.Format("Sprites/Pirate {0}", color));

        Health = 10;
        IsDead = false;
    }

    public void ReduceHealth(int damage)
    {
        Health -= damage;
        if (Health <= 0)
            IsDead = true;
    }
}
