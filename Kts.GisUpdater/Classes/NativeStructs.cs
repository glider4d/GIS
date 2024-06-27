using System.Runtime.InteropServices;

namespace Kts.GisUpdater
{
    /// <summary>
    /// Нативные структуры.
    /// </summary>
    internal static class NativeStructs
    {
        #region Нативные структуры

        /// <summary>
        /// Структура USE_INFO_2.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct USE_INFO_2
        {
            /// <summary>
            /// ???.
            /// </summary>
            internal string ui2_local;

            /// <summary>
            /// ???.
            /// </summary>
            internal string ui2_remote;

            /// <summary>
            /// ???.
            /// </summary>
            internal string ui2_password;

            /// <summary>
            /// ???.
            /// </summary>
            internal uint ui2_status;

            /// <summary>
            /// ???.
            /// </summary>
            internal uint ui2_asg_type;

            /// <summary>
            /// ???.
            /// </summary>
            internal uint ui2_refcount;

            /// <summary>
            /// ???.
            /// </summary>
            internal uint ui2_usecount;

            /// <summary>
            /// ???.
            /// </summary>
            internal string ui2_username;

            /// <summary>
            /// ???.
            /// </summary>
            internal string ui2_domainname;
        }

        #endregion
    }
}