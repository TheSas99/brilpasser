using UnityEngine;
using System.IO;

[System.Serializable]
public class Product
{
    public string naam;
    public string vorm;
    public string image;
    public string materiaal;
    public string type;
    public string kleur;
    public string gender;
    public string Merknaam;
    public float montuur_prijs;

    // Define properties to access the fields
    public string Name => naam;
    public string Shape => vorm;
    public Sprite Image
    {
        get
        {
            string fileName = Path.GetFileName(image);
            string imagePath = "Sprites/Images/Brillen/" + Path.GetFileNameWithoutExtension(fileName);
            Debug.Log("Loading image from path: " + imagePath);
            Sprite loadedSprite = Resources.Load<Sprite>(imagePath);
            if (loadedSprite == null)
            {
                Debug.LogError("Failed to load image: " + imagePath);
            }
            return loadedSprite;
        }
    }
    public string Material => materiaal;
    public string Type => type;
    public string Color => kleur;
    public string Gender => gender;
    public string Brand => Merknaam;
    public float Price => montuur_prijs;
}
