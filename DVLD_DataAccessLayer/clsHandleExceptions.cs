using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DVLD_DataAccessLayer
{
    internal class clsHandleExceptions
    {
        private const string EventSource = "DVLD_Project_DAL";
        private const string EventLogName = "Application";

        static clsHandleExceptions()
        {
            // Ensure the event source exists
            if (!EventLog.SourceExists(EventSource))
            {
                EventLog.CreateEventSource(EventSource, EventLogName);
            }
        }

        /// <summary>
        /// Logs exceptions to the Windows Event Log.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="additionalInfo">Optional additional information about the context.</param>
        /// <param name="methodName">The name of the method where the exception occurred. Automatically populated.</param>
        public static void LogException(
            Exception ex,
            string additionalInfo = "",
            [CallerMemberName] string methodName = "")
        {
            string message = $"Error in method: {methodName}\n" +
                             $"Exception: {ex.Message}\n" +
                             $"Stack Trace: {ex.StackTrace}\n" +
                             (!string.IsNullOrEmpty(additionalInfo) ? $"Additional Info: {additionalInfo}" : "");

            EventLog.WriteEntry(EventSource, message, EventLogEntryType.Error);
        }
    }
}
