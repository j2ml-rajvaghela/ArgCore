using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class TemplateCatsImpl
    {
        public IEnumerable<TemplateCats> GetTemplateCats(int catId, string q)
        {
            var parameters = new DynamicParameters();
            if (catId > 0)
            {
                parameters.Add("@CatId", catId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(q))
            {
                parameters.Add("@q", q, DbType.String);
            }
            using (var connection = Common.Database)
            {
                var templateCates = connection.Query<TemplateCats>("GetTemplateCats", parameters, commandType: CommandType.StoredProcedure).ToList();
                return templateCates;
            }
               
        }

        public TemplateCats GetTemplateCat(int catId, string name)
        {
            var parameters = new DynamicParameters();
            if (catId > 0)
            {
                parameters.Add("@CatId", catId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                parameters.Add("@Name", name, DbType.String);
            }
            using (var connection = Common.Database)
            {
                var templateCate = connection.QueryFirstOrDefault<TemplateCats>("GetTemplateCat", parameters, commandType: CommandType.StoredProcedure);
                return templateCate;
            }
        }

        public List<TemplateCats> TemplateCatsExist(string name, int catId)
        {
            var parameters = new DynamicParameters();
            if (catId > 0)
            {
                parameters.Add("@CatId", catId, DbType.Int32);
            }
            parameters.Add("@Name", name, DbType.String);
            using (var connection = Common.Database)
            { 

                var isTemplateCatesExist = connection.Query<TemplateCats>("TemplateCatsExits", parameters, commandType: CommandType.StoredProcedure).ToList();
                return isTemplateCatesExist;
            }
        }

        public void SaveTemplateCat(TemplateCats templateCat)
        {
            if (string.IsNullOrWhiteSpace(templateCat.Name))
            {
                throw new Exception("Name can't be empty.");
            }
            templateCat.AddedOn = DateTime.Now;
            using (var connection = Common.Database)
            {
                if (templateCat.CatId == 0)
                {
                    connection.Insert(templateCat);
                }
                else
                {
                    connection.Update(templateCat);
                }
            }
        }

        public int DeleteTemplateCat(int catId)
        {
            const string query = @"DELETE FROM TemplateCats WHERE CatId=@CatId";
            using (var connection = Common.Database)
            {
                var result = connection.Execute(query, new { @CatId = catId });
                return result;
            }
        }
    }
}
