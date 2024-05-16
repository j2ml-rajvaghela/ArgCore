using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Arg.DataAccess
{
    public class MenusImpl
    {
        public List<Menus> GetMenus(string q)
        {
            var parameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(q))
            {
                parameters.Add("@q", q, DbType.String);
            }

            using var connection = Common.Database;
            var menus = connection.Query<Menus>("GetMenus", parameters, commandType: CommandType.StoredProcedure).ToList();
            return menus;
        }

        public Menus GetMenu(int menuId)
        {
            var parameters = new DynamicParameters();
            if (menuId > 0)
            {
                parameters.Add("@MenuId", menuId, DbType.Int32);
            }

            using var connection = Common.Database;
            var menu = connection.QueryFirstOrDefault<Menus>("GetMenu", parameters, commandType: CommandType.StoredProcedure);
            return menu;
        }

        public void SaveMenu(Menus menus)
        {
            if (string.IsNullOrWhiteSpace(menus.Name))
            {
                throw new Exception("Name can't be empty.");
            }
            if (string.IsNullOrWhiteSpace(menus.DisplayName))
            {
                throw new Exception("DisplayName can't be empty.");
            }

            using var connection = Common.Database;
            if (menus.MenuId == 0)
            {
                connection.Insert(menus);
            }
            else
            {
                connection.Update(menus);
            }
        }

        public int DeleteMenu(int menuId)
        {
            const string query = @"DELETE FROM Menus 
                                   WHERE MenuId=@MenuId;";

            using var connection = Common.Database;
            var result = connection.Execute(query, new { menuId });
            return Convert.ToInt32(result);
        }
    }
}
