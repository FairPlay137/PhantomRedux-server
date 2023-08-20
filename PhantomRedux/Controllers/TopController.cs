using Microsoft.AspNetCore.Mvc;
using PhantomRedux.Request;
using PhantomRedux.Response;
using PhantomRedux.DebugHelpers;
using System.Text.Json;

namespace PhantomRedux.Controllers
{
    [ApiController]
    [Route("intl/top")]
    public class TopController : ControllerBase
    {
        /// <summary>
        /// Lets the game check the online service status. Agnostic response mirrors original server behavior.
        /// </summary>
        [HttpPost]
        [Route("status")]
        [Produces("text/json")]
        public JsonResult Status([FromForm] string post)
        {
            CommonRequest? requestData = JsonSerializer.Deserialize<CommonRequest>(post);
            if (requestData == null) {
                return new JsonResult(new BaseResponse(GameStatusCode.Err_JsonAnalysisFailed, 0, 0));
            }

            DebugHelper.Log($"User ID: {requestData.common.user_id}", 1);
            DebugHelper.Log($"Session ID: {requestData.common.session_id}", 1);
            DebugHelper.Log($"Lang: {requestData.common.lang}", 1);
            DebugHelper.Log($"Platform: {requestData.common.platform}", 1);

            return new JsonResult(new BaseResponse());
        }
    }
}
