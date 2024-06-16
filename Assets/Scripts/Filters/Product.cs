using UnityEngine;

[System.Serializable]
public class Product
{
    // Map the JSON keys to the corresponding fields
    [SerializeField] private string naam;
    [SerializeField] private string vorm;
    [SerializeField] private string image;
    [SerializeField] private string materiaal;
    [SerializeField] private string type;
    [SerializeField] private string kleur;
    [SerializeField] private string gender;
    [SerializeField] private string Merknaam; // Assuming this is the brand name
    [SerializeField] private float montuur_prijs;

    // Define properties to access the fields
    public string Name => naam;
    public string Shape => vorm;
    public Sprite Image => Resources.Load<Sprite>("/Sprites/Images/Brillen/" + image); // Load sprite from Resources folder
    public string Material => materiaal;
    public string Type => type;
    public string Color => kleur;
    public string Gender => gender;
    public string Brand => Merknaam;
    public float Price => montuur_prijs;
}
