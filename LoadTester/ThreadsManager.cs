using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Xml;

namespace LoadTester
{
    public static class ThreadsManager
    {
        public const int LastMeasurementsCount = (int) (3 * 1000);
        private const int Period = 10;
        private static IList<ThreadWrapper> m_threadWrappers;

        private static long s_totalElapsed = 0;
        private static long[] s_times;

        private static readonly double[][] EmptyDoubles = new double[0][];
        private static IntPtr s_waitableTimer;
        private static volatile int m_threadCollectionBusy;
        private static volatile bool s_disposed;


        public static long[] Times
        {
            get { return s_times; }
        }

        public static IList<ThreadWrapper> ThreadWrappers
        {
            get { return m_threadWrappers; }
        }

        public static void StartAll()
        {
            foreach (var threadWrapper in m_threadWrappers)
            {
                threadWrapper.Start();
            }
        }

        public static void StopAll()
        {
            foreach (var threadWrapper in m_threadWrappers)
            {
                threadWrapper.Stop();
            }
        }

        public static void FinishWork()
        {
            s_disposed = true;
        }

        public static void Init()
        {
            s_times = new long[LastMeasurementsCount];
            m_threadWrappers = new List<ThreadWrapper>();

            s_waitableTimer = NativeMethods.CreateWaitableTimer(default(IntPtr), false, "ThreadManager");

            m_stopwatch.Restart();
            var lPeriod = Period;
            long pDueTime = lPeriod*10000;
            NativeMethods.SetWaitableTimer(s_waitableTimer, ref pDueTime, lPeriod, null, IntPtr.Zero, false);

            for (int index = 0; index < s_times.Length; index++)
                s_times[index] = -1;

            var thread = new Thread(
                UpdateSpeeds);
            thread.Start();
            thread.Priority = System.Threading.ThreadPriority.Highest;
        }

        private static Stopwatch m_stopwatch = new Stopwatch();
        private static void UpdateSpeeds()
        {
            var intervalIndex = 0;
            long milliseconds;
            while (false == s_disposed)
            {
                m_stopwatch.Restart();
                NativeMethods.WaitForSingleObject(s_waitableTimer, NativeMethods.INFINITE);

                milliseconds = m_stopwatch.ElapsedMilliseconds;
                s_totalElapsed += milliseconds;
                if (milliseconds < Period)
                    continue;

                s_times[intervalIndex] = s_totalElapsed;
                if (intervalIndex > 0)
                {
                    var previousTotalElapsed = s_times[intervalIndex - 1];
                    milliseconds = s_totalElapsed - previousTotalElapsed;
                }
                ++intervalIndex;
                bool needShifting = false;
                if (intervalIndex == s_times.Length)
                {
                    intervalIndex = s_times.Length-1;
                    needShifting = true;
                    ShiftArrayValues(s_times);
                }

#pragma warning disable 420
                //                       M
                //                      dM
                //                      MMr
                //                     4MMML                  .
                //                     MMMMM.                xf
                //     .              "MMMMM               .MM-
                //      Mh..          +MMMMMM            .MMMM
                //      .MMM.         .MMMMML.          MMMMMh
                //       )MMMh.        MMMMMM         MMMMMMM
                //        3MMMMx.     'MMMMMMf      xnMMMMMM"
                //        '*MMMMM      MMMMMM.     nMMMMMMP"
                //          *MMMMMx    "MMMMM\    .MMMMMMM=
                //           *MMMMMh   "MMMMM"   JMMMMMMP
                //             MMMMMM   3MMMM.  dMMMMMM            .
                //              MMMMMM  "MMMM  .MMMMM(        .nnMP"
                //  =..          *MMMMx  MMM"  dMMMM"    .nnMMMMM*
                //    "MMn...     'MMMMr 'MM   MMM"   .nMMMMMMM*"
                //     "4MMMMnn..   *MMM  MM  MMP"  .dMMMMMMM""
                //       ^MMMMMMMMx.  *ML "M .M*  .MMMMMM**"
                //          *PMMMMMMhn. *x > M  .MMMM**""
                //             ""**MMMMhx/.h/ .=*"
                //                      .3P"%....
                //                    nP"     "*MMnx
                while (0 != Interlocked.CompareExchange(ref m_threadCollectionBusy, 1, 0))
                    Thread.SpinWait(100);
#pragma warning restore 420

                for (int i = 0; i < ThreadWrappers.Count; i++)
                {
                    var threadWrapper = ThreadWrappers[i];
                    var wrapperLooped = threadWrapper.Looped;
                    double threadWrapperLooped = wrapperLooped - threadWrapper.LoopedPreviously;
                    threadWrapper.Speeds[intervalIndex] = threadWrapperLooped/milliseconds;
                    threadWrapper.LoopedPreviously = wrapperLooped;

                    if (needShifting)
                        ShiftArrayValues(threadWrapper.Speeds);
                }
                m_threadCollectionBusy = 0;

            }
            NativeMethods.CloseHandle(s_waitableTimer);
        }

        private static void ShiftArrayValues<T>(T[] p_array)
        {
            for (int i = 1; i < p_array.Length; i++)
            {
                var interval = p_array[i];
                p_array[i - 1] = interval;
            }
        }

        public static ThreadWrapper[] AddThreads(int p_count)
        {
            var result = new ThreadWrapper[p_count];
            for (int i = 0; i < p_count; i++)
            {
                 
                var newThreadWrapper = new ThreadWrapper();
                result[i] = newThreadWrapper;
#pragma warning disable 420
                //                       M
                //                      dM
                //                      MMr
                //                     4MMML                  .
                //                     MMMMM.                xf
                //     .              "MMMMM               .MM-
                //      Mh..          +MMMMMM            .MMMM
                //      .MMM.         .MMMMML.          MMMMMh
                //       )MMMh.        MMMMMM         MMMMMMM
                //        3MMMMx.     'MMMMMMf      xnMMMMMM"
                //        '*MMMMM      MMMMMM.     nMMMMMMP"
                //          *MMMMMx    "MMMMM\    .MMMMMMM=
                //           *MMMMMh   "MMMMM"   JMMMMMMP
                //             MMMMMM   3MMMM.  dMMMMMM            .
                //              MMMMMM  "MMMM  .MMMMM(        .nnMP"
                //  =..          *MMMMx  MMM"  dMMMM"    .nnMMMMM*
                //    "MMn...     'MMMMr 'MM   MMM"   .nMMMMMMM*"
                //     "4MMMMnn..   *MMM  MM  MMP"  .dMMMMMMM""
                //       ^MMMMMMMMx.  *ML "M .M*  .MMMMMM**"
                //          *PMMMMMMhn. *x > M  .MMMM**""
                //             ""**MMMMhx/.h/ .=*"
                //                      .3P"%....
                //                    nP"     "*MMnx
                while (0 != Interlocked.CompareExchange(ref m_threadCollectionBusy, 1, 0))
                    Thread.SpinWait(100);
#pragma warning restore 420
                m_threadWrappers.Add(newThreadWrapper);
                m_threadCollectionBusy = 0;
            }
            return result;
        }
    }
}
