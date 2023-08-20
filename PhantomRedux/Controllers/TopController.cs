using Microsoft.AspNetCore.Mvc;
using PhantomRedux.Response;

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
        public JsonResult Status()
        {
            return new JsonResult(new BaseResponse());
        }
    }
}
