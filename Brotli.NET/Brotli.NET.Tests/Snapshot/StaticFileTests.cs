namespace Brotli.NET.Tests.Snapshot
{
    public static class Fixtures
    {
        public const string UncompressedPath = "Resource/BingCN.bin";
        public const string CompressedPath = "Resource/BingCN_Compressed.bin";

        // Copied from original tests but now preserved as constant: https://github.com/Yellow-Dog-Man/brotli.net/commit/0f4bc44c8466cc19777612639ca225de7cbcd644
        public const int FIXTURE_QUALITY = 11;
        public const int FIXTURE_WINDOW = 22;
    }

    public class StaticFileTests
    {
        [Fact]
        public Task Run() => VerifyChecks.Run();

        public static byte[] GetBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        [Fact]
        public Task TestCompress()
        {
            var input = GetBytes(Fixtures.UncompressedPath);
            var output = input.CompressToBrotli(Fixtures.FIXTURE_QUALITY, Fixtures.FIXTURE_WINDOW);
            return Verify(output);
        }

        [Fact]
        public Task TestDecompress()
        {
            var input = GetBytes(Fixtures.CompressedPath);
            var output = input.DecompressFromBrotli();
            return Verify(output);
        }

        [Fact]
        public void TestRoundTrip()
        {
            var input = GetBytes(Fixtures.UncompressedPath);
            var compressed = input.CompressToBrotli(Fixtures.FIXTURE_QUALITY, Fixtures.FIXTURE_WINDOW);
            var decompressed = compressed.DecompressFromBrotli();
            Assert.Equal(input, decompressed);
        }
    }
}
