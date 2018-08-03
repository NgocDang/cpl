using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace CPL.SmartContractService.Misc
{
    public class Utils
    {
        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();

        public static void FileAppendThreadSafe(string path, string message)
        {
            // Set Status to Locked
            _readWriteLock.EnterWriteLock();
            try
            {
                File.AppendAllText(path, message);
            }
            finally
            {
                // Release lock
                _readWriteLock.ExitWriteLock();
            }
        }
    }
}
