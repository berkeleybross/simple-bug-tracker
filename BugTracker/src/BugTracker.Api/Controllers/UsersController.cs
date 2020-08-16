using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BugTracker.Api.PgSqlDatabase;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController
        : ControllerBase
    {
        private readonly DatabaseContext databaseContext;

        public UsersController(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        [HttpGet]
        public List<UserDto> GetAll()
        {
            return this.databaseContext.SiteUser.OrderBy(u => u.Name).AsEnumerable()
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name
                })
                .ToList();
        }

        [HttpPost]
        public ActionResult Post(CreateUserForm form)
        {
            var id = Guid.NewGuid();
            this.databaseContext.SiteUser.Add(new SiteUser
            {
                Name = form.Name,
                Id = id
            });
            this.databaseContext.SaveChanges();

            return this.CreatedAtAction("Get", new {id = id}, new {id = id});
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> Get(Guid id)
        {
            var user = this.databaseContext.SiteUser.Find(id);

            if (user == null)
            {
                return this.NotFound();
            }

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name
            };
        }

        [HttpPut("{id}")]
        public ActionResult Put(Guid id, CreateUserForm form)
        {
            var user = this.databaseContext.SiteUser.Find(id);

            if (user == null)
            {
                return this.NotFound();
            }

            user.Name = form.Name;
            this.databaseContext.SaveChanges();
            return this.Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            var user = this.databaseContext.SiteUser.Find(id);

            if (user == null)
            {
                return this.NotFound();
            }

            this.databaseContext.SiteUser.Remove(user);
            this.databaseContext.SaveChanges();

            return this.NoContent();
        }

        public class UserDto
        {
            public Guid Id { get; set; }

            public string Name { get; set; }
        }

        public class CreateUserForm
        {
            [Required] public string Name { get; set; }
        }
    }
}
