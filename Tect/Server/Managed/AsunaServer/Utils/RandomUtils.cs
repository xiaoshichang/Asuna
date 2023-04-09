namespace AsunaServer.Utils;

public static class RandomUtils
{
    public static T RandomGetItem<T>(List<T> seq)
    {
        if (seq.Count == 0)
        {
            throw new IndexOutOfRangeException();
        }
        
        var index = Random.Shared.Next(seq.Count);
        return seq[index];
    }
    
    public static T RandomGetItem<T>(T[] seq)
    {
        if (seq.Length == 0)
        {
            throw new IndexOutOfRangeException();
        }
        
        var index = Random.Shared.Next(seq.Length);
        return seq[index];
    }
}