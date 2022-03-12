namespace Enumerations;

public enum ImportStatus
{
    Pending = 0,
    Processing = 10,

    Completed = 100,
    Canceled = 900,
    Error = 910,
}
