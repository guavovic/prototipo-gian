using UnityEngine;

public class VehicleCharacteristics
{
    public string Name { get; private set; }
    public Sprite Sprite { get; private set; }
    public Color Color { get; private set; }

    public VehicleCharacteristics(string name, Sprite sprite, Color color)
    {
        Name = name;
        Sprite = sprite;
        Color = color;
    }
}