using System;
using System.Runtime.InteropServices;

namespace Brotli
{
    internal class BrolibNew
    {
        const string LIBRARY_NAME = "brolib";

        #region Encoder
        [DllImport(LIBRARY_NAME)]
        internal static extern IntPtr BrotliEncoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);

        [DllImport(LIBRARY_NAME)]
        internal static extern bool BrotliEncoderSetParameter(IntPtr state, BrotliEncoderParameter parameter, UInt32 value);

        [DllImport(LIBRARY_NAME)]
        internal static extern bool BrotliEncoderCompressStream(
            IntPtr state, BrotliEncoderOperation op, ref UInt32 availableIn,
            ref IntPtr nextIn, ref UInt32 availableOut, ref IntPtr nextOut, out UInt32 totalOut);

        [DllImport(LIBRARY_NAME)]
        internal static extern bool BrotliEncoderIsFinished(IntPtr state);

        [DllImport(LIBRARY_NAME)]
        internal static extern void BrotliEncoderDestroyInstance(IntPtr state);

        [DllImport(LIBRARY_NAME)]
        internal static extern UInt32 BrotliEncoderVersion();

        [DllImport(LIBRARY_NAME)]
        internal static extern IntPtr BrotliEncoderTakeOutput(IntPtr state, ref UInt32 size);

        #endregion
        #region Decoder
        [DllImport(LIBRARY_NAME)]
        internal static extern IntPtr BrotliDecoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);

        [DllImport(LIBRARY_NAME)]
        internal static extern bool BrotliDecoderSetParameter(IntPtr state, BrotliDecoderParameter param, UInt32 value);

        [DllImport(LIBRARY_NAME)]
        internal static extern BrotliDecoderResult BrotliDecoderDecompressStream(
            IntPtr state, ref UInt32 availableIn, ref IntPtr nextIn,
            ref UInt32 availableOut, ref IntPtr nextOut, out UInt32 totalOut);

        [DllImport(LIBRARY_NAME)]
        internal static extern void BrotliDecoderDestroyInstance(IntPtr state);

        [DllImport(LIBRARY_NAME)]
        internal static extern UInt32 BrotliDecoderVersion();

        [DllImport(LIBRARY_NAME)]
        internal static extern bool BrotliDecoderIsUsed(IntPtr state);

        [DllImport(LIBRARY_NAME)]
        internal static extern bool BrotliDecoderIsFinished(IntPtr state);

        [DllImport(LIBRARY_NAME)]
        internal static extern Int32 BrotliDecoderGetErrorCode(IntPtr state);

        [DllImport(LIBRARY_NAME)]
        internal static extern IntPtr BrotliDecoderErrorString(Int32 code);

        [DllImport(LIBRARY_NAME)]
        internal static extern IntPtr BrotliDecoderTakeOutput(IntPtr state, ref UInt32 size);

        #endregion

    }
}



