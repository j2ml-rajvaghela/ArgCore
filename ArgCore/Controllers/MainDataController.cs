using Arg.DAL;
using Arg.DataModels;
using ArgCore.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Reporting.WebForms;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Web.Http;

namespace ArgCore.Controllers
{
    [RoutePrefix("api")]
    [Authorize]
    public class MainDataController : _baseAPIController
    {
        #region CLIENTS

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [Route("manageclients")]
        public List<Client> list_adminclients()
        {
            List<Client> LST = new List<Client>();
            try
            {
                using (clients db = new clients())
                {
                    LST = db.list_adminclients(ThisUserID());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return LST;
        }

        [HttpGet]
        [Route("clients")]
        public List<Client> list_client()
        {
            List<Client> LST = new List<Client>();
            try
            {
                using (clients db = new clients())
                {
                    LST = db.list_clients(ThisUserID());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return LST;
        }

        // return single Client record based on permissions
        [HttpGet]
        [Route("client/{clientid}")]
        public Client get_client(int clientid)
        {
            Client REC = new Client();
            try
            {
                using (clients db = new clients())
                {
                    REC = db.get_client(ThisUserID(), clientid);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return REC;
        }

        [HttpGet]
        [Route("ddlclients")]
        public List<selectlistitem> ddl_clients()
        {
            List<selectlistitem> LST = new List<selectlistitem>();
            try
            {
                using (clients db = new clients())
                {
                    LST = db.ddl_clients(ThisUserID());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return LST;
        }

        [HttpPost]
        [Route("client")]
        public dbresult customer_update(Client REC)
        {
            dbresult RSLT = new dbresult();
            try
            {
                using (clients db = new clients())
                {
                    RSLT = db.client_update(ThisUserID(), REC);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return RSLT;
        }
        #endregion

        #region  GROUPS

        [HttpGet]
        [Route("ddlgroups")]
        public List<selectlistitem> ddl_groups()
        {
            List<selectlistitem> LST = new List<selectlistitem>();
            try
            {
                using (clients db = new clients())
                {
                    LST = db.ddl_groups(ThisUserID());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return LST;
        }

        // group list
        [HttpGet]
        [Route("groups")]
        public List<group> list_groups()
        {
            List<group> LST = new List<group>();
            try
            {
                using (groups db = new groups())
                {
                    LST = db.listgroups(ThisUserID());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return LST;
        }

        // single record
        [HttpGet]
        [Route("group/{id}")]
        public group get_groups(int id)
        {
            group REC = new group();
            try
            {
                using (groups db = new groups())
                {
                    REC = db.get_group(ThisUserID(), id);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return REC;
        }

        // single record
        [HttpPost]
        [Route("group")]
        public dbresult update_group(group REC)
        {
            dbresult RSLT = new dbresult();
            try
            {
                using (groups db = new groups())
                {
                    RSLT = db.update_group(ThisUserID(), REC);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return RSLT;
        }

        // group member list
        [HttpGet]
        [Route("groupmembers/{groupid}")]
        public List<groupmember> list_groupmembers(int groupid)
        {
            List<groupmember> LST = new List<groupmember>();
            try
            {
                using (groups db = new groups())
                {
                    LST = db.list_groupmembersall(ThisUserID(), groupid);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return LST;
        }

        // entire data set
        [HttpPost]
        [Route("groupmembers/{groupid}")]
        public dbresult update_groupmembers(int groupid, List<groupmember> LST)
        {
            dbresult RSLT = new dbresult();
            try
            {
                using (groups db = new groups())
                {
                    RSLT = db.update_groupmembers(ThisUserID(), groupid, LST);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return RSLT;
        }

        #endregion

        #region BOLs 

        [HttpGet]
        [Route("billofladings/{clientid}/{customerid}/{index?}")]
        public List<billoflading> list_billofladings(int clientid, int customerid, int index = 0)
        {
            List<billoflading> LST = new List<billoflading>();
            try
            {
                using (billofladings db = new billofladings())
                {
                    LST = db.billofladings_list(ThisUserID(), clientid, customerid, index);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return LST;
        }

        [HttpGet]
        [Route("billoflading/{clientid}/{customerid}/{billofladingid}")]
        public billoflading billoflading_get(int clientid, int customerid, int billofladingid)
        {
            billoflading REC = new billoflading();
            try
            {
                using (billofladings db = new billofladings())
                {
                    REC = db.billoflading_get(ThisUserID(), clientid, customerid, billofladingid);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return REC;
        }

        [HttpGet]
        [Route("bolsearch/{clientid}/{bolnumber}/{index?}")]
        public List<billoflading> bolsearch_list(int clientid, string bolnumber, int index = 0)
        {
            List<billoflading> LST = new List<billoflading>();
            try
            {
                using (billofladings db = new billofladings())
                {
                    LST = db.bolsearch_list(ThisUserID(), clientid, bolnumber, index);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return LST;
        }

        [HttpGet]
        [Route("booksearch/{clientid}/{booknumber}/{index?}")]
        public List<billoflading> booksearch_list(int clientid, string booknumber, int index = 0)
        {
            List<billoflading> LST = new List<billoflading>();
            try
            {
                using (billofladings db = new billofladings())
                {
                    LST = db.bolsearch_list(ThisUserID(), clientid, booknumber, index);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return LST;
        }

        [HttpGet]
        [Route("billofladingnext/{clientid}/{customerid}/{bolnumber}")]
        public billoflading billoflading_next(int clientid, int customerid, string bolnumber)
        {
            billoflading REC = new billoflading();
            try
            {
                using (billofladings db = new billofladings())
                {
                    REC = db.billoflading_next(ThisUserID(), clientid, customerid, bolnumber);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return REC;
        }

        [HttpGet]
        [Route("billofladingprev/{clientid}/{customerid}/{bolnumber}")]
        public billoflading billoflading_prev(int clientid, int customerid, string bolnumber)
        {
            billoflading REC = new billoflading();
            try
            {
                using (billofladings db = new billofladings())
                {
                    REC = db.billoflading_prev(ThisUserID(), clientid, customerid, bolnumber);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return REC;
        }

        #endregion

        #region REGIONS 

        [HttpGet]
        [Route("regions")]
        public List<region> list_regions()
        {
            List<region> LST = new List<region>();
            try
            {
                using (misc db = new misc())
                {
                    LST = db.listregions(ThisUserID());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return LST;
        }


        [HttpPost]
        [Route("regions")]
        public dbresult regions_update(List<region> LST)
        {
            dbresult RSLT = new dbresult();
            try
            {
                using (misc db = new misc())
                {
                    RSLT = db.regions_update(ThisUserID(), LST);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return RSLT;
        }

        #endregion

        #region USERS

        [HttpGet]
        [Route("ddlusers")]
        public List<selectlistitem> ddl_users()
        {
            List<selectlistitem> LST = new List<selectlistitem>();
            try
            {
                using (users db = new users())
                {
                    LST = db.ddl_users(ThisUserID(), false);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return LST;
        }

        // group member list
        [HttpGet]
        [Route("memberships/{userid}")]
        public List<groupmember> list_memberships(int userid)
        {
            List<groupmember> LST = new List<groupmember>();
            try
            {
                using (groups db = new groups())
                {
                    LST = db.list_groupmembershipsall(ThisUserID(), userid);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return LST;
        }

        // entire data set
        [HttpPost]
        [Route("memberships/{userid}")]
        public dbresult update_memberships(int userid, List<groupmember> LST)
        {
            dbresult RSLT = new dbresult();
            try
            {
                using (groups db = new groups())
                {
                    RSLT = db.update_groupmembers(ThisUserID(), userid, LST);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return RSLT;
        }
        #endregion

        #region PERMISSIONS

        [HttpGet]
        [Route("permissions/{userid}/{clientid?}")]
        public List<permission> objectswithpermissions_list(int userid, int clientid = 0)
        {
            List<permission> LST = new List<permission>();
            try
            {
                using (security db = new security())
                {
                    LST = db.permissions_list(ThisUserID(), clientid);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return LST;
        }


        [HttpPost]
        [Route("permissions")]
        public dbresult permissions_update(List<permission> LST)
        {
            dbresult RSLT = new dbresult();
            try
            {
                using (security db = new security())
                {
                    RSLT = db.permissions_update(ThisUserID(), LST);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return RSLT;
        }

        [HttpGet]
        [Route("groupobjects/{clientid?}")]
        public List<groupobject> groupobjects_list(int clientid = 0)
        {
            List<groupobject> LST = new List<groupobject>();
            try
            {
                using (security db = new security())
                {
                    LST = db.groupobjects_list(ThisUserID(), clientid);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return LST;
        }

        #endregion

        #region MISC

        [HttpGet]
        [Route("containers/{clientid}/{billofladingid}")]
        public List<container> containers_list(int clientid, int billofladingid)
        {
            List<container> LST = new List<container>();
            try
            {
                using (misc db = new misc())
                {
                    LST = db.containers_list(ThisUserID(), clientid, billofladingid);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return LST;
        }

        [HttpGet]
        [Route("countries")]
        public List<selectlistitem> naics_ddl()
        {
            List<selectlistitem> LST = new List<selectlistitem>();
            try
            {
                using (misc db = new misc())
                {
                    LST = db.countries_ddl(ThisUserID(), true);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("db error:" + e.ToString());
            }
            return LST;
        }
        #endregion 
    }
}
