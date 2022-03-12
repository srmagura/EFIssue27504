namespace FieldLengths
{
    public static class Fields
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
            public const int VersionName = 16;
        }
    }
}
