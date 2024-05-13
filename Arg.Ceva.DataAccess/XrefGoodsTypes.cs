using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;
namespace Arg.Ceva.DataAccess
{
    public class XrefGoodsTypes
    {
        private readonly SqlConnection _connection;

        public XrefGoodsTypes()
        {
            _connection = Common.ClientDatabase;
        }

        [Table("XrefGoodsType")]
        public class XrefGoodsType
        {
            public string GOODSTYPE { get; set; }
            public string Description { get; set; }
        }

        public XrefGoodsType GetGoodsType(string code)
        {
            const string query = @"SELECT * 
                                   FROM XrefGoodsType 
                                   WHERE GOODSTYPE=@GOODSTYPE;";

            return _connection.QueryFirstOrDefault<XrefGoodsType>(query, new { @GOODSTYPE = code });
        }
    }
}
