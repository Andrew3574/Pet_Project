using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Models;
using Repositories;
namespace UnitTests
{
    public class EventTests
    {
        private readonly EventsDbContext _dbContext;

        private readonly EventsRepository _eventsRepository; 
        private readonly GuestsRepository _guestsRepository;
        private readonly SharedEventsGuestsRepository _sharedEventsGuestRepository;

        private Category _category;
        private Event _event;
        private Guest _guest;

        public EventTests()
        {
            var options = new DbContextOptionsBuilder<EventsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new EventsDbContext(options);
            _eventsRepository = new EventsRepository(_dbContext);
            _guestsRepository = new GuestsRepository(_dbContext);
            _sharedEventsGuestRepository = new SharedEventsGuestsRepository(_dbContext);

            _guest = new Guest { Name = "bob", Surname = "miley", Email = "anderson64217@gmail.com",BirthDate=DateOnly.Parse("2000-10-10"),Role_id=1 };
            _category = new Category { Name = "TestCategory" };
            _event = new Event { Name = "TestEvent", EventDate = DateTime.Now, Location = "TestLocation", Category = _category, GuestLimit = 10 };
        }

        [Fact]
        public async Task Create()
        {
            var @eventByName = await _eventsRepository.GetByName("TestEvent");
            Assert.Empty(@eventByName);

            await _eventsRepository.Create(_event);

            @eventByName = await _eventsRepository.GetByName("TestEvent");
            Assert.NotEmpty(@eventByName);
        }
        [Fact]
        public async Task GetAll()
        {
            await _eventsRepository.Create(_event);

            await _eventsRepository.Create(new Event { Name = "TestEvent2", EventDate = DateTime.Now, Location = "TestLocation2", Category = _category, GuestLimit = 10 });

            var events = await _eventsRepository.GetAll();
            Assert.Equal(2, events.Count());
        }
        [Fact]
        public async Task Update()
        {
            await _eventsRepository.Create(_event);

            _event.Location = "TestLocation2222";
            await _eventsRepository.Update(_event);

            var @event = await _eventsRepository.GetByName("TestEvent");

            Assert.Equal("TestLocation2222", @event.First().Location);
        }

        [Fact]
        public async Task Delete()
        {
            await _eventsRepository.Create(_event);
            Assert.NotNull(await _eventsRepository.GetById(_event.EventId));

            await _eventsRepository.Delete(_event);
            Assert.Null(await _eventsRepository.GetById(_event.EventId));

        }

        [Fact]
        public async Task AddGuestToEvent()
        {
            await _eventsRepository.Create(_event);
            await _guestsRepository.Create(_guest);

            var entity = new SharedEventsGuest { Event = _event, Guest = _guest };

            await _sharedEventsGuestRepository.Create(entity);

            Assert.Equal(9,_event.GuestLimit);

        }

        [Fact]
        public async Task DeleteGuestFromEvent()
        {
            await _eventsRepository.Create(_event);
            await _guestsRepository.Create(_guest);

            var entity = new SharedEventsGuest { Event = _event, Guest = _guest };

            await _sharedEventsGuestRepository.Create(entity);

            Assert.Equal(9, _event.GuestLimit);

            await _sharedEventsGuestRepository.Delete(entity);

            Assert.Equal(10, _event.GuestLimit);

        }

        [Fact]
        public async Task GetEventsByGuest()
        {
            Event @event1 = new Event { Name = "TestEvent222222", EventDate = DateTime.Now, Location = "TestLocation222222", Category = _category, GuestLimit = 1 };
            Event @event2 = new Event { Name = "dontShowInTest", EventDate = DateTime.Now, Location = "TestLocation222222", Category = _category, GuestLimit = 1 };

            await _eventsRepository.Create(_event);
            await _eventsRepository.Create(@event1);
            await _eventsRepository.Create(@event2);

            await _guestsRepository.Create(_guest);

            var entity1 = new SharedEventsGuest { Event = _event, Guest = _guest };
            var entity2 = new SharedEventsGuest { Event = @event1, Guest = _guest };

            await _sharedEventsGuestRepository.Create(entity1);
            await _sharedEventsGuestRepository.Create(entity2);

            var result = await _sharedEventsGuestRepository.GetByGuest(_guest);
            Assert.Equal(2,result.Count);
            Assert.Equal("TestEvent", result[0].Event.Name);
            Assert.Equal("TestEvent222222", result[1].Event.Name);
        }

    }
}