using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Moq;
using NET.Repository.Tests.Fakes;

namespace NET.Repository.Tests.Utility
{
    public static class UnifiedMethods
    {
        public static Mock<DbSet<T>> GetQueryableMockDbSet<T>(IEnumerable<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return dbSet;
        }

        public static CartoonContext GetDefaultCartoonContext() {
            var options = new DbContextOptionsBuilder<CartoonContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new CartoonContext(options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}