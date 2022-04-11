using System.Collections.Generic;

namespace Dinosaur.DinkToPdf.Contracts
{
    public interface IDocument : ISettings
    {
        IEnumerable<IObject> GetObjects();
    }
}
