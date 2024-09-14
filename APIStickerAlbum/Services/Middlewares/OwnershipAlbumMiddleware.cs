using APIStickerAlbum.Interfaces;

namespace APIStickerAlbum.Services.Middlewares;

public class OwnershipAlbumMiddleware
{
    private readonly RequestDelegate _next;

    public OwnershipAlbumMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        if (context.Request.Path.StartsWithSegments("/api/albums") && (context.Request.Method == HttpMethods.Get
                                                                        || context.Request.Method == HttpMethods.Put
                                                                        || context.Request.Method == HttpMethods.Delete))
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var _currentUserService = scope.ServiceProvider.GetRequiredService<ICurrentUserService>();
                var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var albumId = context.Request.RouteValues["id"]?.ToString();
                var userId = _currentUserService.GetUserId();
                var userType = _currentUserService.GetUserType();

                if (albumId is not null)
                {
                    if (userId is null || _unitOfWork.AlbumRepository.GetAlbumByAuthenticatedUser(int.Parse(albumId), int.Parse(userId), userType) is null)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Você não possui permissão para acessar esse recurso");
                        return;
                    }
                }

            }
        }
        await _next(context);
    }
}
