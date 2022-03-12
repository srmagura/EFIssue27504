using ValueObjects;

namespace Graphics;

public interface IBellCurvePlotGenerator
{
    Task<string> GenerateImage(
        TypicalBudgetRange typicalBudgetRange,
        Money projectPrice,
        Stream outputStream
    ); // returns image file type
}
