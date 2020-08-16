using System;
using System.Collections.Generic;
using System.Linq;
using BugTracker.Api.PgSqlDatabase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace BugTracker.Api.Controllers
{
    [ApiController]
    [Route("api/bugs")]
    public class BugsController
        : ControllerBase
    {
        private readonly DatabaseContext databaseContext;
        private readonly IClock clock;

        public BugsController(DatabaseContext databaseContext, IClock clock)
        {
            this.databaseContext = databaseContext;
            this.clock = clock;
        }

        [HttpGet]
        public List<BugDto> Get()
        {
            return this.databaseContext.Bug.Include(b => b.ActiveUser)
                .Where(b => b.Status != BugStatus.Closed)
                .AsEnumerable()
                .Select(b => new BugDto
                {
                    ActiveUserId = b.ActiveUser?.Id,
                    Slug = b.Id.ToString(),
                    Title = b.Title
                })
                .ToList();
        }

        [HttpPost]
        public ActionResult Post(CreateBugForm form)
        {
            // TODO: Return invalid request if user not found
            var activeUser = form.ActiveUserId != null
                ? this.databaseContext.SiteUser
                    .Find(form.ActiveUserId)
                : null;

            var bug = new Bug
            {
                Created = this.clock.GetCurrentInstant(),
                Title = form.Title,
                Description = form.Description,
                ActiveUser = activeUser,
                Status = BugStatus.New,
            };
            this.databaseContext.Bug.Add(bug);
            this.databaseContext.SaveChanges();

            return this.CreatedAtAction("Get", "BugsSlug", new {slug = bug.Id}, new {slug = bug.Id});
        }

        public class BugDto
        {
            public Guid? ActiveUserId { get; set; }

            public string Slug { get; set; }

            public string Title { get; set; }
        }

        public class CreateBugForm
        {
            public string Title { get; set; }

            public string Description { get; set; }

            public Guid? ActiveUserId { get; set; }
        }
    }
}
