using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
namespace Arg.Ceva.DataAccess
{
    public class XrefGoodsTypes
    {
        [Table("XrefGoodsType")]
        public class XrefGoodsType
        {
            public string GOODSTYPE { get; set; }
            public string Description { get; set; }
        }

        public XrefGoodsType GetGoodsType(string code)
        {
            const string query = @"SELECT * FROM XrefGoodsType WHERE GOODSTYPE=@GOODSTYPE;";

            using var connection = Common.ClientDatabase;
            var goodsType = connection.QueryFirstOrDefault<XrefGoodsType>(query, new { @GOODSTYPE = code });
            return goodsType;
        }
    }
}
