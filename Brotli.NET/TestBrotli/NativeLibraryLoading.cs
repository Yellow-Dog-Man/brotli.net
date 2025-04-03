using Brotli;
using System;
using Xunit;

namespace TestBrotli
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
