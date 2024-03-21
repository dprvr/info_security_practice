namespace FC.Algo
{
    public interface ICryptAlgorithm
    {
        string Key { get; }
        string Encrypt(string text);
        string Decrypt(string text);
    }
}
