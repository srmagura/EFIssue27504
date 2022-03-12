using System.IO.Compression;
using System.Text;

namespace DbEntities;

public class DbDesignerData
{
    public DbDesignerData(Guid organizationId, Guid pageId, DesignerDataType type, string json)
    {
        OrganizationId = organizationId;
        PageId = pageId;
        Type = type;
        Json = json;
    }

    public long Id { get; set; }

    public Guid OrganizationId { get; set; }
    public DbOrganization? Organization { get; set; }

    public Guid PageId { get; set; }
    public DbPage? Page { get; set; }

    public DesignerDataType Type { get; set; }
    public string Json { get; set; }

    public int JsonSchemaVersion { get; set; } = 0; // Put the current version here

    public static void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<DbDesignerData>()
            .Property(d => d.Json)
            .HasConversion(s => Compress(s), b => Decompress(b));

        mb.Entity<DbDesignerData>()
            .HasIndex(d => new { d.PageId, d.Type })
            .IsUnique();
    }

    internal static byte[] Compress(string text)
    {
        using var outputStream = new MemoryStream();
        using var brotliStream = new BrotliStream(outputStream, CompressionLevel.Fastest);

        brotliStream.Write(Encoding.UTF8.GetBytes(text));
        brotliStream.Flush();

        return outputStream.ToArray();
    }

    internal static string Decompress(byte[] bytes)
    {
        using var inputStream = new MemoryStream(bytes);
        using var brotliStream = new BrotliStream(inputStream, CompressionMode.Decompress);

        using var streamReader = new StreamReader(brotliStream, Encoding.UTF8);
        return streamReader.ReadToEnd();
    }
}
