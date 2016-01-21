using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EFTEST
{
    public class Test
    {
        static void Main(string[] args)
        {
            DevOrangeEntities database = new DevOrangeEntities();

            var user = database.UserGet(1);
        }
    }
}
