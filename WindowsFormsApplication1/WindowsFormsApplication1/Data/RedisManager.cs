using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace RedisHelper
{
    public class RedisManager
    {
        private static PooledRedisClientManager prcm;

        /// <summary>
        /// 静态构造方法，初始化链接池管理对象
        /// </summary>
        static RedisManager()
        {
            CreateManager();
        }

        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        public static void CreateManager()
        {
            string writer = ConfigurationManager.ConnectionStrings["WriteServerConStr"].ToString();
            string read = ConfigurationManager.ConnectionStrings["ReadServerConStr"].ToString();
            string[] WriteServerConStr = SplitString(writer, ",");
            string[] ReadServerConStr = SplitString(read, ", ");
            prcm = new PooledRedisClientManager(ReadServerConStr, WriteServerConStr,
                             new RedisClientManagerConfig
                             {
                                 MaxWritePoolSize = int.Parse(ConfigurationManager.ConnectionStrings["MaxWritePoolSize"].ToString()),
                                 MaxReadPoolSize = int.Parse(ConfigurationManager.ConnectionStrings["MaxReadPoolSize"].ToString()),
                                 AutoStart = RedisConfig.AutoStart,
                             });
        }

        private static string[] SplitString(string strSource, string split)
        {
            return strSource.Split(split.ToArray());
        }
        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static IRedisClient GetClient()
        {
            if (prcm == null)
                CreateManager();
            return prcm.GetClient();
        }
    }
}
