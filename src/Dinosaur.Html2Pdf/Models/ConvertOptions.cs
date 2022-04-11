namespace Dinosaur.Html2Pdf.Models
{
    /// <summary>
    /// 转换参数
    /// </summary>
    public class ConvertOptions
    {
        /// <summary>
        /// 可访问的url（优先）
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// html内容
        /// </summary>
        public string HtmlContent { get; set; }

        /// <summary>
        /// pdf文件的文档名称
        /// </summary>
        public string DocumentTitle { get; set; }

        /// <summary>
        /// 文件下载名称
        /// </summary>
        public string FileDownloadName { get; set; }
    }
}
