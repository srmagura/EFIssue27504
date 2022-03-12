using Entities;
using Enumerations;
using Identities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Settings;
using ValueObjects;

namespace UnitTests;

[TestClass]
public class ReportNameSanitizeTests
{
    [TestMethod]
    public void InvalidCharactersAreRemoved()
    {
        var projectShortname = "Test@#%!$^&*()=+[]{}\\|;:\"<,>/?`~";
        var project = new Project(
            new OrganizationId(),
            "Name",
            projectShortname,
            "Description",
            new PartialAddress(),
            "CustomerName",
            "SigneeName",
            new LogoSetId(),
            new TermsDocumentId(),
            "PreparerName",
            1,
            new ProjectSettings()
        );

        var report = new Report(project, null, ReportType.DrawingSet);
        report.SetCompleted(new FileRef(new FileId(), "application/pdf"), projectShortname, 1);

        Assert.AreEqual("Drawing Set - Test v1.pdf", report.Filename);
    }
}
