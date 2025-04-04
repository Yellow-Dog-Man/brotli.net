namespace Brotli.NET.Tests.Snapshot;

public class BrotliSnapshotting
{
    private static string ROOT_PATH => CurrentFile.Directory();
    private static string SNAPSHOT_PATH => Path.Combine(ROOT_PATH, "..", "Resource/brotli");

    [Theory]
    [MemberData(nameof(GetData))]
    public void ValidateBrotli(string uncompressedFilePath, string compressedFilePath)
    {

        var inputBytes = File.ReadAllBytes(Path.Combine(SNAPSHOT_PATH, uncompressedFilePath));
        var expectedCompressedBytes = File.ReadAllBytes(Path.Combine(SNAPSHOT_PATH, compressedFilePath));

        var decompressedBytes = expectedCompressedBytes.DecompressFromBrotli();
        var compressedBytes = inputBytes.CompressToBrotli();
        
        Assert.Equal(inputBytes, decompressedBytes);

        //TODO: this fails, it must be because the window size or quality are wrong.
        Assert.Equal(expectedCompressedBytes, compressedBytes);
    }

    public static IEnumerable<object[]> GetData()
    {
        var files = Directory.EnumerateFiles(SNAPSHOT_PATH);
        var uncompressed = files.Where(file => !IsCompressed(file));
        var compressed = files.Where(file => IsCompressed(file));

        foreach (string uncompressFilePath in uncompressed)
        {
            var compressedFilePath = compressed.FirstOrDefault(file => file.StartsWith(uncompressFilePath));
            if (!string.IsNullOrEmpty(compressedFilePath))
            {
                yield return new object[] { Path.GetFileName(uncompressFilePath), Path.GetFileName(compressedFilePath), 22u, 5u };
                yield return new object[] { Path.GetFileName(uncompressFilePath), Path.GetFileName(compressedFilePath), 22u, 11u };
                yield return new object[] { Path.GetFileName(uncompressFilePath), Path.GetFileName(compressedFilePath), 22u, 0u };
                yield return new object[] { Path.GetFileName(uncompressFilePath), Path.GetFileName(compressedFilePath), 22u, 10u };
                yield return new object[] { Path.GetFileName(uncompressFilePath), Path.GetFileName(compressedFilePath), 22u, 6u };
            }
        }
    }

    private static bool IsCompressed(string file)
    {
        return file.EndsWith("compressed");
    }
}
