using Infrastructure.Consts;
using Infrastructure.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public BaseRepository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public void Insert(T entity)
        {
            dbSet.Add(entity);
        }

        public void InsertRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }

        //public T Get(Expression<Func<T, bool>> filter, bool noTracking = false)
        //{
        //    IQueryable<T> query = dbSet;
        //    if (noTracking)
        //        query = query.AsNoTracking();

        //    query = query.Where(filter);

        //    return query.FirstOrDefault();
        //}

        public T Get(Expression<Func<T, bool>> filter, string[] includes = null, bool noTracking = false)
        {
            IQueryable<T> query = dbSet;

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (noTracking)
                query = query.AsNoTracking();

            query = query.Where(filter);

            return query.FirstOrDefault();
        }

        public IQueryable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query;
        }
        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter, string[] includes = null)
        {
            IQueryable<T> query = dbSet;

            query = query.Where(filter);

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter, int take, int skip)
        {
            IQueryable<T> query = dbSet;

            query = query.Where(filter);

            query = query.Take(take).Skip(skip);


            return query;
        }
        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter, int take, int skip, string[] includes = null)
        {
            IQueryable<T> query = dbSet;

            query = query.Where(filter);

            query = query.Take(take).Skip(skip);

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter, int? take, int? skip, Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending, string[] includes = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);

            if (take.HasValue) query = query.Take(take.Value);
            if (skip.HasValue) query = query.Skip(skip.Value);

            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query;
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            dbSet.UpdateRange(entities);
        }


        public int Count()
        {
            return dbSet.Count();
        }

        public int Count(Expression<Func<T, bool>> filter)
        {
            return dbSet.Count(filter);
        }

        public void ExecuteTransaction(string SQLQueries)
        {
            using (var dbContextTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (!string.IsNullOrEmpty(SQLQueries))
                    {
                        FormattableString sql_query = FormattableStringFactory.Create(SQLQueries);
                        int affRows = _db.Database.ExecuteSqlInterpolated(sql_query);
                    }

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    // Attempt to roll back the transaction.
                    try
                    {
                        dbContextTransaction.Rollback();
                        throw;
                    }
                    catch (Exception ex2)
                    {
                        throw;
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                    }
                    throw;
                }
            }
        }

        public void ExecuteTransaction(Dictionary<object, EntityState> EF_Entities, params string[] SQLQueries)
        {
            using (var dbContextTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    for (int i = 0; i < SQLQueries.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(SQLQueries[i]))
                        {
                            FormattableString sql_query = FormattableStringFactory.Create(SQLQueries[i]);
                            // sql_query = FormattableStringFactory.Create( "*");
                            _db.Database.ExecuteSqlInterpolated(sql_query);
                        }
                    }

                    // Save entity in context B
                    /* 2- Run an EF command in the transaction */
                    if (EF_Entities != null)
                    {
                        foreach (var entity in EF_Entities)
                        {
                            if (entity.Value == EntityState.Added)
                            {
                                _db.Add(entity.Key);
                            }

                            else if (entity.Value == EntityState.Modified)
                            {
                                _db.Attach(entity.Key);
                                _db.Entry(entity.Key).State = EntityState.Modified;
                            }

                            //else if (entity.Value == EntityState.Deleted)
                            //{
                            //    if (Context.Entry(EF_Entities).State == EntityState.Detached)
                            //        dbset.Attach(entity.Key as T);
                            //    dbset.Remove(entity.Key as T);
                            //}
                        }
                    }

                    _db.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    // Attempt to roll back the transaction.
                    try
                    {
                        dbContextTransaction.Rollback();
                        throw;
                    }
                    catch (Exception ex2)
                    {
                        throw;
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                    }
                    throw;
                }
            }
        }


        public DataTable Exec_SqlQuery(string SqlQuery, Dictionary<string, object> parameters = null, bool is_Stored = true)
        {
            using (SqlConnection con = new SqlConnection(_db.Database.GetDbConnection().ConnectionString))
            {
                con.Open();
                using (SqlCommand dbCommand = new SqlCommand(SqlQuery, con))
                {
                    dbCommand.CommandType = is_Stored ? CommandType.StoredProcedure : CommandType.Text;
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            dbCommand.Parameters.AddWithValue($"@{param.Key}", param.Value ?? DBNull.Value);
                        }
                    }
                    using (var result = dbCommand.ExecuteReader())
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(result);
                        return dataTable;
                    }
                }
            }
        }

        public DataSet Exec_SqlQueryDataSet(string SqlQuery, Dictionary<string, object> parameters = null, bool is_Stored = true)
        {
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(_db.Database.GetDbConnection().ConnectionString))
            {
                con.Open();
                using (SqlCommand dbCommand = new SqlCommand(SqlQuery, con))
                {
                    dbCommand.CommandType = is_Stored ? CommandType.StoredProcedure : CommandType.Text;
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            dbCommand.Parameters.AddWithValue($"@{param.Key}", param.Value ?? DBNull.Value);
                        }
                    }
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = dbCommand;
                    da.Fill(ds);
                }
            }
            return ds;

        }

        public List<T> Exec_SqlQuery<T>(string SqlQuery, bool is_Stored = true, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection con = new SqlConnection(_db.Database.GetDbConnection().ConnectionString))
            {
                con.Open();
                using (SqlCommand dbCommand = new SqlCommand(SqlQuery, con))
                {
                    dbCommand.CommandType = is_Stored ? CommandType.StoredProcedure : CommandType.Text;
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            // dbCommand.Parameters.Add(new SqlParameter($"@{param.Key}", param.Value));
                            dbCommand.Parameters.AddWithValue($"@{param.Key}", param.Value);
                        }
                    }

                    using (var result = dbCommand.ExecuteReader())
                    {
                        //Type returnType = typeof(T);
                        //if (returnType == typeof(DataTable))
                        //{
                        //    var dataTable = new DataTable();
                        //    dataTable.Load(result);
                        //    return dataTable.AsEnumerable();
                        //}

                        var entities = new List<T>();
                        while (result.Read())
                        {
                            entities.Add(Map<T>(result));
                        }
                        return entities;
                    }
                }
            }
        }
        public List<T> Exec_SqlQuery<T>(string SqlQuery, List<DataTable> sqlParameter, string[] sqlParameterNames, bool is_Stored = true, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection con = new SqlConnection(_db.Database.GetDbConnection().ConnectionString))
            {
                con.Open();
                using (SqlCommand dbCommand = new SqlCommand(SqlQuery, con))
                {
                    dbCommand.CommandType = is_Stored ? CommandType.StoredProcedure : CommandType.Text;
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            // dbCommand.Parameters.Add(new SqlParameter($"@{param.Key}", param.Value));
                            dbCommand.Parameters.AddWithValue($"@{param.Key}", param.Value);
                        }
                    }

                    for (int i = 0; i < sqlParameter.Count(); i++)
                    {

                        dbCommand.Parameters.Add(new SqlParameter($"@{sqlParameterNames[i]}", SqlDbType.Structured)
                        {
                            Direction = ParameterDirection.Input,
                            Value = sqlParameter[0],
                        });
                    }


                    using (var result = dbCommand.ExecuteReader())
                    {
                        //Type returnType = typeof(T);
                        //if (returnType == typeof(DataTable))
                        //{
                        //    var dataTable = new DataTable();
                        //    dataTable.Load(result);
                        //    return dataTable.AsEnumerable();
                        //}

                        var entities = new List<T>();
                        while (result.Read())
                        {
                            entities.Add(Map<T>(result));
                        }
                        return entities;
                    }
                }
            }
        }

        protected T Map<T>(IDataRecord record)
        {
            var objT = Activator.CreateInstance<T>();
            foreach (var property in typeof(T).GetProperties())
            {
                if (record.HasColumn(property.Name) && !record.IsDBNull(record.GetOrdinal(property.Name)))
                {
                    Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    object safeValue = (record[property.Name] == null) ? null : Convert.ChangeType(record[property.Name], type);
                    property.SetValue(objT, safeValue, null);
                }
            }
            return objT;
        }
    }
}
