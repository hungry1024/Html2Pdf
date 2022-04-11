using Dinosaur.DinkToPdf.Contracts;
using System.Collections.Generic;

namespace Dinosaur.DinkToPdf
{
    public class HtmlToPdfDocument : IDocument
    {
        public List<ObjectSettings> Objects { get; private set; }

        private GlobalSettings globalSettings = new GlobalSettings();

        public GlobalSettings GlobalSettings
        {
            get { return globalSettings; }
            set
            {
                globalSettings = value;
            }
        }

        public HtmlToPdfDocument()
        {
            Objects = new List<ObjectSettings>();
        }

        public IEnumerable<IObject> GetObjects()
        {
            return Objects.ToArray();
        }
    }
}
