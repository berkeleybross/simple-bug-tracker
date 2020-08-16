using System;
using System.Linq;
using BugTracker.Api.PgSqlDatabase;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace BugTracker.Api.Controllers
{
    [ApiController]
    [Route("api/bugs/{slug}")]
    public class BugsSlugController
        : ControllerBase
    {
        private readonly DatabaseContext databaseContext;

        public BugsSlugController(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        [HttpGet]
        public ActionResult<BugDto> Get(int slug)
        {
            var bug = this.databaseContext.Bug.SingleOrDefault(b => b.Id == slug);
            if (bug == null)
            {
                return this.NotFound();
            }

            return new BugDto
            {
                Slug = bug.Id.ToString(),
                Title = bug.Title,
                Description = bug.Description,
                Created = bug.Created,
                ActiveUserId = bug.ActiveUser?.Id,
                ActiveUserName = bug.ActiveUser?.Name
            };
        }

        public class BugDto
        {
            public string Slug { get; set; }

            public string Title { get; set; }

            public string Description { get; set; }

            public Instant Created { get; set; }

            public Guid? ActiveUserId { get; set; }

            public string ActiveUserName { get; set; }
        }
    }
}
