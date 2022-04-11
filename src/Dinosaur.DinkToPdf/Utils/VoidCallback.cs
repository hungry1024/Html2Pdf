using System;
using System.Runtime.InteropServices;

namespace Dinosaur.DinkToPdf
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void VoidCallback(IntPtr converter);
}
