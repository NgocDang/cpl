using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

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

        public static string AddOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }

        }

        public static double ToJSTimeStamp(DateTime dt)
        {
            return dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public static string ConvertObjToQueryString(object obj)
        {
            var result = new List<string>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj))
            {
                result.Add(property.Name + "=" + property.GetValue(obj));
            }

            return string.Join("&", result);
        }
    }
}
