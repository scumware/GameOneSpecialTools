using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LoadTester
{
    public class Size32K
    {
        public Size4K Field000;
        public Size4K Field001;
        public Size4K Field002;
        public Size4K Field003;
        public Size4K Field004;
        public Size4K Field005;
        public Size4K Field006;
        public Size4K Field007;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Size4K
    {
        public Size512 Field000;
        public Size512 Field001;
        public Size512 Field002;
        public Size512 Field003;
        public Size512 Field004;
        public Size512 Field005;
        public Size512 Field006;
        public Size512 Field007;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Size512
    {
        public Size64 Field000;
        public Size64 Field001;
        public Size64 Field002;
        public Size64 Field003;
        public Size64 Field004;
        public Size64 Field005;
        public Size64 Field006;
        public Size64 Field007;
    }


    [StructLayout( LayoutKind.Sequential )]
    public struct Size64
    {
        public UInt64 Field000;
        public UInt64 Field001;
        public UInt64 Field002;
        public UInt64 Field003;
        public UInt64 Field004;
        public UInt64 Field005;
        public UInt64 Field006;
        public UInt64 Field007;
    }
}
