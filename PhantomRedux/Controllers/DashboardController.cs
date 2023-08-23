using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using PhantomRedux;
using PhantomRedux.Response;

namespace PhantomRedux.Controllers
{
    [ApiController]
    [Route("Dashboard")]
    public class DashboardController
    {
        [HttpGet]
        [Route("resetDatabase")]
        public IActionResult ResetDatabase(string privilegedRoutePassword,
            bool config = false,
            bool players = false,
            bool sessions = false,
            bool episodes = false)
        {
            if (privilegedRoutePassword != "TemporaryPassword") // TODO: This should be configurable!
            {
                return new JsonResult(new BaseResponse(GameStatusCode.Err_Other, 0, 0));
            }

            try
            {
                Db.ResetDatabase(config, players, sessions, episodes);
                return new JsonResult(new BaseResponse(GameStatusCode.Success, 0, 0));
            }
            catch (MySqlException e)
            {
                return new JsonResult(new PRExceptionResponse(e, GameStatusCode.Err_ReturnException, 0, 0));
            }
        }

        [HttpPost]
        [Route("setDatabaseDetails")]
        public IActionResult SetDatabaseDetails(string privilegedRoutePassword, string host, string port, string username, string password, string database)
        {
            if (privilegedRoutePassword != "TemporaryPassword") // TODO: This should be configurable!
            {
                return new JsonResult(new BaseResponse(GameStatusCode.Err_Other, 0, 0));
            }
            try
            {
                Db.SetDetails(host, port, username, password, database);
                return new JsonResult(new BaseResponse(GameStatusCode.Success, 0, 0));
            }
            catch (MySqlException e)
            {
                return new JsonResult(new PRExceptionResponse(e, GameStatusCode.Err_ReturnException, 0, 0));
            }
        }
    }
}
