
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Objects.DataClasses;
using TelChina.TRF.Persistant.Core.Resource;
using System.Data;
using TelChina.TRF.Domain.Core.Specification;
using TelChina.TRF.Domain.Core.Extentions;
using TelChina.TRF.Debug.ObjectStateVisualizer;


namespace TelChina.TRF.Domain.Core
{
    /// <summary>
    /// Default base class for repostories. This generic repository 
    /// is a default implementation of <see cref=" TelChina.TRF.Domain.Core.IRepository{TEntity}"/>
    /// and your specific repositories can inherit from this base class so automatically will get default implementation.
    /// IMPORTANT: Using this Base Repository class IS NOT mandatory. It is just a useful base class:
    /// You could also decide that you do not want to use this base Repository class, because sometimes you don't want a 
    /// specific Repository getting all these features and it might be wrong for a specific Repository. 
    /// For instance, you could want just read-only data methods for your Repository, etc. 
    /// in that case, just simply do not use this base class on your Repository.
    /// </summary>
    /// <typeparam name="TEntity">Type of elements in repostory</typeparam>
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : EntityObject//, IObjectWithChangeTracker
    {

        #region Members

        //ITraceManager _TraceManager = null;
        //IQueryableUnitOfWork _CurrentUoW;
        ObjectContext ctx;
        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of Repository
        /// </summary>
        /// <param name="ctx"></param>
        internal Repository(ObjectContext ctx)
        {
            this.ctx = ctx;
        }

        #endregion

        #region IRepository<TEntity> Members

        /// <summary>
        /// <see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/>
        /// </summary>
        /// <param name="item"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        public virtual void Add(TEntity item)
        {
            //check item
            if (item == (TEntity)null)
                throw new ArgumentNullException("item", Messages.exception_ItemArgumentIsNull);
            CreateSet().AddObject(item);
            //this.ctx.AddObject(item.EntityKey.EntitySetName, item);
            //if (item.EntityState == EntityState.Added)
            //{
            //    this.ctx.Attach(item);
            //}          
            //else
            //    throw new InvalidOperationException(Messages.exception_ChangeTrackerIsNullOrStateIsNOK);
        }
        /// <summary>
        /// <see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/>
        /// </summary>
        /// <param name="item"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        public virtual void Remove(TEntity item)
        {
            //check item
            if (item == (TEntity)null)
                throw new ArgumentNullException("item", Messages.exception_ItemArgumentIsNull);


            IObjectSet<TEntity> objectSet = CreateSet();

            //Attach object to unit of work and delete this
            // this is valid only if T is a type in model
            objectSet.Attach(item);

            //delete object to IObjectSet for this type
            objectSet.DeleteObject(item);
        }
        /// <summary>
        /// <see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/>
        /// </summary>
        /// <param name="item"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        public virtual void RegisterItem(TEntity item)
        {
            if (item == (TEntity)null)
                throw new ArgumentNullException("item");

            (CreateSet()).Attach(item);
        }
        /// <summary>
        /// <see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/>
        /// </summary>
        /// <param name="item"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        public virtual void Modify(TEntity item)
        {
            //check arguments
            if (item == (TEntity)null)
                throw new ArgumentNullException("item", Messages.exception_ItemArgumentIsNull);

            //Set modifed state if change tracker is enabled and state is not deleted
            if (((item.EntityState & EntityState.Deleted) != EntityState.Deleted)
               )
            {
                if (item.EntityState != EntityState.Modified)
                    this.ctx.ObjectStateManager.ChangeObjectState(item, EntityState.Modified);
            }
            //apply changes for item object
            //_CurrentUoW.RegisterChanges(item);
        }
        /// <summary>
        /// <see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/>
        /// </summary>
        /// <returns><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            //Create IObjectSet and perform query 
            return (CreateSet()).AsEnumerable<TEntity>();
        }
        /// <summary>
        /// <see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/>
        /// </summary>
        /// <param name="specification"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <returns><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></returns>
        public virtual IEnumerable<TEntity> GetBySpec(ISpecification<TEntity> specification)
        {
            if (specification == (ISpecification<TEntity>)null)
                throw new ArgumentNullException("specification");

            return (CreateSet().Where(specification.SatisfiedBy())
                                     .AsEnumerable<TEntity>());

        }

        /// <summary>
        /// <see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/>
        /// </summary>
        /// <param name="pageIndex"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <param name="pageCount"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <param name="orderByExpression"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <param name="ascending"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <returns><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></returns>
        public virtual IEnumerable<TEntity> GetPagedElements<S>(int pageIndex, int pageCount,
            System.Linq.Expressions.Expression<Func<TEntity, S>> orderByExpression, bool ascending)
        {
            //checking arguments for this query 
            if (pageIndex < 0)
                throw new ArgumentException(Messages.exception_InvalidPageIndex, "pageIndex");

            if (pageCount <= 0)
                throw new ArgumentException(Messages.exception_InvalidPageCount, "pageCount");

            if (orderByExpression == (Expression<Func<TEntity, S>>)null)
                throw new ArgumentNullException("orderByExpression", Messages.exception_OrderByExpressionCannotBeNull);

            //Create associated IObjectSet and perform query

            IObjectSet<TEntity> objectSet = CreateSet();

            return objectSet.Paginate<TEntity, S>(orderByExpression, pageIndex, pageCount, ascending);
        }


        /// <summary>
        /// <see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/>
        /// </summary>
        /// <typeparam name="S"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></typeparam>
        /// <param name="pageIndex"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <param name="pageCount"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <param name="orderByExpression"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <param name="specification"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <param name="ascending"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <returns><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></returns>
        public virtual IEnumerable<TEntity> GetPagedElements<S>(int pageIndex, int pageCount, Expression<Func<TEntity, S>> orderByExpression, ISpecification<TEntity> specification, bool ascending)
        {
            //checking arguments for this query 
            if (pageIndex < 0)
                throw new ArgumentException(Messages.exception_InvalidPageIndex, "pageIndex");

            if (pageCount <= 0)
                throw new ArgumentException(Messages.exception_InvalidPageCount, "pageCount");

            if (orderByExpression == (Expression<Func<TEntity, S>>)null)
                throw new ArgumentNullException("orderByExpression", Messages.exception_OrderByExpressionCannotBeNull);

            if (specification == (ISpecification<TEntity>)null)
                throw new ArgumentNullException("specification", Messages.exception_SpecificationIsNull);

            //_TraceManager.TraceInfo(
            //               string.Format(CultureInfo.InvariantCulture,
            //                            Resources.Messages.trace_GetPagedElementsRepository,
            //                            typeof(TEntity).Name, pageIndex, pageCount, orderByExpression.ToString()));

            //Create associated IObjectSet and perform query

            IObjectSet<TEntity> objectSet = CreateSet();

            //this query cannot  use Paginate IQueryable extension method because Linq queries cannot be
            //merged with Object Builder methods. See Entity Framework documentation for more information

            return (ascending)
                                ?
                                    objectSet
                                     .Where(specification.SatisfiedBy())
                                     .OrderBy(orderByExpression)
                                     .Skip(pageIndex * pageCount)
                                     .Take(pageCount)
                                     .ToList()
                                :
                                    objectSet
                                     .Where(specification.SatisfiedBy())
                                     .OrderByDescending(orderByExpression)
                                     .Skip(pageIndex * pageCount)
                                     .Take(pageCount)
                                     .ToList();
        }


        /// <summary>
        /// <see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/>
        /// </summary>
        /// <param name="filter"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <returns><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></returns>
        public virtual IEnumerable<TEntity> GetFilteredElements(Expression<Func<TEntity, bool>> filter)
        {
            //checking query arguments
            if (filter == (Expression<Func<TEntity, bool>>)null)
                throw new ArgumentNullException("filter", Messages.exception_FilterCannotBeNull);

            //Create IObjectSet and perform query
            return CreateSet().Where(filter)
                                    .ToList();
        }

        /// <summary>
        /// <see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/>
        /// </summary>
        /// <param name="filter"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <param name="orderByExpression"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <param name="ascending"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <returns><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></returns>
        public virtual IEnumerable<TEntity> GetFilteredElements<S>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, S>> orderByExpression, bool ascending)
        {
            //Checking query arguments
            if (filter == (Expression<Func<TEntity, bool>>)null)
                throw new ArgumentNullException("filter", Messages.exception_FilterCannotBeNull);

            if (orderByExpression == (Expression<Func<TEntity, S>>)null)
                throw new ArgumentNullException("orderByExpression", Messages.exception_OrderByExpressionCannotBeNull);


            //Create IObjectSet for this type and perform query
            IObjectSet<TEntity> objectSet = CreateSet();

            return (ascending)
                                ?
                                    objectSet
                                     .Where(filter)
                                     .OrderBy(orderByExpression)
                                     .ToList()
                                :
                                    objectSet
                                     .Where(filter)
                                     .OrderByDescending(orderByExpression)
                                     .ToList();
        }

        /// <summary>
        /// <see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/>
        /// </summary>
        /// <param name="filter"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <param name="pageIndex"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <param name="pageCount"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <param name="orderByExpression"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <param name="ascending"><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></param>
        /// <returns><see cref="TelChina.TRF.Domain.Core.IRepository{TEntity}"/></returns>
        public virtual IEnumerable<TEntity> GetFilteredElements<S>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, Expression<Func<TEntity, S>> orderByExpression, bool ascending)
        {

            //checking query arguments
            if (filter == (Expression<Func<TEntity, bool>>)null)
                throw new ArgumentNullException("filter", Messages.exception_FilterCannotBeNull);

            if (pageIndex < 0)
                throw new ArgumentException(Messages.exception_InvalidPageIndex, "pageIndex");

            if (pageCount <= 0)
                throw new ArgumentException(Messages.exception_InvalidPageCount, "pageCount");

            if (orderByExpression == (Expression<Func<TEntity, S>>)null)
                throw new ArgumentNullException("orderByExpression", Messages.exception_OrderByExpressionCannotBeNull);


            //Create IObjectSet for this particular type and query this
            IObjectSet<TEntity> objectSet = CreateSet();

            return (ascending)
                                ?
                                    objectSet
                                     .Where(filter)
                                     .OrderBy(orderByExpression)
                                     .Skip(pageIndex * pageCount)
                                     .Take(pageCount)
                                     .ToList()
                                :
                                    objectSet
                                     .Where(filter)
                                     .OrderByDescending(orderByExpression)
                                     .Skip(pageIndex * pageCount)
                                     .Take(pageCount)
                                     .ToList();


        }
        #endregion

        #region Private Methods

        IObjectSet<TEntity> CreateSet()
        {
            var t = typeof(TEntity).BaseType;
            //try
            //{
            return ctx.CreateObjectSet<TEntity>();
            //}
            //catch (Exception ex)
            //{
            //    //ctx.CreateObjectSet<t>();
            //}
            //set merge option to underlying ObjectQuery
            //ObjectQuery<TEntity> query = objectSet as ObjectQuery<TEntity>;

            //if (query != null) // check if this objectset is not in memory object set for testing
            //    query.MergeOption = MergeOption.NoTracking;
            //return objectSet;

        }
        #endregion

        //public void BeginTransaction()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Commit()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Rollback()
        //{
        //    throw new NotImplementedException();
        //}
        /// <summary>
        /// 提交更新
        /// </summary>
        /// <returns>更新的数据行数</returns>
        public int SaveChanges()
        {
            if (TelChina.TRF.Util.Common.ConfigHelper.GetConfigValue("TraceEntityState").ToLower() == "true")
            {
                TraceEntityState();
            }
            ctx.DetectChanges();
            var entries =
                from ose in ctx.ObjectStateManager.GetObjectStateEntries
                    (EntityState.Added | EntityState.Modified)
                where ose.Entity != null
                select ose;
            foreach (var ose in entries)
            {
                var BusinessEntity = ose.Entity as IEntity;
                if (BusinessEntity != null)
                {
                    if (ose.State != EntityState.Added)
                    {
                        //控制并发
                        BusinessEntity.SysVersion++;
                    }
                    //设默认值
                    BusinessEntity.SetDefaultValue();
                    //实体校验
                    BusinessEntity.OnValidate();
                }
            }
            return ctx.SaveChanges();
        }

        private void TraceEntityState()
        {

            if (ctx.ObjectStateManager.GetObjectStateEntries().Count(x => x.Entity != null) > 0)
            {
                ctx.TraceAllEntityState();
                //TelChina.TRF.Debug.ObjectStateVisualizer.Visualizer.TraceAllEntityState(ctx);
            }
        }
    }
}
