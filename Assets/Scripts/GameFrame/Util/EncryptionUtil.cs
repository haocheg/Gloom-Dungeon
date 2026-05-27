using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class EncryptionUtil
{

    public static int GetRandomKey()
    {
        return Random.Range(1, 10000);
    }

    public static int LockValue(int value, int key)
    {
        value = value ^ key;
        value += key;
        return value;
    }

    public static int UnlockValue(int value, int key)
    {
        if (value == 0) return 0;

        value -= key;
        value = value ^ key;
        return value;
    }

}
