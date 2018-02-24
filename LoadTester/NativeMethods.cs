using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
// ReSharper disable InconsistentNaming

namespace LoadTester
{
    public static class NativeMethods
    {
        [DllImport( "user32.dll", CharSet = CharSet.Unicode )]
        public static extern void SendMessage( IntPtr p_hWnd, uint p_msg, IntPtr p_wParam, IntPtr p_lParam );

        public const uint WS_EX_COMPOSITED = 0x02000000;
        public const uint WM_SETREDRAW = 0xB;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern void DisableProcessWindowsGhosting();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32")]
        public static extern bool VirtualFree(IntPtr lpAddress, UInt32 dwSize, FreeType dwFreeType);

        [Flags()]
        public enum AllocationType : uint
        {
            COMMIT = 0x1000,
            RESERVE = 0x2000,
            RESET = 0x80000,
            LARGE_PAGES = 0x20000000,
            PHYSICAL = 0x400000,
            TOP_DOWN = 0x100000,
            WRITE_WATCH = 0x200000
        }

        [Flags()]
        public enum FreeType : uint
        {
            DECOMMIT = 0x4000,
            RELEASE = 0x8000
        }

        [Flags()]
        public enum MemoryProtection : uint
        {
            EXECUTE = 0x10,
            EXECUTE_READ = 0x20,
            EXECUTE_READWRITE = 0x40,
            EXECUTE_WRITECOPY = 0x80,
            NOACCESS = 0x01,
            READONLY = 0x02,
            READWRITE = 0x04,
            WRITECOPY = 0x08,
            GUARD_MODIFIERFLAG = 0x100,
            NOCACHE_MODIFIERFLAG = 0x200,
            WRITECOMBINE_MODIFIERFLAG = 0x400
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern unsafe uint CreateThread(
            uint* lpThreadAttributes,
            uint dwStackSize,
            ThreadStart lpStartAddress,
            uint* lpParameter,
            ThreadCreationFlags dwCreationFlags,
            out uint lpThreadId);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern unsafe uint ResumeThread(
            [In] uint hThread
        );

        [Flags]
        public enum ThreadCreationFlags : UInt32
        {
            CREATE_NORMAL = 0
            ,
            CREATE_SUSPENDED = 0x00000004
            ,
            STACK_SIZE_PARAM_IS_A_RESERVATION = 0x00010000
        }

        [DllImport( "kernel32.dll", SetLastError = true )]
        public static extern bool SetThreadPriority( IntPtr hThread, ThreadPriority nPriority );

        [DllImport( "kernel32.dll" )]
        public static extern UIntPtr SetThreadAffinityMask( IntPtr hThread,
            UIntPtr dwThreadAffinityMask );

        [DllImport( "kernel32.dll" )]
        public static extern uint GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentThread();

        [DllImport( "kernel32.dll", SetLastError = true )]
        public static extern IntPtr OpenThread( ThreadAccess dwDesiredAccess, bool bInheritHandle,
            uint dwThreadId );

        [DllImport( "kernel32.dll", SetLastError = true )]
        [ReliabilityContract( Consistency.WillNotCorruptState, Cer.Success )]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static extern bool CloseHandle( IntPtr hObject );

        [DllImport("winmm.dll", SetLastError = true)]
        private static extern UInt32 timeGetDevCaps(ref TimeCaps timeCaps,
                    UInt32 sizeTimeCaps);

        public static unsafe TimeCaps GetTimerDeviceCaps()
        {
            NativeMethods.TimeCaps timeCaps = new NativeMethods.TimeCaps();
            NativeMethods.timeGetDevCaps(ref timeCaps, (uint) sizeof(NativeMethods.TimeCaps));
            return timeCaps;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern unsafe IntPtr CreateWaitableTimer(
          [In][Optional] uint* lpTimerAttributes,
          [In] bool             bManualReset,
          [In] string lpTimerName
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern unsafe bool SetWaitableTimer(
          [In][Optional] IntPtr    hTimer,
          [In]           ref long  pDueTime,
          [In]           long lPeriod,
          [In][Optional] PTIMERAPCROUTINE pfnCompletionRoutine,
          [In][Optional] void* lpArgToCompletionRoutine,
          [In]           bool fResume
);

        public unsafe delegate void PTIMERAPCROUTINE(
          [In][Optional]  void* lpArgToCompletionRoutine,
          [In] ulong  dwTimerLowValue,
          [In] ulong dwTimerHighValue
        );

        [StructLayout(LayoutKind.Sequential)]
        public struct TimeCaps
        {
            public UInt32 wPeriodMin;
            public UInt32 wPeriodMax;
        };

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateWaitableTimer(IntPtr lpTimerAttributes, bool bManualReset, string lpTimerName);

        [DllImport("kernel32.dll")]
        public static extern bool SetWaitableTimer(IntPtr hTimer, [In] ref long pDueTime,
            int lPeriod, TimerCompleteDelegate pfnCompletionRoutine,
            IntPtr lpArgToCompletionRoutine, bool fResume);


        [DllImport("kernel32.dll")]
        public static extern bool CancelWaitableTimer(IntPtr hTimer);

        public delegate void TimerCompleteDelegate();


        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern Int32 WaitForSingleObject(IntPtr Handle, uint Wait);

        /// <summary>
        /// Yield execution to another thread. Better then Sleep(0)
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool SwitchToThread();

        [Flags]
        // ReSharper disable once BuiltInTypeReferenceStyle
        // ReSharper disable once EnumUnderlyingTypeIsInt
        public enum ThreadAccess :Int32
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }
    }
}
