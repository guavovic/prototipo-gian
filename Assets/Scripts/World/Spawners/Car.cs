using UnityEngine;

public class Car
{
    public string Name { get; private set; }
    public Color Color { get; private set; }

    public Car(string name, Color color)
    {
        Name = name;
        Color = color;
    }
}