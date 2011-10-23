using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PronoFoot.Data.EntityFramework.Tests
{
    internal static class DatabaseTestUtility
    {
        public static void DropCreateDatabase()
        {
            PronoFootDbContext context = new PronoFootDbContext();
            DropAndCreateDatabase(context);
            context.SaveChanges();
        }

        private static void DropAndCreateDatabase(PronoFootDbContext context)
        {
            if (context.Database.Exists())
            {
                context.Database.Delete();
            }
            context.Database.Create();
        }
    }
}
