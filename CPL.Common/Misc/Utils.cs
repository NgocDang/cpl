using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CPL.Common.Misc
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
