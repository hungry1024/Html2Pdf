using Dinosaur.Html2Pdf.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Text;

namespace Dinosaur.Html2Pdf.Controllers
{
    /// <summary>
    /// 文件合并组织
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class MergerController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="cache"></param>
        public MergerController(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// html文件合并，按文件名升序逐个拼在一起
        /// </summary>
        /// <param name="htmlSlices">待合并的html文件集合</param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult Html(IFormFileCollection htmlSlices)
        {
            if (htmlSlices == null
               || htmlSlices.Count == 0
               || htmlSlices.Any(f => f.Length == 0L)
               || htmlSlices.Any(f => !".html".Equals(Path.GetExtension(f.FileName), StringComparison.OrdinalIgnoreCase))
               )
            {
                return BadRequest($"{nameof(htmlSlices)} is required.");
            }

            string saveDirectory = DateTime.Now.ToString("yyyyMM");
            var di = Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "temp", saveDirectory));

            string mergeFileName = $"{RandomNumberGenerator.GetInt32(11200, 100000)}.html";

            using var mfs = System.IO.File.Create(Path.Combine(di.FullName, mergeFileName));

            foreach (var file in htmlSlices.OrderBy(f => f.FileName))
            {
                using var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8);
                string text = reader.ReadToEnd();
                var buffer = Encoding.UTF8.GetBytes(text);
                mfs.Write(buffer, 0, buffer.Length);
            }

            string htmlPath = $"temp/{saveDirectory}/{mergeFileName}";

            string key = Guid.NewGuid().ToString("n");

            _cache.Set($"temp:{key}", htmlPath, TimeSpan.FromSeconds(600));

            return Ok(new MergedHtml
            {
                Key = key,
                ExpiresIn = 600
            });
        }

        /// <summary>
        /// 合并后的预览
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Preview([FromQuery] string key)
        {
            if (key == null || key.Length != 32)
            {
                return NotFound();
            }

            if (!_cache.TryGetValue($"temp:{key}", out string path))
            {
                return NotFound();
            }

            string fullPath = Path.Combine(AppContext.BaseDirectory, path);
            string html = System.IO.File.ReadAllText(fullPath);
            return Content(html, "text/html", Encoding.UTF8);
        }

    }
}
