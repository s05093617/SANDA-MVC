using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Music_Station0730.Extentions
{
    public static class IQueryableExtension
    {
        /// 

        /// DbQuery轉ObjectQuery
        /// 
        /// 
        /// 
        /// 
        public static ObjectQuery Get_ObjectQuery(this IQueryable query)
        {
            var internalQuery = query.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(field => field.Name == "_internalQuery")
                .Select(field => field.GetValue(query))
                .First();

            var objectQuery = internalQuery.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(field => field.Name == "_objectQuery")
                .Select(field => field.GetValue(internalQuery))
                .Cast<ObjectQuery>()
                .First();

            return objectQuery;
        }
    }
}