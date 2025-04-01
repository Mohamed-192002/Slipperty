using Infrastructure.Consts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> filter, string[] includes = null);
        IQueryable<T> GetAll(Expression<Func<T, bool>> filter, int take, int skip);
        IQueryable<T> GetAll(Expression<Func<T, bool>> filter, int take, int skip, string[] includes = null);
        IQueryable<T> GetAll(Expression<Func<T, bool>> filter, int? take, int? skip, Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending, string[] includes = null);
        //T Get(Expression<Func<T, bool>> filter, bool noTracking = false);
        T Get(Expression<Func<T, bool>> filter, string[] includes = null, bool noTracking = false);
        void Insert(T entity);
        void InsertRange(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        int Count();
        //int Count(Expression<Func<T, bool>> filter);

        void ExecuteTransaction(string SQLQueries);
        void ExecuteTransaction(Dictionary<object, EntityState> EF_Entities, params string[] SQLQueries);

        List<T> Exec_SqlQuery<T>(string SqlQuery, bool is_Stored = true, Dictionary<string, object> parameters = null);
        List<T> Exec_SqlQuery<T>(string SqlQuery, List<DataTable> sqlParameter, string[] sqlParameterNames, bool is_Stored = true, Dictionary<string, object> parameters = null);
        DataTable Exec_SqlQuery(string SqlQuery, Dictionary<string, object> parameters = null, bool is_Stored = true);
        DataSet Exec_SqlQueryDataSet(string SqlQuery, Dictionary<string, object> parameters = null, bool is_Stored = true);
    }
}
