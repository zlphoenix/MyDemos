
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TelChina.TRF.TestBase.PerformanceCounter;
using System.Transactions;
using TelChina.TRF.Demo.DomainModel.Model;
using TelChina.TRF.Domain.Core;
using System.Data.Objects;
using TelChina.TRF.Domain.Core.Specification;

namespace TelChina.TRF.Demo.DomainModelTest
{


    /// <summary>
    ///This is a test class for SysParamBLLTest and is intended
    ///to contain all SysParamBLLTest Unit Tests
    ///</summary>
    [TestClass]
    public class EntityCRUDTest
    {
        private const int BATCHCOUNT = 1000;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        private IRepository<SysParam> _repository;
        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{          
        //}
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void MyTestInitialize()
        {

            ObjectContext edmContext = new DemoEntities();
            _repository = EDMRepositoryContext.GetCurrentContext(edmContext).GetRepository<SysParam>();
            //清理垃圾数据
            var rubbish = _repository.GetFilteredElements(x => x.sysparamName.StartsWith("ParamName"));
            if (rubbish == null || rubbish.Count() <= 0) return;
            foreach (var item in rubbish)
                this._repository.Remove(item);

            _repository.SaveChanges();
        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void MyTestCleanup()
        {
            if (_gc.Count > 0)
            {
                foreach (var item in _gc)
                {
                    DeleteEntity(item);
                }
                _gc.Clear();
            }
        }

        #endregion

        #region Infrastructure

        /// <summary>
        /// 垃圾回收器
        /// </summary>
        private readonly List<SysParam> _gc = new List<SysParam>();
        /// <summary>
        /// 实体计数器
        /// </summary>
        private int _entityCount;

        /// <summary>
        /// 创建实体，并加入垃圾回收器
        /// 每创建一个实体计数器加一
        /// </summary>
        /// <returns></returns>
        private SysParam CreateEntity()
        {
            //Create Entity
            var sysParam = new SysParam(); // TODO: Initialize to an appropriate value
            //sysParam.sysparamid;
            SetEntityFields(sysParam);
            _gc.Add(sysParam);

            return sysParam;
        }
        /// <summary>
        /// 为实体的字段赋值,计数器增量
        /// </summary>
        /// <param name="sysParam"></param>
        private void SetEntityFields(SysParam sysParam)
        {
            sysParam.sysparamName = "ParamName" + _entityCount;
            sysParam.sysparamDsc = "ParamDsc" + _entityCount;
            sysParam.sysparamValue = "ParamValue" + _entityCount;

            _entityCount++;
        }

        /// <summary>
        /// 执行数据库新增操作
        /// </summary>
        /// <returns></returns>
        private SysParam AddNewEntity()
        {
            var sysParam = CreateEntity();
            _repository.Add(sysParam);
            var actual = _repository.SaveChanges() > 0;
            Assert.AreEqual(true, actual, "实体新增失败!");
            return sysParam;
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        private int DeleteEntity(SysParam entity)
        {
            if (entity == null)
            {
                return -1;
            }
            _repository.Remove(entity);
            var result = _repository.SaveChanges();
            return result;
        }
        /// <summary>
        /// 实体比较
        /// </summary>
        /// <param name="entity1"></param>
        /// <param name="entity2"></param>
        /// <returns></returns>
        private bool CompareEntities(SysParam entity1, SysParam entity2)
        {
            if (entity1 == null || entity2 == null)
            {
                return false;
            }

            if (entity1.sysparamDsc == entity2.sysparamDsc
                && entity1.sysparamName == entity2.sysparamName
                && entity1.sysparamValue == entity2.sysparamValue)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 验证数据库中是否存在相同实体
        /// </summary>
        /// <param name="sysParam"></param>
        private void ValidateEntity(SysParam sysParam)
        {
            var entityFound = FindEntity(sysParam);
            Assert.IsTrue(CompareEntities(sysParam, entityFound), "查询出的实体与新增的实体不一致");
        }

        private SysParam FindEntity(SysParam sysParam)
        {
            var entityFound = _repository.GetBySpec(
                new DirectSpecification<SysParam>(s => s.sysparamName == sysParam.sysparamName)).FirstOrDefault();
            return entityFound;
        }
        /// <summary>
        /// 批量验证
        /// </summary>
        /// <param name="entityList"></param>
        private void ValidateEntitys(IEnumerable<SysParam> entityList)
        {
            foreach (var item in entityList)
            {
                ValidateEntity(item);
            }
        }

        /// <summary>
        /// 通过ID查询实体
        /// </summary>
        /// <param name="sysparamid"></param>
        /// <returns></returns>
        private SysParam FindEntityById(long sysparamid)
        {
            var foundEntity = _repository.GetBySpec(
                new DirectSpecification<SysParam>(s => s.sysparamid == sysparamid))
                .FirstOrDefault();
            return foundEntity;
        }
        /// <summary>
        /// 创建事务作用域
        /// </summary>
        /// <param name="tso">事务级别</param>
        /// <returns></returns>
        private TransactionScope CreateTrans(TransactionScopeOption tso)
        {
            var txSettings = new TransactionOptions
                                 {
                                     Timeout = TransactionManager.DefaultTimeout,
                                     IsolationLevel = IsolationLevel.Serializable// review this option
                                 };
            return new TransactionScope(tso, txSettings);
        }
        #endregion Infrastructure

        #region FunctionalityTest
        /// <summary>
        ///A test for SysParamAdd
        ///</summary>
        [TestMethod]
        public void EntityAddTest()
        {
            SysParam sysParam = AddNewEntity();
            ValidateEntity(sysParam);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /// <summary>
        ///A test for SysParamSelOne
        ///</summary>
        [TestMethod]
        public void EntityFindTest()
        {
            SysParam sysParam = AddNewEntity();
            var entityFound = FindEntity(sysParam);
            long sysParamID = entityFound.sysparamid; // TODO: Initialize to an appropriate value
            SysParam expected = entityFound; // TODO: Initialize to an appropriate value
            SysParam actual = FindEntity(sysParam);
            Assert.IsTrue(CompareEntities(expected, actual), "查询单一实体操作失败!");
        }
        /// <summary>
        ///A test for GetAllSysParams
        ///</summary>
        //[TestMethod()]
        //public void GetAllSysParamsTest()
        //{
        //    string paramName = string.Empty; // TODO: Initialize to an appropriate value
        //    string paramDsc = string.Empty; // TODO: Initialize to an appropriate value
        //    List<SysParam> expected = null; // TODO: Initialize to an appropriate value
        //    List<SysParam> actual;
        //    actual = SysParamBLL.GetAllSysParams(paramName, paramDsc);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}
        /// <summary>
        ///A test for SysParamDel
        ///</summary>
        [TestMethod]
        public void EntityDelTest()
        {


            SysParam sysParam = AddNewEntity();

            var result = DeleteEntity(sysParam) > 0;
            Assert.IsTrue(result, "删除实体操作执行失败!");

            var queryResult = FindEntity(sysParam);
            Assert.IsTrue(queryResult == null,
                string.Format("数据库中的记录没有被删掉,Exp1={0}"
                , queryResult == null));
            _gc.Remove(sysParam);
        }
        [TestMethod]
        public void EntityUpdateTest()
        {
            SysParam sysParam = AddNewEntity();
            SetEntityFields(sysParam);
            _repository.Modify(sysParam);
            var result = _repository.SaveChanges() > 0;
            Assert.IsTrue(result, "实体更新失败");
            ValidateEntity(sysParam);
        }

        #endregion FunctionalityTest

        #region PerformanceTest

        [TestMethod]
        public void BatchAddTest()
        {
            CodeTimer.Time(string.Format("BatchCreateTest for {0} Actions", BATCHCOUNT), BATCHCOUNT, this.AddNewEntity_Batch);
            ValidateEntitys(this._gc);
        }
        /// <summary>
        /// 为批量新增封装现有方法
        /// </summary>
        private void AddNewEntity_Batch()
        {
            this.AddNewEntity();
        }
        [TestMethod]
        public void BatchRetrieveTest()
        {
            var sysParam = this.AddNewEntity();
            var entityFound = FindEntity(sysParam);
            this._gc.Clear();
            this._gc.Add(entityFound);
            CodeTimer.Time(string.Format("BatchRetrieveTest for {0} Actions", BATCHCOUNT), BATCHCOUNT, RetrieveEntity_Batch);
        }
        /// <summary>
        /// 批量查询
        /// </summary>
        private void RetrieveEntity_Batch()
        {
            var entity = this._gc.FirstOrDefault();
            var result = _repository.GetFilteredElements(s => s.sysparamid == entity.sysparamid).FirstOrDefault();
            Assert.IsNotNull(result, "按照ID查询实体,返回Null");

            CompareEntities(entity, result);
        }
        [TestMethod]
        public void BatchUpdateEnityTest()
        {
            var sysParam = this.AddNewEntity();
            CodeTimer.Time(string.Format("BatchUpdateEnityTest for {0} Actions", BATCHCOUNT), BATCHCOUNT, UpdateEnity_Batch);
            //repository.Remove(sysParam);
            //repository.SaveChanges();
        }
        /// <summary>
        /// 批量查询
        /// </summary>
        private void UpdateEnity_Batch()
        {
            var entity = this._gc.FirstOrDefault();
            SetEntityFields(entity);
            _repository.Modify(entity);
            var result = _repository.SaveChanges() > 0;
            Assert.IsTrue(result, "实体更新失败");
        }
        [TestMethod]
        public void BatchDelEntityTest()
        {
            //数据准备
            for (var i = 0; i < BATCHCOUNT; i++)
            {
                AddNewEntity();
            }
            CodeTimer.Time(string.Format("BatchDelTest for {0} Actions", BATCHCOUNT), BATCHCOUNT, DelEntity_Batch);
        }
        private void DelEntity_Batch()
        {
            if (this._gc.Count > 0)
            {
                var entity = this._gc.LastOrDefault();
                _gc.Remove(entity);
                _repository.Remove(entity);
                var isDel = _repository.SaveChanges() > 0;
                Assert.IsTrue(isDel, "删除实体操作执行失败!");
            }
        }
        #endregion

        #region TransactionTest

        /// <summary>
        /// 没有想到更好的方式跟踪两次提交操作使用同一个Trans,
        /// 需要使用IntelliTrace或者Profiler跟踪
        /// 看来IntelliTrace不太方便跟踪事物信息,在用Profiler时不能用ITtrace,执行的SQL会有影响
        /// 结论:是两条插入语句确实是在同一个Trans中
        /// </summary>
        [TestMethod]
        public void TransactionTest_Add2EntitiesSucceed()
        {
            SysParam sysParam = null;
            //Create a transaction context for this operation

            using (TransactionScope scope = CreateTrans(TransactionScopeOption.Required))
            {
                sysParam = AddNewEntity();
                sysParam = AddNewEntity();
                scope.Complete();
            }
            ValidateEntity(sysParam);
        }

        /// <summary>
        /// 事务失败测试,
        /// 结论:在事务作用域内发生异常,所做的修改会回滚
        /// </summary>
        [TestMethod]
        public void TransactionTest_Fail_RollBack()
        {
            SysParam SucceedEntity = AddNewEntity();
            SysParam failedEntity = null;
            //Create a transaction context for this operation

            try
            {
                using (TransactionScope scope = CreateTrans(TransactionScopeOption.Required))
                {
                    failedEntity = AddNewEntity();
                    throw new Exception("强制终止事务");
                }
            }
            catch
            {
                //吃掉异常
                this._gc.Remove(failedEntity);
            }
            var foundEntity = FindEntityById(SucceedEntity.sysparamid);
            Assert.IsNotNull(foundEntity, "事务边界外创建的实体没有提交成功");
            foundEntity = FindEntityById(failedEntity.sysparamid);
            Assert.IsNull(foundEntity, "事务回滚失败");
        }

        /// <summary>
        /// 事务失败测试,
        /// 结论:在事务作用域内发生异常,所做的修改会回滚
        /// </summary>
        [TestMethod]
        public void TransactionTest_Fail_DoNotRollBack()
        {
            SysParam SucceedEntity = AddNewEntity();
            SysParam failedEntity = null;
            //Create a transaction context for this operation

            try
            {
                using (TransactionScope scope = CreateTrans(TransactionScopeOption.Required))
                {
                    failedEntity = AddNewEntity();
                    scope.Complete();
                    throw new Exception("强制终止事务");
                }
            }
            catch
            {
                //吃掉异常
                //this.GC.Remove(failedEntity);
            }
            var foundEntity = FindEntityById(SucceedEntity.sysparamid);
            Assert.IsNotNull(foundEntity, "事务边界外创建的实体没有提交成功");
            foundEntity = FindEntityById(failedEntity.sysparamid);
            Assert.IsNotNull(foundEntity, "事务边界内的实体提交后发生异常,提交失败");
        }
        /// <summary>
        /// 验证内层事务继承了外层事务
        /// 经跟踪发现,确实在同一个事务中
        /// </summary>
        [TestMethod]
        public void NestingTransTest_RsRs()
        {
            SysParam outerEntity = null;
            SysParam innerEntity = null;
            using (TransactionScope outerScope = CreateTrans(TransactionScopeOption.Required))
            {
                outerEntity = AddNewEntity();
                //如果不执行Complete,超出事务边界后会自动回滚
                outerScope.Complete();
                using (TransactionScope innerScope = CreateTrans(TransactionScopeOption.Required))
                {
                    innerEntity = AddNewEntity();
                    innerScope.Complete();
                }
            }
            var foundEntity = FindEntityById(outerEntity.sysparamid);
            Assert.IsNotNull(foundEntity, "外层事务创建的实体没有提交成功");
            foundEntity = FindEntityById(innerEntity.sysparamid);
            Assert.IsNotNull(foundEntity, "内层事务创建的实体没有提交成功");
        }
        /// <summary>
        /// 验证内层事务失败后需要回滚外层事务
        /// 经跟踪发现,外层事务正常回滚
        /// </summary>
        [TestMethod]
        public void NestingTransTest_RsRf()
        {
            SysParam outerEntity = null;
            SysParam innerEntity = null;
            try
            {
                using (TransactionScope outerScope = CreateTrans(TransactionScopeOption.Required))
                {
                    outerEntity = AddNewEntity();
                    //如果不执行Complete,超出事务边界后会自动回滚
                    outerScope.Complete();
                    using (TransactionScope innerScope = CreateTrans(TransactionScopeOption.Required))
                    {
                        innerEntity = AddNewEntity();
                        throw new Exception("强制终止事务");
                    }
                }
            }
            catch
            {
                this._gc.Clear();
            }
            var foundEntity = FindEntityById(outerEntity.sysparamid);
            Assert.IsNull(foundEntity, "外层事务创建的实体没有回滚");
            foundEntity = FindEntityById(innerEntity.sysparamid);
            Assert.IsNull(foundEntity, "内层事务创建的实体没有回滚");
        }
        /// <summary>
        /// 验证外层事务失败后需要回滚内层事务
        /// 经跟踪发现,外层事务正常回滚
        /// </summary>
        [TestMethod]
        public void NestingTransTest_RfRs()
        {
            SysParam outerEntity = null;
            SysParam innerEntity = null;
            try
            {
                using (TransactionScope outerScope = CreateTrans(TransactionScopeOption.Required))
                {

                    using (TransactionScope innerScope = CreateTrans(TransactionScopeOption.Required))
                    {
                        innerEntity = AddNewEntity();
                        innerScope.Complete();

                    }
                    outerEntity = AddNewEntity();
                    //如果不执行Complete,超出事务边界后会自动回滚
                    throw new Exception("强制终止事务");
                }
            }
            catch
            {
                this._gc.Clear();
            }
            var foundEntity = FindEntityById(outerEntity.sysparamid);
            Assert.IsNull(foundEntity, "外层事务创建的实体没有回滚");
            foundEntity = FindEntityById(innerEntity.sysparamid);
            Assert.IsNull(foundEntity, "内层事务创建的实体没有回滚");
        }

        /// <summary>
        /// 内层事务使用RequireNew
        /// 经跟踪发现,两次新增操作确实实在两个事务中执行的
        /// </summary>
        [TestMethod]
        public void NestingTransTest_RsRNs()
        {
            SysParam outerEntity = null;
            SysParam innerEntity = null;
            try
            {
                using (TransactionScope outerScope = CreateTrans(TransactionScopeOption.Required))
                {
                    outerEntity = AddNewEntity();
                    //如果不执行Complete,超出事务边界后会自动回滚
                    //真正的数据库事务提交是在finally中执行的,
                    //执行Complete方法并没有真正提交,只是通知Scope可以做提交
                    outerScope.Complete();
                    using (TransactionScope innerScope = CreateTrans(TransactionScopeOption.RequiresNew))
                    {
                        innerEntity = AddNewEntity();
                        innerScope.Complete();
                    }
                }
            }
            catch
            {
                this._gc.Clear();
            }
            var foundEntity = FindEntityById(outerEntity.sysparamid);
            Assert.IsNotNull(foundEntity, "外层事务创建的实体没有提交成功");
            foundEntity = FindEntityById(innerEntity.sysparamid);
            Assert.IsNotNull(foundEntity, "内层事务创建的实体没有提交成功");
        }

        /// <summary>
        /// 验证内层事务失败后需要回滚外层事务
        /// 经跟踪发现,外层事务没有被
        /// </summary>
        [TestMethod]
        public void NestingTransTest_RsRNf()
        {
            SysParam outerEntity = null;
            SysParam innerEntity = null;
            try
            {
                using (TransactionScope outerScope = CreateTrans(TransactionScopeOption.Required))
                {
                    outerEntity = AddNewEntity();
                    //如果不执行Complete,超出事务边界后会自动回滚
                    outerScope.Complete();
                    using (TransactionScope innerScope = CreateTrans(TransactionScopeOption.RequiresNew))
                    {
                        innerEntity = AddNewEntity();
                        throw new Exception("强制终止事务");
                    }
                }
            }
            catch
            {
                this._gc.Clear();
            }
            var foundEntity = FindEntityById(outerEntity.sysparamid);
            Assert.IsNotNull(foundEntity, "外层事务创建的实体不应被回滚");
            foundEntity = FindEntityById(innerEntity.sysparamid);
            Assert.IsNull(foundEntity, "内层事务创建的实体没有回滚");
        }

        /// <summary>
        /// 验证外层事务失败后不需要回滚内层事务
        /// 经跟踪发现,外层事务正常回滚,内层事务没有回滚
        /// </summary>
        [TestMethod]
        public void NestingTransTest_RfRNs()
        {
            SysParam outerEntity = null;
            SysParam innerEntity = null;
            try
            {
                using (TransactionScope outerScope = CreateTrans(TransactionScopeOption.Required))
                {

                    using (TransactionScope innerScope = CreateTrans(TransactionScopeOption.RequiresNew))
                    {
                        innerEntity = AddNewEntity();
                        innerScope.Complete();

                    }
                    outerEntity = AddNewEntity();
                    //如果不执行Complete,超出事务边界后会自动回滚
                    throw new Exception("强制终止事务");
                }
            }
            catch
            {
                this._gc.Clear();
            }
            var foundEntity = FindEntityById(outerEntity.sysparamid);
            Assert.IsNull(foundEntity, "外层事务创建的实体没有回滚");
            foundEntity = FindEntityById(innerEntity.sysparamid);
            Assert.IsNotNull(foundEntity, "内层事务创建的实体不应回滚");
        }


        /// <summary>
        /// 内层事务使用Suppress    
        /// </summary>
        [TestMethod]
        public void NestingTransTest_RsSs()
        {
            SysParam outerEntity = null;
            SysParam innerEntity = null;
            try
            {
                using (TransactionScope outerScope = CreateTrans(TransactionScopeOption.Required))
                {
                    outerEntity = AddNewEntity();
                    //如果不执行Complete,超出事务边界后会自动回滚
                    //真正的数据库事务提交是在finally中执行的,
                    //执行Complete方法并没有真正提交,只是通知Scope可以做提交
                    outerScope.Complete();
                    //Suppress方式还是创建了数据库事务
                    using (TransactionScope innerScope = CreateTrans(TransactionScopeOption.Suppress))
                    {
                        innerEntity = AddNewEntity();
                        innerScope.Complete();
                    }
                }
            }
            catch
            {
                this._gc.Clear();
            }
            var foundEntity = FindEntityById(outerEntity.sysparamid);
            Assert.IsNotNull(foundEntity, "外层事务创建的实体没有提交成功");
            foundEntity = FindEntityById(innerEntity.sysparamid);
            Assert.IsNotNull(foundEntity, "内层事务创建的实体没有提交成功");
        }

        /// <summary>
        /// 验证内层事务失败后需要回滚外层事务
        /// 经跟踪发现,外层事务没有被
        /// </summary>
        [TestMethod]
        public void NestingTransTest_RsSf()
        {
            SysParam outerEntity = null;
            SysParam innerEntity = null;
            try
            {
                using (TransactionScope outerScope = CreateTrans(TransactionScopeOption.Required))
                {
                    outerEntity = AddNewEntity();
                    //如果不执行Complete,超出事务边界后会自动回滚
                    //一旦执行了Complete就不能再回滚了
                    //事务的提交不能与ObjectContext一致,否则将失去分步提交的特性,
                    //也就是事务的边界范围内要包含ObjectContext的持久化边界
                    //不应该在实现内部调用Complete
                    outerScope.Complete();
                    //throw new Exception("强制终止事务");
                    using (TransactionScope innerScope = CreateTrans(TransactionScopeOption.Suppress))
                    {
                        //Suppress方式,不用显示调用Complete,如果没有异常,自动提交
                        innerEntity = AddNewEntity();
                        throw new Exception("强制终止事务");
                    }
                }
            }
            catch
            {
                this._gc.Clear();
            }
            var foundEntity = FindEntityById(outerEntity.sysparamid);
            Assert.IsNotNull(foundEntity, "外层事务创建的实体不应被回滚");
            foundEntity = FindEntityById(innerEntity.sysparamid);
            Assert.IsNotNull(foundEntity, "内层事务创建的实体不应被回滚");
        }

        #endregion


        #region ConcurrencyTest
        [TestMethod]
        public void ConcurrencyTest()
        {
            var entity = CreateEntity();
            this._repository.Add(entity);
            this._repository.SaveChanges();
            Console.WriteLine(entity.SysVersion);

            SetEntityFields(entity);
            entity.SysVersion++;
            this._repository.SaveChanges();
            Console.WriteLine(entity.SysVersion);


            SetEntityFields(entity);
            entity.SysVersion--;
            this._repository.SaveChanges();
            Console.WriteLine(entity.SysVersion);
        }
        #endregion

      
    }
}
