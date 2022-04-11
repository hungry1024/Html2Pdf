﻿using System;

namespace Dinosaur.ChromeToPdf.Loggers
{
    /// <summary>
    ///     Writes log information to the console
    /// </summary>
    public class Console: Stream
    {
        public Console() : base(System.Console.OpenStandardOutput())
        {
        }
    }
}
