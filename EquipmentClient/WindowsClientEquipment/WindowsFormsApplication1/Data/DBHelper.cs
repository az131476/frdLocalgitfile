using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace WindowsFormsApplication1.Data
{
    class DBHelper
    {
        /// <summary>
        /// execute sql/insert /delete/update
        /// </summary>
        /// <param name="sql"></param>
        public bool updateDB(string sql)
        {
            Debug.Write(sql);
            MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["myStr"].ToString());
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                
                return false;
            }
            finally
            {
                con.Close();
            }
            return true;
        }
        public bool exist(string sql)
        {
            MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["myStr"].ToString());
            MySqlDataReader reader = null;
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sql, con);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    reader[0].ToString();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
            return true;
        }
        public void reader(string sql)
        {
            
        }
    }
}
