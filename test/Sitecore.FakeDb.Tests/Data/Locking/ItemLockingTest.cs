﻿namespace Sitecore.FakeDb.Tests.Data.Locking
{
    using FluentAssertions;
    using Sitecore.Security.Accounts;
    using Xunit;

    [Trait("Category", "RequireLicense")]
    public class ItemLockingTest
    {
        [Fact]
        public void ShouldLockItem()
        {
            // arrange
            using (var db = new Db {new DbItem("home")})
            {
                var item = db.GetItem("/sitecore/content/home");

                // act
                item.Locking.Lock();

                // assert
                item.Locking.CanLock().Should().BeFalse();
                item.Locking.CanUnlock().Should().BeTrue();
                item.Locking.GetOwner().Should().Be(@"default\Anonymous");
                item.Locking.HasLock().Should().BeTrue();
                item.Locking.IsLocked().Should().BeTrue();
            }
        }

        [Fact]
        public void ShouldLockItemByAnotherUser()
        {
            // arrange
            using (var db = new Db {new DbItem("home")})
            {
                var item = db.GetItem("/sitecore/content/home");

                // act
                using (new UserSwitcher(@"extranet\John", false))
                {
                    item.Locking.Lock();
                }

                // assert
                item.Locking.CanLock().Should().BeFalse("CanLock()");
                item.Locking.GetOwner().Should().Be(@"extranet\John");
                item.Locking.HasLock().Should().BeFalse("HasLock()");
                item.Locking.IsLocked().Should().BeTrue("IsLocked()");
            }
        }

        [Fact]
        public void ShouldUnlockLockedItem()
        {
            // arrange
            using (var db = new Db {new DbItem("home")})
            {
                var item = db.GetItem("/sitecore/content/home");
                item.Locking.Lock();

                // act
                item.Locking.Unlock();

                // assert
                item.Locking.IsLocked().Should().BeFalse();
            }
        }

        [Fact]
        public void ShouldBeAbleToUnlockItem()
        {
            // arrange
            using (var db = new Db {new DbItem("home")})
            {
                var item = db.GetItem("/sitecore/content/home");
                item.Locking.Lock();

                // act & assert
                item.Locking.CanUnlock().Should().BeTrue();
            }
        }
    }
}