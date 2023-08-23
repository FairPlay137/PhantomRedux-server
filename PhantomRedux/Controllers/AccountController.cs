using Microsoft.AspNetCore.Mvc;
using PhantomRedux.DebugHelpers;
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
            var requestData = JsonSerializer.Deserialize<RegisterRequest>(post);
            if (requestData == null)
            {
                return new JsonResult(new BaseResponse(GameStatusCode.Err_JsonAnalysisFailed, 0, 0));
            }
            DebugHelper.Log($"Registering new user with the \"{requestData.account.nick_name}\" nickname...", 1);
            return new JsonResult(new BaseResponse(GameStatusCode.Err_UserDoesntExist, 0, 0));
        }
    }
}
