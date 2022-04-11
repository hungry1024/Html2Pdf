using Dinosaur.ChromeToPdf;
using Dinosaur.ChromeToPdf.Settings;
using Dinosaur.DinkToPdf;
using Dinosaur.DinkToPdf.Contracts;
using Dinosaur.Html2Pdf.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Dinosaur.Html2Pdf.Controllers
{
    /// <summary>
    /// Html 转 pdf
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class Html2PdfController : ControllerBase
    {
        private readonly IConverter _converter;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="converter"></param>
        public Html2PdfController(IConverter converter)
        {
            _converter = converter;
        }

        /// <summary>
        /// 基于 wkhtml 的方式
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [HttpPost]
        public FileResult Wkhtml(ConvertOptions opts)
        {
            if (string.IsNullOrEmpty(opts.Url) && string.IsNullOrEmpty(opts.HtmlContent))
            {
                throw new ArgumentException("传入参数Url与HtmlContent不能同时为空");
            }

            if (!string.IsNullOrEmpty(opts.HtmlContent))
            {
                opts.Url = string.Empty;
            }

            if (!string.IsNullOrEmpty(opts.Url))
            {
                if (!opts.Url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !opts.Url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException("传入参数Url格式不正确");
                }
            }

            if (string.IsNullOrEmpty(opts.FileDownloadName))
            {
                opts.FileDownloadName = $"{DateTime.Now:yyyyMMddHHmmss}.pdf";
            }
            else
            {
                if (!opts.FileDownloadName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    opts.FileDownloadName = opts.FileDownloadName + ".pdf";
                }
            }

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = opts.DocumentTitle
            };
            var objectSettings = new ObjectSettings
            {
                Page = opts.Url,
                HtmlContent = opts.HtmlContent,
                PagesCount = true,
                WebSettings = { DefaultEncoding = "utf-8" },
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var bytes = _converter.Convert(pdf);

            return File(bytes, "application/pdf", opts.FileDownloadName);
        }

        /// <summary>
        /// 基于 google-chrome 打印的方式
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Chrome(ConvertOptions opts)
        {
            if (string.IsNullOrEmpty(opts.Url) && string.IsNullOrEmpty(opts.HtmlContent))
            {
                throw new ArgumentException("传入参数Url与HtmlContent不能同时为空");
            }

            if (!string.IsNullOrEmpty(opts.HtmlContent))
            {
                opts.Url = string.Empty;
            }

            if (!string.IsNullOrEmpty(opts.Url))
            {
                if (!opts.Url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !opts.Url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException("传入参数Url格式不正确");
                }
            }

            if (string.IsNullOrEmpty(opts.FileDownloadName))
            {
                opts.FileDownloadName = $"{DateTime.Now:yyyyMMddHHmmss}.pdf";
            }
            else
            {
                if (!opts.FileDownloadName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    opts.FileDownloadName = opts.FileDownloadName + ".pdf";
                }
            }

            try
            {
                using (var converter = new Converter())
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        converter.AddChromeArgument("--no-sandbox");
                    }

                    using var ms = new MemoryStream();

                    if (string.IsNullOrEmpty(opts.Url))
                    {
                        converter.ConvertToPdf(opts.HtmlContent, ms, new PageSettings());
                    }
                    else
                    {
                        converter.ConvertToPdf(new ConvertUri(opts.Url), ms, new PageSettings());
                    }
                    return File(ms.ToArray(), "application/pdf", opts.FileDownloadName);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }
    }
}
