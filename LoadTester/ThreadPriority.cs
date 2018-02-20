using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoadTester
{
    public enum ThreadPriority : int
    {
        THREAD_PRIORITY_IDLE = WinNTThreadPriority.THREAD_BASE_PRIORITY_IDLE
        ,
        THREAD_PRIORITY_LOWEST = WinNTThreadPriority.THREAD_BASE_PRIORITY_MIN
        ,
        THREAD_PRIORITY_BELOW_NORMAL = (THREAD_PRIORITY_LOWEST+1)
        ,
        THREAD_PRIORITY_NORMAL = 0
        ,
        THREAD_PRIORITY_ABOVE_NORMAL = (THREAD_PRIORITY_HIGHEST-1)
        ,
        THREAD_PRIORITY_HIGHEST = WinNTThreadPriority.THREAD_BASE_PRIORITY_MAX
        ,
        THREAD_PRIORITY_TIME_CRITICAL = WinNTThreadPriority.THREAD_BASE_PRIORITY_LOWRT
//------------------------------------
        /*
 ,THREAD_BASE_PRIORITY_LOWRT=  15  // value that gets a thread to LowRealtime-1
 ,THREAD_BASE_PRIORITY_MAX  =  2   // maximum thread base priority boost
 ,THREAD_BASE_PRIORITY_MIN  =  (-2)  // minimum thread base priority boost
 ,THREAD_BASE_PRIORITY_IDLE =  (-15) // value that gets a thread to idle

        #define THREAD_PRIORITY_LOWEST          THREAD_BASE_PRIORITY_MIN
        #define THREAD_PRIORITY_BELOW_NORMAL    (THREAD_PRIORITY_LOWEST+1)
        #define THREAD_PRIORITY_NORMAL          0
        #define THREAD_PRIORITY_HIGHEST         THREAD_BASE_PRIORITY_MAX
        #define THREAD_PRIORITY_ABOVE_NORMAL    (THREAD_PRIORITY_HIGHEST-1)
        #define THREAD_PRIORITY_ERROR_RETURN    (MAXLONG)
        */
    
    }

    public enum WinNTThreadPriority :int
    {
        THREAD_BASE_PRIORITY_LOWRT = 15 // value that gets a thread to LowRealtime-1
        ,
        THREAD_BASE_PRIORITY_MAX = 2 // maximum thread base priority boost
        ,
        THREAD_BASE_PRIORITY_MIN = (-2) // minimum thread base priority boost
        ,
        THREAD_BASE_PRIORITY_IDLE = (-15) // value that gets a thread to idle
    }
}
