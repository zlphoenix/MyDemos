using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.TRF.Domain.Core;
using TelChina.TRF.Demo.DomainModel.Model;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq.Expressions;
using TelChina.TRF.Domain.Core.Extentions;
using System.Diagnostics.Contracts;
using TelChina.TRF.Domain.Core.Specification;

namespace TelChina.TRF.Demo.DomainModelTest
{
    [TestClass]
    public class RelationShipTest
    {
        #region Member

        private const int BATCHCOUNT = 1000;
        private TestContext testContextInstance;
        private const string NamePerfix = "Debug_";

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private IRepository<User> UserRepository;
        private IRepository<Role> RoleRepository;
        #endregion
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
        [TestInitialize()]
        public void MyTestInitialize()
        {
            ObjectContext edmContext = new DemoEntities();

            this.UserRepository = InitRepository<User>(edmContext, x => x.Name.StartsWith(NamePerfix));
            this.RoleRepository = InitRepository<Role>(edmContext, x => x.Name.StartsWith(NamePerfix));
            string[] userNames = { "张三", "李四", "王五" };
            string[] RoleNames = { "Admin", "User", "Guest" };
            foreach (string username in userNames)
            {
                User u = new User();
                u.Name = NamePerfix + username;
                u.Code = u.Name;
                UserRepository.Add(u);
            }
            UserRepository.SaveChanges();
            foreach (string rolename in RoleNames)
            {
                Role r = new Role();
                r.Name = NamePerfix + rolename;
                r.Code = r.Name;
                RoleRepository.Add(r);
            }
            RoleRepository.SaveChanges();
        }
        [TestCleanup()]
        public void MyTestCleanup()
        {
            //垃圾清理
            CleanupRubbish<User>(this.UserRepository);
            CleanupRubbish<Role>(this.RoleRepository);
        }

        /// <summary>
        /// 垃圾清理
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="repository"></param>
        private void CleanupRubbish<TEntity>(IRepository<TEntity> repository)
            where TEntity : EntityObject, IEntity
        {
            var entitys = repository.GetAll();
            if (entitys != null && entitys.Count() > 0)
            {
                foreach (var entity in entitys)
                {
                    repository.Remove(entity);
                }

                repository.SaveChanges();
            }
        }
        #endregion
        #region Infrastructure
        private IRepository<TEntity>
          InitRepository<TEntity>(ObjectContext edmContext, Expression<Func<TEntity, bool>> filter)
          where TEntity : EntityObject, IEntity
        {
            var result = EDMRepositoryContext.GetCurrentContext(edmContext).GetRepository<TEntity>();
            //清理垃圾数据
            var rubbish = result.GetFilteredElements(filter);
            if (rubbish != null && rubbish.Count() > 0)
            {
                foreach (var item in rubbish)
                    result.Remove(item);

                result.SaveChanges();
            }
            return result;
        }
        #endregion Infrastructure

        [TestMethod]
        public void GroupTest()
        {
            var result = (from r in RoleRepository.GetAll()
                          group r by new { Name = r.Name.ToLower().Contains('a'), Code = r.Code.Contains('u') }
                              into myGroup
                              select new { Title = myGroup.Key, myGroup }).ToList();
        }

        /// <summary>
        /// EdmTest
        /// </summary>
        [TestMethod]
        public void InheritanceTest()
        {
            DemoEntities edmContext = new DemoEntities();
            MO mo = new MO();
            mo.DocNo = "MO001";
            mo.Product = "PC";
            Pick p = new Pick();
            p.Code = "CPU";
            p.Name = p.Code;
            p.MO = mo;
            edmContext.OrderSet.AddObject(mo);
            edmContext.SaveChanges();
            var mos = edmContext.OrderSet
                     .OfType<MO>()
                     .Include("Pick").ToList();
            foreach (var m in mos)
            {
                Console.WriteLine(m.Pick.Count());
            }



        }
        /// <summary>
        /// 懒加载测试
        /// 由于官方建议不要使用Context options使用LazyLoad
        /// </summary>
        //[TestMethod]
        public void LazyLoadTest()
        {
            var user = UserRepository.GetFilteredElements(u => u.Name.Contains("张三")).FirstOrDefault();
            var role = RoleRepository.GetFilteredElements(u => u.Name.Contains("Admin")).FirstOrDefault();
            DemoEntities ctx = new DemoEntities();
            var ExcResult = ctx.ExecuteStoreCommand(string.Format("insert into UserRoleSet([UserId],[RoleId],[SysVersion])values({0},{1},0)", user.Id, role.Id));
            //UserRole relation = new UserRole();
            //relation.User = user;
            //relation.Role = role;
            //user.UserRole.Add(relation);
            //UserRepository.SaveChanges();

            var users = UserRepository.GetAll() as IQueryable<User>;
            user = (from u in users.Include("UserRole")
                    where u.UserRole.Count > 0
                    select u).FirstOrDefault();
            Assert.IsNotNull(user);
        }


        //public int Sum(int x, int y)
        //{
        //    Contract.Requires<ArgumentOutOfRangeException>(x > 0 && y > 0);
        //    return x + y;
        //}

        //[TestMethod]
        //public void ContractTest()
        //{
        //    Assert.AreEqual<int>(this.Sum(-1, -2), -3);
        //}


        //[TestMethod]
        //public void LinqDateTest()
        //{
        //    var r1 = UserRepository.GetAll().Where((x, y) => x.CreatedOn > DateTime.Now && y > 1);
        //    Assert.IsNotNull(r1.ToList());


        //    var result = UserRepository.GetBySpec(
        //        new DirectSpecification<User>(u => new DateTime(u.CreatedOn.Value.Year, u.CreatedOn.Value.Month, u.CreatedOn.Value.Day)
        //            > new DateTime(u.ModifiedOn.Value.Year, u.ModifiedOn.Value.Month, u.ModifiedOn.Value.Day)
        //            ));
        //    Assert.IsNotNull(result.ToList());

        //}
    }
}
