namespace VehicleMaintenance.Services.Security
{
    public interface PasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string hash);
    }
}
