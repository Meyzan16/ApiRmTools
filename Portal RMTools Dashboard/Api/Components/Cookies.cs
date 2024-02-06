namespace Api.Components
{
    public interface ICookies
    {
        Task<(bool status, string error)> SetTokenCookies(string cookieName, string token, DateTime expires);

    }

    public class Cookies : ICookies
    {
        private readonly IHttpContextAccessor _accessor;

        public Cookies(IHttpContextAccessor accessor )
        {
            _accessor = accessor;
        }

        public async Task<(bool status, string error)> SetTokenCookies(string cookieName, string token, DateTime expires)
        {
            try
            {
                // Set cookie options (customize based on your requirements)
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax,
                    Expires = expires
                };

                if (GetConfig.AppSetting["env"] == "Production" && _accessor.HttpContext.Request.IsHttps)
                {
                    cookieOptions.Secure = true;
                }

                _accessor.HttpContext.Response.Cookies.Append(cookieName, token, cookieOptions);

                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString());
            }
        }

    }
}
