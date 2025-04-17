using ShortageAPP.Models;
using ShortageAPP.Services;

namespace ShortageAppUnitTest;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        if (File.Exists("users.json"))
        {
            File.Delete("users.json");
        }

        if (File.Exists("shortages.json"))
        {
            File.Delete("shortages.json");
        }

    }

    [Test]
    public void RegisterUser_ShouldNotAddUser_WhenUserAlreadyExists()
    {
        const string name = "TestUser";
        const string password = "TestPassword";
        const UserRole role = UserRole.User;

        UserService.RegisterUser(name, password, role);

        bool result = UserService.RegisterUser(name, password, role);

        Assert.IsFalse(result, "User should not be registered again.");
    }

    [Test]
    public void LoginUser_ShouldReturnUser_WhenValidCredentialsAreProvided()
    {
        const string name = "TestUser";
        const string password = "TestPassword";
        const UserRole role = UserRole.User;

        UserService.RegisterUser(name, password, role);

        var user = UserService.LoginUser(name, password);

        Assert.IsNotNull(user, "User should be logged in successfully.");
    }

    [Test]
    public void LoginUser_ShouldReturnNull_WhenInvalidCredentialsAreProvided()
    {
        const string name = "TestUser";
        const string password = "TestPassword";
        const UserRole role = UserRole.User;

        UserService.RegisterUser(name, password, role);

        var user = UserService.LoginUser(name, "WrongPassword");

        Assert.IsNull(user, "User should not be logged in with invalid credentials.");
    }

    [Test]
    public void RegisterShortage_ShouldRegisterUniqueShortage_WhenValidDataIsProvided()
    {
        var shortage = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage",
            Title = "TestTitle",
            Priority = 1,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.MeetingRoom,
            CategoryType = Category.Electronics
        };

        bool result = ShortageService.RegisterShortage(shortage);

        Assert.IsTrue(result, "Shortage should be registered successfully.");
    }

    [Test]
    public void RegisterShortage_ShouldNotRegisterShortage_WhenGivenNameAndTitleIsNotUnique()
    {
        var shortage = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage",
            Title = "TestTitle",
            Priority = 1,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.MeetingRoom,
            CategoryType = Category.Electronics
        };


        ShortageService.RegisterShortage(shortage);

        bool result2 = ShortageService.RegisterShortage(shortage);

        Assert.IsFalse(result2, "Shortage should not be registered again with the same name and title.");
    }

    [Test]
    public void RegisterShortage_ShouldOverrideExistingShortage_WhenHigherPriorityIsProvided()
    {
        var shortage1 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage",
            Title = "TestTitle",
            Priority = 1,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.MeetingRoom,
            CategoryType = Category.Electronics
        };

        var shortage2 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage",
            Title = "TestTitle",
            Priority = 2,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.MeetingRoom,
            CategoryType = Category.Electronics
        };

        ShortageService.RegisterShortage(shortage1);

        bool result2 = ShortageService.RegisterShortage(shortage2);

        Assert.IsTrue(result2, "Shortage should be registered successfully with higher priority.");
    }

    [Test]
    public void DeleteShortage_ShouldDeleteShortage_WhenValidTitleAndNameAreProvided()
    {
        var shortage = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage",
            Title = "TestTitle",
            Priority = 1,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.MeetingRoom,
            CategoryType = Category.Electronics
        };
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "TestUser",
            Password = "TestPassword",
            Role = UserRole.Admin
        };

        ShortageService.RegisterShortage(shortage);

        ShortageService.DeleteShortage("TestTitle", "TestShortage", user);

        var result = StorageService<Shortage>.ReadStorage("shortages.json").ToList();

        Assert.IsEmpty(result, "Shortage should be deleted successfully.");
    }

    [Test]
    public void DeleteShortage_ShouldNotDeleteShortage_WhenInvalidTitleAndNameIsGiven()
    {
        var shortage = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage",
            Title = "TestTitle",
            Priority = 1,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.MeetingRoom,
            CategoryType = Category.Electronics
        };
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "TestUser",
            Password = "TestPassword",
            Role = UserRole.User
        };

        ShortageService.RegisterShortage(shortage);
        ShortageService.DeleteShortage("InvalidTitle", "InvalidShortage", user);
        var result = StorageService<Shortage>.ReadStorage("shortages.json").ToList();

        Assert.IsNotEmpty(result, "Shortage should not be deleted with invalid title and name.");
    }

    [Test]
    public void DeleteShortage_ShouldNotDeleteShortage_WhenUserIsDeletingNotHisOwnShortage()
    {
        var shortage = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage",
            Title = "TestTitle",
            Priority = 1,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.MeetingRoom,
            CategoryType = Category.Electronics
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "TestUser",
            Password = "TestPassword",
            Role = UserRole.User
        };

        ShortageService.RegisterShortage(shortage);
        ShortageService.DeleteShortage("TestTitle", "TestShortage", user);
        Assert.IsNotEmpty(StorageService<Shortage>.ReadStorage("shortages.json").ToList(), "Shortage should not be deleted by another user.");
        
    }

    [Test]
    public void GetShortagesByRole_ShouldReturnAllShortages_WhenUserIsAdmin()
    {
        var adminUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "AdminUser",
            Password = "AdminPassword",
            Role = UserRole.Admin
        };

        var shortage1 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage1",
            Title = "TestTitle1",
            Priority = 1,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.MeetingRoom,
            CategoryType = Category.Electronics
        };

        var shortage2 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage2",
            Title = "TestTitle2",
            Priority = 2,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.Kitchen,
            CategoryType = Category.Food
        };

        ShortageService.RegisterShortage(shortage1);
        ShortageService.RegisterShortage(shortage2);

        var result = ShortageService.GetShortagesByRole(adminUser);

        Assert.AreEqual(2, result.Count, "Admin should see all shortages.");
    }
    
    [Test]
    public void FilterByTitle_ShouldReturnFilteredShortages_WhenValidTitleIsProvided()
    {
        var shortage1 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage1",
            Title = "TestTitle1",
            Priority = 1,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.MeetingRoom,
            CategoryType = Category.Electronics
        };

        var shortage2 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage2",
            Title = "TestTitle1",
            Priority = 2,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.Kitchen,
            CategoryType = Category.Food
        };
        
        var shortage3 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage3",
            Title = "TestTitle2",
            Priority = 2,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.Kitchen,
            CategoryType = Category.Food
        };

        ShortageService.RegisterShortage(shortage1);
        ShortageService.RegisterShortage(shortage2);
        ShortageService.RegisterShortage(shortage3);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "TestUser",
            Password = "TestPassword",
            Role = UserRole.Admin
        };

        var result = FilterService.FilterByTitle(user, "TestTitle1");
        
        Assert.AreEqual(2, result.Count(), "Filter should return shortages with the specified title.");
    }
    
    [Test]
    public void FilterByDate_ShouldReturnFilteredShortages_WhenValidDateRangeIsProvided()
    {
        var shortage1 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage1",
            Title = "TestTitle1",
            Priority = 1,
            CreatedAt = DateTime.Parse("2025-01-01"),
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.MeetingRoom,
            CategoryType = Category.Electronics
        };

        var shortage2 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage2",
            Title = "TestTitle2",
            Priority = 2,
            CreatedAt = DateTime.Parse("2025-01-02"),
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.Kitchen,
            CategoryType = Category.Food
        };
        
        var shortage3 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage3",
            Title = "TestTitle3",
            Priority = 2,
            CreatedAt = DateTime.Parse("2025-01-05"),
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.Kitchen,
            CategoryType = Category.Food
        };

        var shortage4 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage4",
            Title = "TestTitle4",
            Priority = 2,
            CreatedAt = DateTime.Parse("2024-01-01"),
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.Kitchen,
            CategoryType = Category.Food
        };

        ShortageService.RegisterShortage(shortage1);
        ShortageService.RegisterShortage(shortage2);
        ShortageService.RegisterShortage(shortage3);
        ShortageService.RegisterShortage(shortage4);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "TestUser",
            Password = "TestPassword",
            Role = UserRole.Admin
        };

        var result = FilterService.FilterByDate(user,"2025-01-01", "2025-01-02" );
        
        Assert.AreEqual(2, result.Count(), "Filter should return shortages within the specified date range.");
    }
    
    [Test]
    public void FilterByCategory_ShouldReturnFilteredShortages_WhenValidCategoryIsProvided()
    {
        var shortage1 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage1",
            Title = "TestTitle1",
            Priority = 1,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.MeetingRoom,
            CategoryType = Category.Electronics
        };

        var shortage2 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage2",
            Title = "TestTitle2",
            Priority = 2,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.Kitchen,
            CategoryType = Category.Food
        };
        
        var shortage3 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage3",
            Title = "TestTitle3",
            Priority = 2,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.Kitchen,
            CategoryType = Category.Food
        };

        ShortageService.RegisterShortage(shortage1);
        ShortageService.RegisterShortage(shortage2);
        ShortageService.RegisterShortage(shortage3);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "TestUser",
            Password = "TestPassword",
            Role = UserRole.Admin
        };

        var result = FilterService.FilterByCategory(user, "Food");
        
        Assert.AreEqual(2, result.Count(), "Filter should return shortages with the specified category.");
    }
    
    [Test]
    public void FilterByRoom_ShouldReturnFilteredShortages_WhenValidRoomIsProvided()
    {
        var shortage1 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage1",
            Title = "TestTitle1",
            Priority = 1,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.MeetingRoom,
            CategoryType = Category.Electronics
        };

        var shortage2 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage2",
            Title = "TestTitle2",
            Priority = 2,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.Kitchen,
            CategoryType = Category.Food
        };
        
        var shortage3 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "TestShortage3",
            Title = "TestTitle3",
            Priority = 2,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.Kitchen,
            CategoryType = Category.Food
        };

        ShortageService.RegisterShortage(shortage1);
        ShortageService.RegisterShortage(shortage2);
        ShortageService.RegisterShortage(shortage3);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "TestUser",
            Password = "TestPassword",
            Role = UserRole.Admin
        };

        var result = FilterService.FilterByRoom(user, "Kitchen");
        
        Assert.AreEqual(2, result.Count(), "Filter should return shortages with the specified room.");
    }

    [Test]
    public void Shortages_ShouldBeOrderedDescendingByPriority_Always()
    {
        //Tested as admin user
        var shortage1 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "1",
            Title = "1",
            Priority = 1,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.Kitchen,
            CategoryType = Category.Food
        };
        var shortage2 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "2",
            Title = "2",
            Priority = 5,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.Kitchen,
            CategoryType = Category.Food
        };
        var shortage3 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "3",
            Title = "3",
            Priority = 3,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.Kitchen,
            CategoryType = Category.Food
        };
        var shortage4 = new Shortage
        {
            Id = Guid.NewGuid(),
            Name = "4",
            Title = "4",
            Priority = 10,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.NewGuid(),
            RoomType = Room.Kitchen,
            CategoryType = Category.Food
        };
        
        ShortageService.RegisterShortage(shortage1);
        ShortageService.RegisterShortage(shortage2);
        ShortageService.RegisterShortage(shortage3);
        ShortageService.RegisterShortage(shortage4);
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "TestUser",
            Password = "TestPassword",
            Role = UserRole.Admin
        };
        var result = ShortageService.GetShortagesByRole(user);
        var orderedResult = result.OrderByDescending(s => s.Priority).ToList();
        Assert.AreEqual(result, orderedResult, "Shortages should be ordered by priority in descending order.");
    }
}