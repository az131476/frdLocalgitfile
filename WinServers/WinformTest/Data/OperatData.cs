using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace WinformTest.Data
{
    public class OperatData
    {
        #region 将参数保存到数据库
        public void paramsSave(string sql)
        {
            MySqlConnection con = null;
            try
            {
                con = new MySqlConnection(ConfigurationManager.ConnectionStrings["myStr"].ToString());
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Debug.WriteErr("保存数据失败，" + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        #endregion
    }
}
