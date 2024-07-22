using ImageMagick;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace CustomRouteHandler.Models.Handlers
{
	public class ImageHandler
	{
		public RequestDelegate Handle(string filePath)
		{
			return async c =>
			{
				FileInfo fileInfo = new FileInfo($"{filePath}//{c.Request.RouteValues["image"]}");

				if (!fileInfo.Exists)
				{
					c.Response.StatusCode = 404;
					await c.Response.WriteAsync("Resim Bulunamadi ");
					return;
				}

				if (string.IsNullOrEmpty(fileInfo.Extension))
				{
					c.Response.StatusCode = 400;
					await c.Response.WriteAsync("Gecerisz Image Turu");
					return;
				}

				try
				{
					using (MagickImage magick = new MagickImage(fileInfo))
					{
						int width = magick.Width, height = magick.Height;

						if (!string.IsNullOrEmpty(c.Request.Query["w"].ToString()))
						{
							width = int.Parse(c.Request.Query["w"].ToString());
						}
						if (!string.IsNullOrEmpty(c.Request.Query["h"].ToString()))
						{
							height = int.Parse(c.Request.Query["h"].ToString());
						}
						magick.Resize(width, height);

						var buffer = magick.ToByteArray();
						c.Response.Clear();
						c.Response.ContentType = $"image/{fileInfo.Extension.Replace(".", "")}";

						await c.Response.Body.WriteAsync(buffer, 0, buffer.Length);
						//await c.Response.WriteAsync(filePath); // Gerekli değilse kaldırabilirsiniz
					}
				}
				catch (Exception ex)
				{
					c.Response.StatusCode = 500;
					await c.Response.WriteAsync($"Internal Server Error: {ex.Message}");
				}
			};
		}
	}
}
