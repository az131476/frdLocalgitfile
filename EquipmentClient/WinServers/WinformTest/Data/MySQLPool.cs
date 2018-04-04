using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Threading;
using System.Data;

namespace WinformTest.Data
{
    class MySQLPool
    {
        ///<summary>
        ///数据库存储轻量数据/参数/日志等、检索
        /// </summary>
        private static volatile MySQLPool pool;
        private Hashtable map;

        private int initPoolSize = 2;
        private int maxPoolSize = 5;
        private int waitTime = 100;

        private static Object lockObj = new Object();

        private MySQLPool()
        {
            init();
        }

        public static MySQLPool getInstance()
        {
            if (pool == null)
            {
                lock (lockObj)
                {
                    if (pool == null)
                    {
                        pool = new MySQLPool();
                    }
                }
            }
            return pool;
        }

        private void init()
        {
            try
            {
                map = Hashtable.Synchronized(new Hashtable());
                for (int i = 0; i < initPoolSize; i++)
                {
                    map.Add(getNewConnection(), true);
                }
            }
            catch (System.Exception ex)
            {
                //
            }
        }

        private MySqlConnection getNewConnection()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["myStr"].ToString());
                conn.Open();
                return conn;
            }
            catch (System.Exception ex)
            {
                //
            }
            return null;
        }

        public MySqlConnection getConnection()
        {
            lock (lockObj)
            {
                MySqlConnection conn = null;
                try
                {
                    foreach (DictionaryEntry item in map)
                    {
                        if ((Boolean)item.Value)
                        {
                            conn = (MySqlConnection)item.Key;
                            map[conn] = false;
                            break;
                        }
                    }

                    if (conn == null)
                    {
                        if (map.Count < maxPoolSize)
                        {
                            conn = getNewConnection();
                            map.Add(conn, false);
                        }
                        else
                        {
                            Thread.Sleep(waitTime);
                            conn = getConnection();
                        }
                    }
                }
                catch (System.Exception ex)
                {

                    return null;
                }
                return conn;
            }
        }

        public void releaseConnection(MySqlConnection conn)
        {
            lock (lockObj)
            {
                if (conn == null)
                {
                    return;
                }
                try
                {
                    if (map.ContainsKey(conn))
                    {
                        map[conn] = true;

                        if (conn.State == ConnectionState.Closed)
                        {
                            map.Remove(conn);
                        }
                    }
                    else
                    {
                        conn.Close();
                    }
                }
                catch (System.Exception ex)
                {
                    //
                }
            }
        }
    }
}
