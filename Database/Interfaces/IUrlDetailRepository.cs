using Database.Entities;
using System.Threading.Tasks;

namespace Database.Interfaces
{
    public interface IUrlDetailRepository
    {
        Task<UrlDetail> AddAsync(UrlDetail urlDetail);

        UrlDetail GetByOriginalUrl(string originalUrl);

        UrlDetail GetByCode(string code);

    }
}
