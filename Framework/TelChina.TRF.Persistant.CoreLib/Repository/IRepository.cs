using System;
using System.Collections.Generic;
using TelChina.TRF.Persistant.CoreLib.Entity;

namespace TelChina.TRF.Persistant.CoreLib.Repository
{

    /// <summary>
    ///  仓储基类,提供了PI 的接口封装
    ///  相关资料参见 http://martinfowler.com/eaaCatalog/repository.html
    /// </summary>
    /// <typeparam name="TEntity">仓储需要管理的实体类型 </typeparam>
    public interface IRepository<TEntity>
        where TEntity : EntityBase, IAgregateRoot
    {
        /// <summary>
        /// 向仓储中添加一个实体
        /// SaveChange前不会提交到数据库
        /// </summary>
        /// <param name="item">需要添加的实体</param>
        void Add(TEntity item);

        /// <summary>
        /// 删除一个实体 
        /// SaveChange前不会提交到数据库
        /// </summary>
        /// <param name="item">需要删除的实体</param>
        void Remove(TEntity item);

        /// <summary>
        /// 更新一个实体
        ///  SaveChange前不会提交到数据库
        /// </summary>
        /// <param name="item">需要更新的实体</param>
        void Modify(TEntity item);

        /// <summary>
        ///  按照实体Id查询单一实体
        /// </summary>
        /// <param name="id">实体Id</param>
        /// <returns>如果找到，则返回实体，否则返回null</returns>
        TEntity FindByID(Guid id);

        /// <summary>
        /// 从仓储中取出此类型的所有实体
        /// </summary>
        /// <returns> 实体结果集合,如果没有找到将会返回一个空集合,而不是null </returns>
        IEnumerable<TEntity> FindAll();

        /// <summary>
        /// 按照指定条件执行查询
        /// </summary>
        /// <param name="queryName">查询名称,此名称对应的查询语句已经在配置中存在,否则将抛出异常</param>
        /// <param name="param">查询对象</param>
        /// <param name="operation">查询选项,包括分页条件等</param>
        /// <returns>实体结果集合,如果没有找到将会返回一个空集合,而不是null </returns>
        IEnumerable<TEntity> FindAll(string queryName, QueryParam param, QueryOption operation);

        /// <summary>
        /// 提交更新
        /// </summary>
        /// <returns>影响的记录数,如果返回0表示没有更新</returns>
        int SaveChanges();

    }
}
