using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.Services.Models;
using Dapper;
using System.Data;

namespace BookStoreApi.Database.Repositories
{
    public class ImageRepository(DapperUtility dapperUtility) : IImageRepository
    {
        public async Task<bool> ChangePrimary(int id)
        {
            string sql = "Image_Change_Primary";
            using var connection = dapperUtility.GetConnection();
            int result = await connection.ExecuteScalarAsync<int>(sql, new { id }, commandType: CommandType.StoredProcedure);
            return result == 1;
        }

        public async Task<List<int>?> CreateAsync(List<ImageInfo> imageInfos, string foreignTable, int foreignId)
        {
            // making images table
            var imagesTable = DataTables.ImageInfoTypeTable(imageInfos);

            try
            {
                var connection = dapperUtility.GetConnection();
                var result = await connection.QueryAsync<int>(
                    "Image_Insert",
                      new
                      {
                          Images = imagesTable.AsTableValuedParameter("ImageInfoType"),
                          ForeignTable = foreignTable,
                          ForeignId = foreignId
                      },
                         commandType: CommandType.StoredProcedure
                    );

                return result.ToList();
            }
            catch (Exception)
            {

                return null;
            }

        }

        public async Task<bool> DeleteAsync(int id)
        {
            string sql = "DELETE FROM Images WHERE Id = @id";
            using var connection = dapperUtility.GetConnection();
            int result = await connection.ExecuteAsync(sql, new { id });
            if (result == 1) return true;
            return false;
        }

        public async Task<bool> ForeignIdExists(int id, string table)
        {
            string sql = $"SELECT COUNT(1) FROM {table} WHERE Id = @Id";

            using var connection = dapperUtility.GetConnection();
            int count = await connection.ExecuteScalarAsync<int>(sql, new { Id = id });

            return count > 0;
        }

        public async Task<Image?> GetByIdAsync(int id)
        {
            string sql = "Image_Get_One";
            using var connection = dapperUtility.GetConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Image>(sql, new { id }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<List<Image>?> GetByIdAsync(List<int> ids)
        {
            string sql = "Image_Get_By_Ids";

            var idsTable = new DataTable();
            idsTable.Columns.Add("Id", typeof(int));
            foreach (var id in ids)
            {
                idsTable.Rows.Add(id);
            }

            using var connection = dapperUtility.GetConnection();
            var result = await connection.QueryAsync<Image>(sql,
                new { Ids = idsTable.AsTableValuedParameter("IntList") },
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
    }
}
