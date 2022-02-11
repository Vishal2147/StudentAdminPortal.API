using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace StudentAdminPortal.API.Reopsitories
{
    public interface IImageRespository
    {

       Task<string> Upload(IFormFile file, string fileName);


    }
}
