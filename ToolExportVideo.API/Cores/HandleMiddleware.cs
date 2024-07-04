using ToolExportVideo.Common;

namespace ToolExportVideo.API
{
    public  class HandleMiddleware
    {
        private readonly RequestDelegate _next;


        public HandleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            AuthozirationUtility.SetHttpContext(context);
            await _next(context);
        }
    }


}
