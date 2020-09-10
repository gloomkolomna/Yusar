using NUnit.Framework;
using System;
using Yusar.Core;
using Yusar.Core.Entities;

namespace Yusar.Tests
{
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

        [Test]
        public void CreateTest()
        {
            var simpleItem = new SimpleString { Str = $"simple string {_rnd.Next(0, 10)}" };
            _repository.Create(simpleItem);
        }

        [Test]
        public void DeleteTest()
        {
            var deleteResult = _repository.Delete(1);
            Assert.IsTrue(deleteResult);
        }

        [Test]
        public void UpdateTest()
        {
            var simpleItem = new SimpleString { Str = $"simple string update {_rnd.Next(0, 10)}", Id = 1 };
            var updateResult = _repository.Update(simpleItem);
            Assert.IsTrue(updateResult);
        }

        [Test]
        public void GetAllTest()
        {
            var items = _repository.GetAll();
            Assert.IsNotEmpty(items);
        }

        [Test]
        public void GetByIdTest()
        {
            var item = _repository.GetById(1);
            Assert.IsNotNull(item);
        }
    }
}