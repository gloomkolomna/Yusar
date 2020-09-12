using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Yusar.Core;
using Yusar.Core.Entities;

namespace Yusar.Tests
{
    [TestFixture]
    public class RepositoryTests
    {
        private IYusarRepository<SimpleString> _repository;
        private Random _rnd;

        [SetUp]
        public void Setup()
        {
            _repository = new YusarRepository<SimpleString>();
            _rnd = new Random();
        }

        [Test, Order(0)]
        public void CreateTest()
        {
            var simpleItem = new SimpleString { Str = $"simple string {_rnd.Next(0, 10)}" };
            var result = _repository.Create(simpleItem);
            Assert.IsTrue(result);
        }

        [Test, Order(2)]
        public void DeleteTest()
        {
            var deleteResult = _repository.Delete(1);
            Assert.IsTrue(deleteResult);
        }

        [Test, Order(1)]
        public void UpdateTest()
        {
            var simpleItem = new SimpleString { Str = $"simple string update {_rnd.Next(0, 10)}", Id = 2 };
            var updateResult = _repository.Update(simpleItem);
            Assert.IsTrue(updateResult);
        }

        [Test, Order(3)]
        public void GetAllTest()
        {
            var items = _repository.GetAll();
            Assert.IsNotEmpty(items);
        }

        [Test, Order(4)]
        public void GetByIdTest()
        {
            var item = _repository.GetById(1);
            Assert.IsNotNull(item);
        }

        [Test, Order(5)]
        public async Task CreateAsyncTest()
        {
            var simpleItem = new SimpleString { Str = $"simple string {_rnd.Next(0, 10)}" };
            var result = await _repository.CreateAsync(simpleItem);
            Assert.IsTrue(result);
        }

        [Test, Order(7)]
        public async Task DeleteAsyncTest()
        {
            var deleteResult = await _repository.DeleteAsync(1);
            Assert.IsTrue(deleteResult);
        }

        [Test, Order(6)]
        public async Task UpdateAsyncTest()
        {
            var simpleItem = new SimpleString { Str = $"simple string update {_rnd.Next(0, 10)}", Id = 1 };
            var updateResult = await _repository.UpdateAsync(simpleItem);
            Assert.IsTrue(updateResult);
        }

        [Test, Order(8)]
        public async Task GetAllAsyncTest()
        {
            var items = await _repository.GetAllAsync();
            Assert.IsNotEmpty(items);
        }

        [Test, Order(9)]
        public async Task GetByIdAsyncTest()
        {
            var item = await _repository.GetByIdAsync(1);
            Assert.IsNotNull(item);
        }
    }
}