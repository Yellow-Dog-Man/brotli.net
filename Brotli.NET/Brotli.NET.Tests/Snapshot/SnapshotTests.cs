namespace Brotli.NET.Tests.Snapshot
{
    public static class Fixtures
    {
        public const string UncompressedPath = "Resource/BingCN.bin";
        public const string CompressedPath = "Resource/BingCN_Compressed.bin";
    }
    public class SnapshotTests
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
            var output = input.CompressToBrotli(11, 22);
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
            var compressed = input.CompressToBrotli(11, 22);
            var decompressed = compressed.DecompressFromBrotli();
            Assert.Equal(input, decompressed);
        }
    }
}
