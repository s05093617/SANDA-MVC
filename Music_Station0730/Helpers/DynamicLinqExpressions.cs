using System;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace Music_Station0730.Helpers.DyLinq
{
    public static class DynamicLinqExpressions//注意static靜態型別   
    {

        /// 构造函数使用True时：单个AND有效，多个AND有效；单个OR无效，多个OR无效；混合时写在AND后的OR有效
        /// 构造函数使用False时：单个AND无效，多个AND无效；单个OR有效，多个OR有效；混合时写在OR后面的AND有效

        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        //注意this   
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// 指定排序的欄位及方法(遞增 or 遞減)
        /// </summary>
        /// <typeparam name="T">排序</typeparam>
        /// <param name="source">IQueryable</param>
        /// <param name="propertyName">欄位名稱</param>
        /// <param name="orderType">排序方式(Asc、Desc)</param>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, string orderType)
        {
            if (propertyName.IndexOf('.') > 0)
                return MutiOrderBy<T>(source, propertyName, orderType);

            var type = typeof(T);
            string methodName = orderType == "Asc" ? "OrderBy" : "OrderByDescending";

            var property = type.GetProperty(propertyName);

            if (property == null)
            {
                //new MadHatter.ErrorHandlers.ErrorLogs().ExceptionLog(new Exception("OrderByException !! \n屬性[" + propertyName + "]並不屬於該資料表[" + type.Name + "]"), ErrorHandlers.ApplicationErrorType.ApplicationError, "", null, 500);
                return source;
            }
            else
            {
                var parameter = Expression.Parameter(type, "p");
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);

                MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
                return source.Provider.CreateQuery<T>(resultExp);
            }
        }

        /// <summary>
        /// 指定排序的欄位及方法(以關聯資料表欄位做排序)
        /// </summary>
        /// <typeparam name="T">排序</typeparam>
        /// <param name="source">IQueryable</param>
        /// <param name="propertyName">欄位名稱</param>
        /// <param name="orderType">排序方式(Asc、Desc)</param>
        private static IQueryable<T> MutiOrderBy<T>(this IQueryable<T> source, string properties, string orderType)
        {
            var type = typeof(T);
            string methodName = orderType == "Asc" ? "OrderBy" : "OrderByDescending";

            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            if (!SplitTypeAndExpression<T>(properties, arg, out type, out expr))
            {
                return source;
            }

            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            // 取得指定的排序方法並傳入參數以取得執行結果
            object result = typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<T>)result;
        }

        /// <summary>
        /// 執行 like 查詢(若關鍵字為空orNull則不做查詢動作)
        /// </summary>
        /// <typeparam name="T">查詢某欄位的關鍵字</typeparam>
        /// <param name="source">IQueryable</param>
        /// <param name="propertyName">欄位名稱</param>
        /// <param name="keyword">關鍵字</param>
        public static IQueryable<T> Like<T>(this IQueryable<T> source, string propertyName, string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return source;
            }

            if (propertyName.IndexOf('.') > 0)
                return MutiLike<T>(source, propertyName, keyword);

            var type = typeof(T);
            var property = type.GetProperty(propertyName);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var constant = Expression.Constant("%" + keyword + "%");
            var like = typeof(SqlMethods).GetMethod("Like",
                       new Type[] { typeof(string), typeof(string) });

            MethodCallExpression methodExp = Expression.Call(null, like, propertyAccess, constant);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(methodExp, parameter);

            return source.Where(lambda);
        }

        /// <summary>
        /// 執行 like 查詢(以關聯資料表欄位做查詢)
        /// </summary>
        /// <typeparam name="T">查詢某欄位的關鍵字</typeparam>
        /// <param name="source">IQueryable</param>
        /// <param name="propertyName">欄位名稱</param>
        /// <param name="keyword">關鍵字</param>
        private static IQueryable<T> MutiLike<T>(this IQueryable<T> source, string properties, string keyword)
        {
            var type = typeof(T);

            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            if (!SplitTypeAndExpression<T>(properties, arg, out type, out expr))
            {
                return source;
            }

            var constant = Expression.Constant("%" + keyword + "%");
            var like = typeof(SqlMethods).GetMethod("Like",
                       new Type[] { typeof(string), typeof(string) });

            MethodCallExpression methodExp = Expression.Call(null, like, expr, constant);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(methodExp, arg);

            return source.Where(lambda);
        }

        /// <summary>
        /// 解析關聯屬性並傳回相關的 type 及 lambda
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="delegateType"></param>
        /// <param name="lambda"></param>
        private static bool SplitTypeAndExpression<T>(string properties, ParameterExpression arg, out Type type, out Expression expr)
        {
            type = typeof(T);
            expr = arg;
            foreach (string prop in properties.Split('.'))
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                if (pi == null)
                {
                    //new MadHatter.ErrorHandlers.ErrorLogs().ExceptionLog(new Exception("OrderByException !! \n屬性[" + prop + "]並不屬於該資料表[" + type.Name + "]"), ErrorHandlers.ApplicationErrorType.ApplicationError, "", null, 500);
                    return false;
                }
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            return true;
        }
    }

}
