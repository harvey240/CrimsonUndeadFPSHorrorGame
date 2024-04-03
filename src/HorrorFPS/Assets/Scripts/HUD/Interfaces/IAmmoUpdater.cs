public interface IAmmoUpdater
{
    void SetAmmo(int ammoCount, int MaxAmmo);
    void SetReserve(int reserveAmmo)
    {
        // default empty optional method
    }
}