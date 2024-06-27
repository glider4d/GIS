using System.Runtime.InteropServices;

namespace Kts.GisUpdater
{
    /// <summary>
    /// Нативные методы.
    /// </summary>
    internal static class NativeMethods
    {
        #region Нативные функции

        /// <summary>
        /// ???.
        /// </summary>
        /// <param name="UncServerName">???.</param>
        /// <param name="Level">???.</param>
        /// <param name="Buf">???.</param>
        /// <param name="ParmError">???.</param>
        /// <returns>???.</returns>
        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern uint NetUseAdd(string UncServerName, uint Level, ref NativeStructs.USE_INFO_2 Buf, out uint ParmError);

        /// <summary>
        /// ???.
        /// </summary>
        /// <param name="UncServerName">???.</param>
        /// <param name="UseName">???.</param>
        /// <param name="ForceCond">???.</param>
        /// <returns>???.</returns>
        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern uint NetUseDel(string UncServerName, string UseName, uint ForceCond);

        #endregion
    }
}