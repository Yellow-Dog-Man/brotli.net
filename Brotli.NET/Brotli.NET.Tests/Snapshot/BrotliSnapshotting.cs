namespace Brotli.NET.Tests.Snapshot;

public class BrotliSnapshotting
{
    [Fact]
    public Task VerifyVerify() => VerifyChecks.Run();

    private static string ROOT_PATH => CurrentFile.Directory();
    private static string SNAPSHOT_PATH => Path.Combine(ROOT_PATH, "..", "Resource/brotli");

    // These are both the "MAX" values and based on the native library.
    public const int SNAPSHOT_WINDOW = 24;
    public const int SNAPSHOT_QUALITY = 11;

    [Theory]
    [MemberData(nameof(GetCompressData))]
    public Task TestCompress(string uncompressedFilePath, uint quality, uint window)
    {
        // Files compressed with identical parameters, should result in identical outputs, verify is perfect for this.
        var inputBytes = File.ReadAllBytes(Path.Combine(SNAPSHOT_PATH, uncompressedFilePath));
        var compressedBytes = inputBytes.CompressToBrotli(quality, window);
        
        return Verify(compressedBytes).UseParameters(uncompressedFilePath, quality, window); 
    }

    [Theory]
    [MemberData(nameof(GetRoundtripData))]
    public void TestRoundtrip(string uncompressedFilePath)
    {
        var inputBytes = File.ReadAllBytes(Path.Combine(SNAPSHOT_PATH, uncompressedFilePath));

        // Compress
        var compressedBytes = inputBytes.CompressToBrotli();

        // Immediately de-compress
        var decompressedBytes = compressedBytes.DecompressFromBrotli();

        // test IN == Roundtripped
        Assert.Equal(inputBytes, decompressedBytes);
    }

    public static IEnumerable<object[]> GetCompressData()
    {
        return Directory.EnumerateFiles(SNAPSHOT_PATH).Select(file => new object[] { Path.GetFileName(file), SNAPSHOT_QUALITY, SNAPSHOT_WINDOW });
    }

    public static IEnumerable<object[]> GetRoundtripData()
    {
        return Directory.EnumerateFiles(SNAPSHOT_PATH).Select(file => new object[] { Path.GetFileName(file)});
    }
}
