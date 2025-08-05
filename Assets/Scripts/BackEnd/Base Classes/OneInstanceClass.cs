namespace BackEnd.Base_Classes
{
    public abstract class OneInstanceClass<T> where T : class, new()
    {
        private static T _instance;
        public static T Instance => _instance ??= new T();
    }
}