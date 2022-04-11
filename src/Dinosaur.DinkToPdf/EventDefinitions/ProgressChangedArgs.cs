using Dinosaur.DinkToPdf.Contracts;
using System;

namespace Dinosaur.DinkToPdf.EventDefinitions
{
    public class ProgressChangedArgs : EventArgs
    {
        public IDocument Document { get; set; }

        public string Description { get; set; }
    }
}
