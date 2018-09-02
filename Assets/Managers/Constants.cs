using UnityEngine;

public static class Inputs
{
    private static string _horizontal = "Horizontal";
    private static string _vertical = "Vertical";
    private static string _fireHorizontal = "HorizontalFire";
    private static string _fireVertical = "VerticalFire";
    private static string _dPadXAxis = "DPadXAxis";
    private static string _aButton = "Abutton";

    public static string Horizontal(int controllerId) 
    {
        return _horizontal + controllerId;
    }

    public static string Vertical(int controllerId) 
    {
        return _vertical + controllerId;
    }

    public static string FireHorizontal(int controllerId) 
    {
        return _fireHorizontal + controllerId;
    }

    public static string FireVertical(int controllerId) 
    {
        return _fireVertical + controllerId;
    }

    public static string DPadAxis(int controllerId)
    {
        return _dPadXAxis + controllerId;
    }

    public static string AButton(int? controllerId = null)
    {
        return _aButton + controllerId;
    }

}

public static class AnimatorConstants
{
    public static string Speed = "Speed";
}

public static class Tags
{
    public static string Player = "Player";
}

public static class LayerConstants
{
    public static int Enemies = 8;
    public static int Players = 9;
    public static string EnemyProjectiles = "EnemyProjectiles";
    public static string PlayerProjectiles = "PlayerProjectiles";

    public static LayerMask GetLayer(string name)
    {
        return LayerMask.NameToLayer(name);
    }

    public static int GetAllExceptLayers(params string[] names)
    {
        int i = 0;
        foreach (var name in names)
        {
            i += GetLayer(name);
        }
        return ~(1 << i);
    }

    public static int GetOnlyLayer(params string[] names)
    {
        int i = 0;
        foreach (var name in names)
        {
            i += GetLayer(name);
        }
        return (1 << i);
    }
}