using ExchangeRateUpdater.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace ExchangeRateUpdater.Domain.UnitTests.Entities
{
    public abstract class EntityTestBase<TEntity, TId> where TEntity: EntityBase<TId>
    {
        protected abstract TEntity CreateEntity(TId id);
        protected abstract TId CreateId();

        [Test]
        public void GetHashCode_ShouldReturnIdHashCode()
        {
            var id = CreateId();
            var entity = CreateEntity(id);
            entity.GetHashCode().Should().Be(id!.GetHashCode());
        }

        [Test]
        public void Equals_ShouldReturnTrue_WhenEntitiesHaveSameReference()
        {
            var id = CreateId();
            var entity = CreateEntity(id);
            var entity2 = entity;
            entity.Equals(entity2).Should().BeTrue();
        }

        [Test]
        public void Equals_ShouldReturnTrue_WhenIdsAreEqual()
        {
            var id = CreateId();
            var entity = CreateEntity(id);
            var entity2 = CreateEntity(id);
            entity.Equals(entity2).Should().BeTrue();
        }
        
        [Test]
        public void Equals_ShouldReturnFalse_WhenSecondObjectIsNull()
        {
            var id = CreateId();
            var entity = CreateEntity(id);
            entity.Equals(null).Should().BeFalse();
        }
        
        [Test]
        public void Equals_ShouldReturnFalse_WhenSecondObjectIsOfDifferentType()
        {
            var id = CreateId();
            var entity = CreateEntity(id);
            entity.Equals(new object()).Should().BeFalse();
        }
    }
}