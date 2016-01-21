namespace Orange.Core.Values
{
    public class HashedPassword
    {
        public string Salt { get; private set; }
        public string Hash { get; private set; }
        public int Iterations { get; private set; }

        public HashedPassword(string salt, string hash, int iterations)
        {
            Salt = salt;
            Hash = hash;
            Iterations = iterations;
        }
    }

}
