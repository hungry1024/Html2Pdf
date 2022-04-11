using Dinosaur.DinkToPdf.Contracts;
using System;

namespace Dinosaur.DinkToPdf.EventDefinitions
{
    public class PhaseChangedArgs : EventArgs
    {
        public IDocument Document { get; set; }

        public int PhaseCount { get; set; }

        public int CurrentPhase { get; set; }

        public string Description { get; set; }
    }
}
