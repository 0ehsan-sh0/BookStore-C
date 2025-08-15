using BookStoreApi.Services.Models;
using System.Data;

namespace BookStoreApi.Database.Models
{
    public static class DataTables
    {
        public static DataTable IntListTable(List<int> numbers)
        {
            DataTable dt = new();
            dt.Columns.Add("Id", typeof(int));
            foreach (var number in numbers)
            {
                dt.Rows.Add(number);
            }
            return dt;
        }

        public static DataTable ImageInfoTypeTable(List<ImageInfo> imageInfos)
        {
            // Columns
            DataTable imagesTable = new();
            imagesTable.Columns.Add("Width", typeof(int));
            imagesTable.Columns.Add("Height", typeof(int));
            imagesTable.Columns.Add("IsPrimary", typeof(bool));
            imagesTable.Columns.Add("StoredFileName", typeof(string));
            imagesTable.Columns.Add("RelativePath", typeof(string));
            imagesTable.Columns.Add("FileSize", typeof(long));
            imagesTable.Columns.Add("MimeType", typeof(string));

            // Rows
            for (int i = 0; i < imageInfos.Count; i++)
            {
                var image = imageInfos[i];
                bool isPrimary = (i == 0);  // First image is primary

                imagesTable.Rows.Add(
                    image.Width,
                    image.Height,
                    isPrimary,
                    image.StoredFileName,
                    image.RelativePath,
                    image.FileSize,
                    image.MimeType
                );
            }

            return imagesTable;
        }
    }
}
