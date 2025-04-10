namespace Brotli.NET.Tests
{
    public class NativeLibraryLoading
    {
        [Fact]
        public void TestLibraryLoads()
        {
            IntPtr res = Brolib.BrotliEncoderCreateInstance();

            Assert.NotEqual(IntPtr.Zero, res); // A valid pointer must be returned
        }

        /// <summary>
        /// Statically set from the native library everytime it is updated.
        /// </summary>
        /// <remarks>See <see href="https://github.com/Yellow-Dog-Man/brotli/blob/master/c/common/version.h#L24">brotli source</see> for its definition.
        /// This tests ensures that CI and therefore the published nuget function as expected as the same version is expected on all platforms.
        /// </remarks>
        public const uint NATIVE_LIBRARY_VERSION = 16781312;
        [Fact]
        public void TestLibraryVersion()
        {
            var decodeVersion = Brolib.BrotliDecoderVersion();
            var encodeVersion = Brolib.BrotliEncoderVersion();

            // we expect these to be the same version as our native library is compiled with both in one file
            // If this isn't the case then something is very wrong.
            Assert.Equal(decodeVersion, encodeVersion);

            Assert.Equal(NATIVE_LIBRARY_VERSION, encodeVersion);
        }
    }
}
