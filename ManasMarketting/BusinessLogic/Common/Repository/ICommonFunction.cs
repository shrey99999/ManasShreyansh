using System.Data;

namespace BusinessLogic.Repository
{
    public interface ICommonFunction
    {
        bool CheckFileFormat(IFormFile file);
        List<T> ConvertDataTable<T>(DataTable dt);
        T GetItem<T>(DataRow dr);
        string CreatePassword(int length);
        bool CheckImageFormat(IFormFile file);
    }
}
