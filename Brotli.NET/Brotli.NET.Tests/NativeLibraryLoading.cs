﻿namespace Brotli.NET.Tests
{
    public class NativeLibraryLoading
    {
        [Fact]
        public void TestLibraryLoads()
        {
            IntPtr res = Brolib.BrotliEncoderCreateInstance();

            Assert.NotEqual(IntPtr.Zero, res); // A valid pointer must be returned
        }
    }
}
