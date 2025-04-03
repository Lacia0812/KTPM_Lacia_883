using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASC.Web.Controllers
{
    [Authorize] // Yêu cầu xác thực cho tất cả controller kế thừa
    public class BaseController : Controller
    {
        // Đây là controller cơ sở, các controller khác sẽ kế thừa nó
    }
}
