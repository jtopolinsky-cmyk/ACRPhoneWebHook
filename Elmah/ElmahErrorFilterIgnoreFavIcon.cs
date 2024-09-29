namespace ACRPhoneWebHook.Elmah
{
    public class ElmahErrorFilterIgnoreFavIcon : ElmahCore.IErrorFilter
    {
        public void OnErrorModuleFiltering(object sender, ElmahCore.ExceptionFilterEventArgs args)
        {
            if (args.Exception.GetBaseException() is FileNotFoundException)
                args.Dismiss();

            if (args.Context is HttpContext httpContext)
            {
                if (httpContext.Response.StatusCode == 404 && httpContext.Request.Path == "/favicon.ico")
                    args.Dismiss();

            }
        }
    }
}
