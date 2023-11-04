using SQLiteCRUD.Models;

namespace SQLiteCRUD.Interface
{
    public interface IUtilities
    {
        void InsertRandomNames(int numberOfNames);
        Task<List<Driver>> GetUsersAlphabetically();
        Task<string> GetAlphabetizedName(int driverId);
    }
}
