using System;
using System.Runtime.InteropServices;

namespace Brotli
{
    static class WindowsLoader
    {
        public const string LIBRARY_NAME = "kernel32.dll";
        [DllImport(LIBRARY_NAME, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string dllFilePath);

        [DllImport(LIBRARY_NAME, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport(LIBRARY_NAME)]
        public static extern bool FreeLibrary(IntPtr hModule);
    }

    static class LinuxLoader
    {
        // Loosly based on: https://stackoverflow.com/questions/9954548/sigsegv-when-p-invoking-dlopen
        // https://github.com/search?q=dlopen+language%3AC%23+&type=code 
        public const string LIBRARY_NAME = "libdl.so.2"; 
        [DllImport(LIBRARY_NAME)]
        internal static extern IntPtr dlopen(string filename, int flags);

        [DllImport(LIBRARY_NAME)]
        internal static extern IntPtr dlerror();

        [DllImport(LIBRARY_NAME)]
        internal static extern IntPtr dlsym(IntPtr handle, string symbol);

        [DllImport(LIBRARY_NAME)]
        internal static extern int dlclose(IntPtr handle);
    }

    static class MacOSXLoader
    {
        public const string LIBRARY_NAME = "libSystem.dylib";
        [DllImport(LIBRARY_NAME)]
        internal static extern IntPtr dlopen(string filename, int flags);

        [DllImport(LIBRARY_NAME)]
        internal static extern IntPtr dlerror();

        [DllImport(LIBRARY_NAME)]
        internal static extern IntPtr dlsym(IntPtr handle, string symbol);

        [DllImport(LIBRARY_NAME)]
        internal static extern int dlclose(IntPtr handle);

    }

    /// <summary>
    /// Similarly as for Mono on Linux, we load symbols for
    /// dlopen and dlsym from the "libcoreclr.so",
    /// to avoid the dependency on libc-dev Linux.
    /// </summary>
    static class CoreCLRLoader
    {
        public const string LIBRARY_NAME = "libcoreclr.so";
        [DllImport(LIBRARY_NAME)]
        internal static extern IntPtr dlopen(string filename, int flags);

        [DllImport(LIBRARY_NAME)]
        internal static extern IntPtr dlerror();

        [DllImport(LIBRARY_NAME)]
        internal static extern IntPtr dlsym(IntPtr handle, string symbol);

        [DllImport(LIBRARY_NAME)]
        internal static extern int dlclose(IntPtr handle);
    }
}
