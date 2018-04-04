using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedisHelper;

namespace WindowsFormsApplication1.Data
{
    class Test
    {
        public void test()
        {
            string key = "Users";
            RedisBase.Core.FlushAll();
            RedisBase.Core.AddItemToList(key, "cuiyanwei");
            RedisBase.Core.AddItemToList(key, "xiaoming");
            RedisBase.Core.Add<string>("mykey", "123456");
            RedisString.Set("mykey1", "abcdef");
        }
    }
}
