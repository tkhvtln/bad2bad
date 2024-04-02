using System.Reflection;
using UnityEngine;

public static class JoystickExtensions 
{
    public static void ResetJoystick(this Joystick joystick)
    {
        //Доступ к приватным полям

        FieldInfo fieldInfoInput = typeof(Joystick).GetField("input", BindingFlags.Instance | BindingFlags.NonPublic);
        Vector2 input = (Vector2)fieldInfoInput.GetValue(joystick);
        input = Vector2.zero;

        FieldInfo fieldInfoHandle = typeof(Joystick).GetField("handle", BindingFlags.Instance | BindingFlags.NonPublic);
        RectTransform handle = (RectTransform)fieldInfoHandle.GetValue(joystick);
        handle.anchoredPosition = Vector2.zero;
    }
}
