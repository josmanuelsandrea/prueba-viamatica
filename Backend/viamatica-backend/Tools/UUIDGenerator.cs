namespace viamatica_backend.Tools
{
    public static class UUIDGenerator
    {
        public static string Generate(string format = "N")
        {
            return Guid.NewGuid().ToString(format);
        }
    }
}
