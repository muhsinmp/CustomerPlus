using DataAccess.CommonModel.Response;
using DataAccess.ViewModels;
using System.Threading.Tasks;

namespace WebAPI.Account
{
    public interface IAccountRepo
    {
        Task<CommonResponse> RegisterAdmin(RegisterModel model);
        Task<LoginResponse> Login(LoginModel model);
    }
}
