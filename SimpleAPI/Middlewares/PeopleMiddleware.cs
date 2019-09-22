using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleAPI.Models;

namespace SimpleAPI.Middlewares
{
    public class PeopleMiddleware
    {
        private PeopleContext db { get; set; }
        public PeopleMiddleware(PeopleContext context)
        {
            db = context;
        }
        public async Task<IEnumerable<Person>> GetPeople()
        {
            return await db.Persons.ToListAsync();
        }
        public Person GetByName(string name)
        {
            return db.Persons.FirstOrDefault(d => d.Name == name);
        }
        public async Task<Person> GetByNameAsync(string name)
        {
            return await db.Persons.FirstOrDefaultAsync(p => p.Name == name);
        }
        public async void CreatePerson(Person person)
        {
            Guid g = Guid.NewGuid();
            person.Id = g;
            db.Persons.Add(person);
            await db.SaveChangesAsync();
        }
        public async void DeletePerson(string name)
        {
            Person pers = new Person { Name = name };
            db.Entry(pers).State = EntityState.Deleted;
            await db.SaveChangesAsync();
        }
    }
}
