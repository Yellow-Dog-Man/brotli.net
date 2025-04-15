namespace Brotli.NET.Tests.Snapshot
{
    public static class Fixtures
    {
        public const string UncompressedPath = "Resource/BingCN.bin";
        public const string CompressedPath = "Resource/BingCN_Compressed.bin";

        public const string LFUncompressed = "Resource/LineEndings/LF.bin";
        public const string CRLFUncompressed = "Resource/LineEndings/CRLF.bin";

        // Copied from original tests but now preserved as constant: https://github.com/Yellow-Dog-Man/brotli.net/commit/0f4bc44c8466cc19777612639ca225de7cbcd644
        public const uint FIXTURE_QUALITY = 11;
        public const uint FIXTURE_WINDOW = 22;
        public const uint FIXTURE_BLOCK_SIZE = 24;
    }

    public class StaticFileTests
    {
        [Fact]
        public Task VerifyVerify() => VerifyChecks.Run();

        public static byte[] GetBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        [Fact]
        public Task TestCompress()
        {
            var input = GetBytes(Fixtures.UncompressedPath);
            var output = input.CompressToBrotli(Fixtures.FIXTURE_QUALITY, Fixtures.FIXTURE_WINDOW, Fixtures.FIXTURE_BLOCK_SIZE);
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
            var compressed = input.CompressToBrotli(Fixtures.FIXTURE_QUALITY, Fixtures.FIXTURE_WINDOW, Fixtures.FIXTURE_BLOCK_SIZE);
            var decompressed = compressed.DecompressFromBrotli();
            Assert.Equal(input, decompressed);
        }

        [Theory]
        [InlineData(Fixtures.LFUncompressed)]
        [InlineData(Fixtures.CRLFUncompressed)]
        public Task TestLineEndingsCompress(string file)
        {
            var input = GetBytes(file);
            var output = input.CompressToBrotli(Fixtures.FIXTURE_QUALITY, Fixtures.FIXTURE_WINDOW, Fixtures.FIXTURE_BLOCK_SIZE);
            return Verify(output).UseParameters(file);
        }

        [Theory]
        [InlineData(Fixtures.LFUncompressed)]
        [InlineData(Fixtures.CRLFUncompressed)]
        public Task TestLineEndingsUncompress(string file)
        {
            var input = GetBytes(file);
            var compress = input.CompressToBrotli(Fixtures.FIXTURE_QUALITY, Fixtures.FIXTURE_WINDOW, Fixtures.FIXTURE_BLOCK_SIZE);

            var uncompress = compress.DecompressFromBrotli();
            return Verify(uncompress).UseParameters(file);
        }
    }
}
