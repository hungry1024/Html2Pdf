using Dinosaur.DinkToPdf.Contracts;
using System;

namespace Dinosaur.DinkToPdf.EventDefinitions
{
    public class WarningArgs : EventArgs
    {
        public IDocument Document { get; set; }

        public string Message { get; set; }
    }
}
