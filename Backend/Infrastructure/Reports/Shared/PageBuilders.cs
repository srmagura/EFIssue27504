using System.Drawing.Imaging;
using System.Text;
using AppDTOs;
using AppDTOs.Report;
using Aspose.Pdf;
using Aspose.Pdf.Facades;
using Identities;
using InfraInterfaces;
using Reports.Floorplan;
using Reports.ProductLegend;
using Reports.Signature;
using Reports.Title;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Svg;

namespace Reports.Shared;

public static class PageBuilders
{
    private static void WriteSvgToDisk(string path, string svgText)
    {
        var byteArray = Encoding.UTF8.GetBytes(svgText);
        using var svgstream = new MemoryStream(byteArray);
        var svgDocument = SvgDocument.Open<SvgDocument>(svgstream);

        using var bitmap = svgDocument.Draw();
        bitmap.Save(path, ImageFormat.Png);
    }

    public static async Task SetupTempFilesAsync(
        IFileStore fileStore,
        IFilePathBuilder path,
        string directoryPath,
        ProjectDto project,
        ProductKitReportDto[] productKits,
        DesignerDataReportDto[] designerData,
        TermsDocumentDto? termsDocument = null
    )
    {
        if (project.Photo != null)
        {
            var filePath = Path.Combine(directoryPath, project.Id.ToString());
            using var fileStream = File.Create(filePath);
            await fileStore.GetAsync(
                path.ForProjectPhoto(project.Photo.FileId),
                fileStream
            );
        }

        WriteSvgToDisk(
            Path.Combine(directoryPath, ReportConstants.ImageUnavailableFileName),
            ReportConstants.ImageUnavailableSvgText
        );

        WriteSvgToDisk(
            Path.Combine(directoryPath, ReportConstants.NoteIconFileName),
            ReportConstants.NoteIconSvgText
        );

        foreach (var productKit in productKits)
        {
            if (productKit.ProductPhotoFileId != null)
            {
                var filePath = Path.Combine(directoryPath, productKit.Id.ToString());
                using var fileStream = File.Create(filePath);
                await fileStore.GetAsync(
                    path.ForProductPhoto(productKit.ProductPhotoFileId),
                    fileStream
                );
            }

            var symbolFilePath = Path.Combine(
                directoryPath,
                $"{productKit.SymbolId}.png" // Need png for ImageSharp to work
            );
            if (!File.Exists(symbolFilePath))
            {
                try
                {
                    WriteSvgToDisk(symbolFilePath, productKit.SymbolSvgText);
                }
                catch
                {
                    WriteSvgToDisk(symbolFilePath, ReportConstants.DefaultSymbolSvg);
                }

                using var image = SixLabors.ImageSharp.Image.Load(symbolFilePath);
                image.Mutate(s => s.Invert());

                image.Save(symbolFilePath);
            }
        }

        foreach (var data in designerData)
        {
            var filePath = Path.Combine(directoryPath, data.PageId.ToString());
            using var fileStream = File.Create(filePath);
            await fileStore.GetAsync(path.ForPage(data.PdfFileId), fileStream);
        }

        if (termsDocument != null)
        {
            var filePath = Path.Combine(directoryPath, termsDocument.Id.ToString());
            using var fileStream = File.Create(filePath);
            await fileStore.GetAsync(path.ForTermsDocument(termsDocument.File.FileId), fileStream);
        }
    }

    private static Document GeneratePdfFromHtml(
        IHtmlBuilder page,
        List<IDisposable> disposables,
        string? htmlPath = null
    )
    {
        HtmlLoadOptions options = new HtmlLoadOptions();
        options.PageInfo.Margin = new MarginInfo(0, 0, 0, 0);
        options.PageInfo.Width = ReportConstants.PageWidth;
        options.PageInfo.Height = ReportConstants.PageHeight;

        using var htmlStream = new MemoryStream();
        using var writer = new StreamWriter(htmlStream);

        var pageHtml = page.Render();

        writer.Write(pageHtml);
        writer.Flush();
        htmlStream.Position = 0;

        if (htmlPath != null) File.WriteAllText(htmlPath, pageHtml);

        var doc = new Document(htmlStream, options);
        disposables.Add(doc);

        return doc;
    }

    private static void StampFloorplan(Document doc, string stampPath)
    {
        PdfFileStamp fileStamp = new PdfFileStamp();
        fileStamp.BindPdf(doc);

        Aspose.Pdf.Facades.Stamp stamp = new Aspose.Pdf.Facades.Stamp();
        stamp.BindPdf(stampPath, 1);
        stamp.IsBackground = true;
        stamp.Pages = new[] { 1 };

        fileStamp.AddStamp(stamp);
    }

    public static List<(string? Number, string? Name)> BuildSheetIndex(
        int productLegendPageCount,
        List<PageSummaryDto> pageEntities,
        SheetTypeSummaryDto[] sheetTypes
    )
    {
        var sheetIndex = new List<(string?, string?)>
            {
                ("T1.000", "PROJECT OVERVIEW")
            };

        for (var i = 1; i <= productLegendPageCount; i++)
        {
            string s = string.Format("{0:000}", i);
            sheetIndex.Add((
                $"T1.{s}",
                $"DEVICE DETAILS & TABLES ({i} OF {productLegendPageCount})"
            ));
        }

        pageEntities.ForEach(f =>
        {
            var sheetType = sheetTypes.FirstOrDefault(t => t.Id == f.SheetTypeId);

            string? sheetName = null;
            string? sheetNumber = null;
            if (sheetType != null)
            {
                sheetNumber = sheetType.SheetNumberPrefix;
                sheetName = sheetType.SheetNamePrefix;

                if (f.SheetNumberSuffix != null) sheetNumber += f.SheetNumberSuffix;
                if (f.SheetNameSuffix != null) sheetName += $" — {f.SheetNameSuffix}"; // This is an em dash
            }

            sheetIndex.Add((sheetNumber, sheetName));
        });

        return sheetIndex;
    }

    public static Document GetTitlePage(
        ProjectDto project,
        List<(string? Number, string? Name)> sheetIndex,
        string directoryPath,
        List<IDisposable> disposables,
        bool isDraft,
        bool devGenerateHtml
    )
    {
        var devGenerateHtmlFileName = $"titlePage.html";
        return GeneratePdfFromHtml(
            new TitlePage(
                isDraft,
                project,
                sheetIndex,
                directoryPath
            ),
            disposables,
            devGenerateHtml ? devGenerateHtmlFileName : null
        );
    }

    public static List<Document> GetProductLegendPages(
        ProductKitReportDto[] productKits,
        string directoryPath,
        List<IDisposable> disposables,
        bool isDraft,
        bool devGenerateHtml
    )
    {
        var categoryRowNames = productKits.Select(p => p.CategoryName)
            .Distinct()
            .ToList();

        List<ProductKitReportDto[]> productKitRows = new List<ProductKitReportDto[]>();
        foreach (var categoryName in categoryRowNames)
        {
            var pk = productKits.Where(p => p.CategoryName == categoryName).ToList();
            for (var i = 0; i < pk.Count; i += ReportConstants.ProductLegendItemCount)
            {
                var row = pk.Skip(i)
                    .Take(ReportConstants.ProductLegendItemCount)
                    .ToArray();

                productKitRows.Add(row);
            }
        }

        List<Document> productLegendDocs = new List<Document>();
        for (var i = 0; i < productKitRows.Count; i += ReportConstants.ProductLegendTotalRowCount)
        {
            var pageProductKitRows = productKitRows.Skip(i)
                .Take(ReportConstants.ProductLegendTotalRowCount)
                .ToList();

            var devGenerateHtmlFileName = $"productLegend{i}.html";
            productLegendDocs.Add(GeneratePdfFromHtml(
                new ProductLegendPage(
                    isDraft,
                    pageProductKitRows,
                    directoryPath
                ),
                disposables,
                devGenerateHtml ? devGenerateHtmlFileName : null
            ));
        }

        return productLegendDocs;
    }

    public static List<Document> GetFloorplanPages(
        DesignerDataReportDto[] designerData,
        ProductKitReportDto[] productKits,
        string directoryPath,
        List<IDisposable> disposables,
        bool isDraft,
        bool devGenerateHtml
    )
    {
        List<Document> floorplanDocs = new List<Document>();
        for (var i = 0; i < designerData.Length; i++)
        {
            var pageDesignerData = designerData[i];

            var devGenerateHtmlFileName = $"floorplan{i}.html";
            var floorplanDoc = GeneratePdfFromHtml(
                new FloorplanPage(
                    isDraft,
                    pageDesignerData,
                    productKits,
                    directoryPath
                ),
                disposables,
                devGenerateHtml ? devGenerateHtmlFileName : null
            );

            StampFloorplan(
                floorplanDoc,
                Path.Combine(directoryPath, pageDesignerData.PageId.ToString())
            );

            floorplanDocs.Add(floorplanDoc);
        }

        return floorplanDocs;
    }

    public static List<Document> GetTermsDocuments(
        TermsDocumentId termsDocumentId,
        string directoryPath,
        List<IDisposable> disposables,
        bool isDraft,
        bool devGenerateHtml
    )
    {
        var termsDocPath = Path.Combine(directoryPath, termsDocumentId.ToString());
        using Document tempTermsDoc = new Document(termsDocPath); // Just need to get page count for later use
        var pageCount = tempTermsDoc.Pages.Count;
        var newPageCount = Math.Ceiling(pageCount / 2d);

        List<Document> termsDocs = new List<Document>();
        for (var i = 0; i < newPageCount; i++)
        {
            var devGenerateHtmlFileName = $"terms{i}.html";
            var termsDoc = GeneratePdfFromHtml(
                new BasePageWithTitleBlock(
                    new List<IHtmlBuilder>(),
                    isDraft
                ),
                disposables,
                devGenerateHtml ? devGenerateHtmlFileName : null
            );
            termsDocs.Add(termsDoc);

            // Terms Doc Left Side

            var leftSideIndex = (i * 2) + 1;

            PdfFileStamp fileStamp = new PdfFileStamp();
            fileStamp.BindPdf(termsDoc);

            Aspose.Pdf.Facades.Stamp leftSideStamp = new Aspose.Pdf.Facades.Stamp();
            leftSideStamp.BindPdf(termsDocPath, leftSideIndex);
            leftSideStamp.IsBackground = true;
            leftSideStamp.Pages = new[] { 1 };

            fileStamp.AddStamp(leftSideStamp);

            // Terms Doc Right Side

            if (leftSideIndex < pageCount)
            {
                var rightSideIndex = (i * 2) + 2;

                Aspose.Pdf.Facades.Stamp rightSideStamp = new Aspose.Pdf.Facades.Stamp();
                rightSideStamp.BindPdf(termsDocPath, rightSideIndex);
                leftSideStamp.IsBackground = true;
                rightSideStamp.Pages = new[] { 1 };
                rightSideStamp.SetOrigin((float)(ReportConstants.ContentWidth / 2), 0);

                fileStamp.AddStamp(rightSideStamp);
            }
        }

        return termsDocs;
    }

    public static Document GetSignaturePage(
        ProjectDto project,
        double budget,
        List<IDisposable> disposables,
        bool isDraft,
        bool devGenerateHtml
    )
    {
        var devGenerateHtmlFileName = $"signaturePage.html";
        return GeneratePdfFromHtml(
            new SignaturePage(
                isDraft,
                project,
                budget
            ),
            disposables,
            devGenerateHtml ? devGenerateHtmlFileName : null
        );
    }
}
