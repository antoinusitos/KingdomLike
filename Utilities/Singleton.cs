namespace KingdomLike.Utilities;

public class Singleton<T> where T : new()
{
    protected static T instance;

    public static T Instance => instance;

    public static void Initialize()
    {
        instance = new T();
    }
}