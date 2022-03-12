namespace Fields;

public static class FieldLengths
{
    public static class PersonName
    {
        public const int First = 64;
        public const int Last = 64;
    }

    public static class Organization
    {
        public const int Name = 64;
        public const int ShortName = 16;
    }

    public static class Project
    {
        public const int Name = 128;
        public const int ShortName = 64;
        public const int CustomerName = 128;
        public const int CompanyContactInfoName = 128;
    }

    public static class ProductPhoto
    {
        public const int Name = 64;
    }

    public static class Category
    {
        public const int Name = 64;
    }

    public static class FileRef
    {
        public const int FileType = 32;
    }

    public static class Misc
    {
        public const int Color = 32;
    }

    public static class Address
    {
        public const int Line1 = 64;
        public const int Line2 = 64;
        public const int City = 64;
        public const int State = 16;
        public const int Zip = 16;
    }

    public static class Symbol
    {
        public const int Name = 64;
    }

    public static class ComponentVersion
    {
        public const int DisplayName = 128;
        public const int VersionName = 16;
        public const int Make = 64;
        public const int Model = 64;
        public const int VendorPartNumber = 64;
        public const int OrganizationPartNumber = 64;
        public const int WhereToBuy = 64;
        public const int Style = 64;
        public const int Color = 64;
    }

    public static class ProductKitVersion
    {
        public const int Name = 128;
        public const int VersionName = 16;
    }

    public static class ProductKitReference
    {
        public const int Tag = 4;
    }

    public static class LogoSet
    {
        public const int Name = 64;
    }

    public static class SheetType
    {
        public const int SheetNumberPrefix = 8;
        public const int SheetNamePrefix = 64;
    }

    public static class Page
    {
        public const int SheetNumberSuffix = 8;
        public const int SheetNameSuffix = 32;
    }

    public static class ProductFamily
    {
        public const int Name = 64;
    }

    public static class ComponentType
    {
        public const int Name = 128;
    }

    public static class ProductRequirement
    {
        public const int Label = 64;
    }
}
