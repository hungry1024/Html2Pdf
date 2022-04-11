using System.Diagnostics.CodeAnalysis;

namespace Dinosaur.Html2Pdf.Models
{
    /// <summary>
    /// html片段合并的结果
    /// </summary>
    public class MergedHtml
    {
        /// <summary>
        /// 合并产生的key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 有效期（秒）
        /// </summary>
        public int ExpiresIn { get; set; }
    }
}
