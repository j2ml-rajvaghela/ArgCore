using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class TemplatesImpl
    {
        public IEnumerable<Templates> GetTemplatesList()
        {
            using (var connection = Common.Database)
            {
                var template = connection.Query<Templates>("GetAllTemplates", commandType: CommandType.StoredProcedure).ToList();
                return template;
            }
        }

        public Templates GetTemplate(int templateId, string name)
        {
            var parameters = new DynamicParameters();
            if (templateId > 0)
            {
                parameters.Add("@TemplateId", templateId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                parameters.Add("@Name", name, DbType.String);
            }
            using (var connection = Common.Database)
            {
                var template = connection.QueryFirstOrDefault<Templates>("GetTemplate", parameters, commandType: CommandType.StoredProcedure);
                return template;
            }
        }

        public IEnumerable<Templates> GetTemplates(string q)
        {
            var parameters = new DynamicParameters();
            if (q != null)
            {
                parameters.Add("@q", q, DbType.String);
            }
            using (var connection = Common.Database)
            {
                var templates = connection.Query<Templates>("GetTemplates", parameters, commandType: CommandType.StoredProcedure).ToList();
                return templates;
            }
        }

        public List<Templates> GetTemplatesExist(string name, int catId, int templateId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Name", name, DbType.String);
            parameters.Add("@CatId", catId, DbType.Int32);
            if (templateId > 0)
            {
                parameters.Add("@TemplateId", templateId, DbType.Int32);
            }
            using (var connection = Common.Database)
            {
                var isTemplatesExist = connection.Query<Templates>("GetTemplatesExist", parameters, commandType: CommandType.StoredProcedure).ToList();
                return isTemplatesExist;
            }
        }

        public void SaveTemplate(Templates template)
        {
            if (string.IsNullOrWhiteSpace(template.Name))
            {
                throw new Exception("Name can't be empty.");
            }
            template.AddedOn = DateTime.Now;

            using (var connection = Common.Database)
            {
                if (template.TemplateId == 0)
                {
                   
                    connection.Insert(template);
                }
                else
                {
                    connection.Update(template);
                }
            }
              

        }

          public int DeleteTemplate(int templateId)
          {
              const string query = @"DELETE FROM Templates WHERE TemplateId=@TemplateId";
              using (var connection = Common.Database)
              {
                var result = connection.Execute(query, new { @TemplateId = templateId});
                return result;
              }
          }
    }
}
