using Arg.DataModels;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
using System.Transactions;

namespace Arg.DataAccess
{
    public class MenuItemsImpl
    {
        public List<MenuItems> GetParentMenuItems()
        {
            using (var connection = Common.Database)
            {
                var parentMenuItems = connection.Query<MenuItems>("GetParentMenuItems", commandType: CommandType.StoredProcedure).ToList();
                return parentMenuItems;
            }
        }

        public List<MenuItems> GetAssignedMenuItemsWithChildren(string roleId)

        {
            using (var connection = Common.Database)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RoleId", roleId, DbType.String);

                var assignedMenuItemsWithChildren = connection.Query<MenuItems>("GetAssignedMenuItemsWithChildren", parameters, commandType: CommandType.StoredProcedure).ToList();
                return assignedMenuItemsWithChildren;
            }
        }

        public List<MenuItems> GetRelatedMenuItems(string roleId)
        {
            using (var connection = Common.Database)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RoleId", roleId, DbType.String);

                var relatedMenuItems = connection.Query<MenuItems>("GetRelatedMenuItems", parameters, commandType: CommandType.StoredProcedure).ToList();
                return relatedMenuItems;
            }
        }

        public MenuItems GetMenuItem(int itemId, string link = "")
        {
            var parameters = new DynamicParameters();
            if (itemId > 0)
            {
                parameters.Add("@ItemId", itemId, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(link))
            {
                if (link.Substring(0, 1) == "/")
                {
                    link = link.Remove(0, 1).Trim();
                }
                parameters.Add("@Link", link, DbType.String);
            }
            using (var connection = Common.Database)
            {
                var menuItem = connection.QueryFirstOrDefault<MenuItems>("GetMenuItem", parameters, commandType: CommandType.StoredProcedure);
                return menuItem;
            }
        }


        public List<MenuItems> GetMenuItems(string q)
        {
            var parameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(q))
            {
                parameters.Add("@q", q, DbType.String);
            }
            using (var connection = Common.Database)
            {
                var menuItems = connection.Query<MenuItems>("GetMenuItems", parameters, commandType: CommandType.StoredProcedure).ToList();
                return menuItems;
            }
        }

        public void SaveMenuItem(MenuItems menuItem)
        {
            if (string.IsNullOrWhiteSpace(menuItem.Name))
            {
                throw new Exception("Name can't be empty.");
            }
            if (string.IsNullOrWhiteSpace(menuItem.DisplayName))
            {
                throw new Exception("DisplayName can't be empty.");
            }
            using (var connection = Common.Database)
            {
                if (menuItem.ItemId == 0)
                {
                    connection.Insert(menuItem);
                }
                else
                {
                    connection.Update(menuItem);
                }

            }
        }

        public int DeleteMenuItem(int itemId)
        {
            const string query = @"DELETE FROM MenuItem WHERE ItemId=@ItemId;";
            using (var connection = Common.Database)
            {
                var result = connection.Execute(query, new { @ItemId = itemId });
                return result;
            }
        }


        public bool CurrentUserHasAccessToMenuItem(string roleId, string menuLink)
        {
            if (menuLink.IndexOf("MenuItems") > 0)
                menuLink = menuLink.Replace("MenuItems", "Menus");
            if (menuLink.IndexOf("SettingGroups") > 0)
                menuLink = menuLink.Replace("SettingGroups", "Menus");
            if (menuLink.IndexOf("TemplateCats") > 0)
                menuLink = menuLink.Replace("TemplateCats", "Templates");
            if (menuLink.IndexOf("Import") > 0)
                menuLink = menuLink.Replace("Import", "Clients");
            var myString = menuLink.Split('/').Last();
            if (myString == "Save")
                menuLink = menuLink.Replace("Save", "Index");
            if (myString == "Delete")
                menuLink = menuLink.Replace("Delete", "Index");
            if (myString == "ManageMappings")
                menuLink = menuLink.Replace("ManageMappings", "Index");
            if (myString == "ManageTableSettings")
                menuLink = menuLink.Replace("ManageTableSettings", "Index");
            if (myString == "ViewAuditingResultStats")
                menuLink = menuLink.Replace("ViewAuditingResultStats", "Index");
            if (myString == "AuditingResults")
                menuLink = menuLink.Replace("AuditingResults", "Index");
            if (myString == "AuditorPlaybook")
                menuLink = menuLink.Replace("AuditorPlaybook", "Index");
            if (myString == "AddBalanceDue")
                menuLink = menuLink.Replace("AddBalanceDue", "Index");
            if (myString == "Assign")
                menuLink = menuLink.Replace("Assign", "Index");
            if (myString == "CollectionStatusDetails")
                menuLink = menuLink.Replace("CollectionStatusDetails", "Index");
            if (myString == "InvoiceStatusDetails")
                menuLink = menuLink.Replace("InvoiceStatusDetails", "Index");
            if (myString == "ClientGLStatusDetails")
                menuLink = menuLink.Replace("ClientGLStatusDetails", "Index");
            if (myString == "BDDetails")
                menuLink = menuLink.Replace("BDDetails", "Index");
            if (myString == "BDPaymentAmountDetails")
                menuLink = menuLink.Replace("BDPaymentAmountDetails", "Index");
            if (myString == "AddCollectionComment")
                menuLink = menuLink.Replace("AddCollectionComment", "Index");
            if (myString == "UpdateBalanceDue")
                menuLink = menuLink.Replace("UpdateBalanceDue", "Index");
            if (myString == "GenerateInvoicePDF")
                menuLink = menuLink.Replace("GenerateInvoicePDF", "Index");
            if (myString == "GenerateInvoice")
                menuLink = menuLink.Replace("GenerateInvoice", "Index");
            if (myString == "PrintManagementReport")
                menuLink = menuLink.Replace("PrintManagementReport", "Index");
            if (myString == "LoadClientData")
                menuLink = menuLink.Replace("LoadClientData", "Index");
            //if (myString == "ManageMappings")
            //    MenuLink = MenuLink.Replace("ManageMappings", "Index");
            //if (myString == "ManagementReports")
            //    MenuLink = MenuLink.Replace("ManagementReports", "Index");
            if (myString == "SaveBasedOnCustomerId")
                menuLink = menuLink.Replace("SaveBasedOnCustomerId", "Index");
            if (myString == "ShowGroups")
                menuLink = menuLink.Replace("ShowGroups", "Index");
            if (myString == "InvoicePDF")
                menuLink = menuLink.Replace("InvoicePDF", "Index");
            if (myString == "InvoicePaymentAmtDetails")
                menuLink = menuLink.Replace("InvoicePaymentAmtDetails", "Index");
            if (myString == "CommissionDetails")
                menuLink = menuLink.Replace("CommissionDetails", "Index");
            if (myString == "PullForResearch")
                menuLink = menuLink.Replace("PullForResearch", "Index");
            if (myString == "StatusDetails")
                menuLink = menuLink.Replace("StatusDetails", "Index");
            var menuItem = GetMenuItem(0, menuLink);
            var parameters = new DynamicParameters();
            parameters.Add("@RoleId", roleId, DbType.String);
            parameters.Add("@ItemId", menuItem.ItemId, DbType.Int32);
            parameters.Add("@ParentId", menuItem.ParentId, DbType.Int32);
            const string query = "SELECT ItemId FROM RoleMenuRels WHERE (RoleId=@RoleId AND ItemId=@ItemId) OR (RoleId=@RoleId AND ItemId=@ParentId)";
            using (var connection = Common.Database)
            {
                if (menuItem != null && menuItem.ItemId > 0)
                {
                    var result = Convert.ToInt32(connection.ExecuteScalar(query, parameters));
                    return result > 0;
                }
                return false;
            }
        }
    }
}
