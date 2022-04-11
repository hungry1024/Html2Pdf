using Dinosaur.DinkToPdf.Contracts;
using System;

namespace Dinosaur.DinkToPdf.EventDefinitions
{
    public class FinishedArgs : EventArgs
    {
        public IDocument Document { get; set; }

        public bool Success { get; set; }
    }
}
