namespace Identities;

public record ReportId : Identity
{
    public ReportId() { }
    public ReportId(Guid guid) : base(guid) { }
    public ReportId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
