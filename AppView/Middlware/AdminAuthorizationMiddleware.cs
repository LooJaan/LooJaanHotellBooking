using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace AppView.Middlware
{
    public class AdminAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;

            // Kiểm tra nếu request thuộc khu vực Admin
            if (path.StartsWithSegments("/Admin", StringComparison.OrdinalIgnoreCase))
            {
                var userRole = context.Session.GetString("Quyen");

                if (string.IsNullOrEmpty(userRole) || userRole != "admin")
                {
                    // Redirect đến trang đăng nhập nếu không phải admin
                    context.Response.Redirect("/Login/Login");
                    return;
                }
            }
			if (path.StartsWithSegments("/NhanVien", StringComparison.OrdinalIgnoreCase))
			{
				var userRole = context.Session.GetString("Quyen");

				if (string.IsNullOrEmpty(userRole) || userRole != "nhanvien")
				{
					// Redirect đến trang đăng nhập nếu không phải admin
					context.Response.Redirect("/Login/Login");
					return;
				}
			}

			await _next(context);
        }
    }
}
