using System;
using System.Linq;
using BugTracker.Api.PgSqlDatabase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var bug = this.databaseContext.Bug.Include(b => b.ActiveUser).SingleOrDefault(b => b.Id == slug);
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
                ActiveUserName = bug.ActiveUser?.Name,
                Status = bug.Status,
            };
        }

        [HttpPut]
        public ActionResult<BugDto> Put(int slug, EditBugForm form)
        {
            var bug = this.databaseContext.Bug.Include(b => b.ActiveUser).SingleOrDefault(b => b.Id == slug);
            if (bug == null)
            {
                return this.NotFound();
            }

            // TODO: Return invalid request if user not found
            bug.ActiveUser = form.ActiveUserId != null
                ? this.databaseContext.SiteUser
                    .Find(form.ActiveUserId)
                : null;
            bug.Description = form.Description;
            bug.Title = form.Title;
            this.databaseContext.SaveChanges();

            return this.Ok();
        }

        [HttpDelete]
        public ActionResult<BugDto> Delete(int slug)
        {
            var bug = this.databaseContext.Bug.SingleOrDefault(b => b.Id == slug);
            if (bug == null)
            {
                return this.NotFound();
            }

            bug.Status = BugStatus.Closed;
            this.databaseContext.SaveChanges();

            return this.Ok();
        }

        public class BugDto
        {
            public string Slug { get; set; }

            public string Title { get; set; }

            public string Description { get; set; }

            public Instant Created { get; set; }

            public Guid? ActiveUserId { get; set; }

            public string ActiveUserName { get; set; }

            public BugStatus Status { get; set; }
        }

        public class EditBugForm
        {
            public string Title { get; set; }

            public string Description { get; set; }

            public Guid? ActiveUserId { get; set; }
        }
    }
}
