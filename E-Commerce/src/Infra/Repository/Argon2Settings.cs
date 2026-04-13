namespace Project.src.Infra.Repository;

public class Argon2Settings
{
    public int MemoryCost { get; set; }
    public int TimeCost { get; set; }
    public int Lanes { get; set; }
}