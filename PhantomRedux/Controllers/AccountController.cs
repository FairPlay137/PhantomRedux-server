using Microsoft.AspNetCore.Mvc;
using PhantomRedux.Request;
using PhantomRedux.Response;
using System.Text.Json;

namespace PhantomRedux.Controllers
{
    [ApiController]
    [Route("intl/account")]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        [Route("regist")]
        [Produces("text/json")]
        public JsonResult RegisterAccount([FromForm] string post)
        {
            RegisterRequest? requestData = JsonSerializer.Deserialize<RegisterRequest>(post);
            if (requestData == null)
            {
                return new JsonResult(new BaseResponse(GameStatusCode.Err_JsonAnalysisFailed, 0, 0));
            }

            return new JsonResult(new BaseResponse(GameStatusCode.Err_UserDoesntExist, 0, 0));
        }
    }
}
