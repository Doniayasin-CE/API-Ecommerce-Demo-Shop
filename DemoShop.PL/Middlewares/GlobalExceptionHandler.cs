using DemoShop.DAL.DTO.Response;

namespace DemoShop.PL.Middlewares
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }catch (Exception ex)
            {
                var error = new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Server Error ...",
                    InnerError = ex.Message
                };
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
