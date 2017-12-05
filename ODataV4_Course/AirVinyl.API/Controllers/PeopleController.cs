using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using AirVinyl.API.Helpers;
using AirVinyl.DataAccessLayer;
using AirVinyl.Model;

namespace AirVinyl.API.Controllers
{
    public class PeopleController : ODataController
    {
        private AirVinylDbContext _context = new AirVinylDbContext();

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_context.People);
        }

        [ReplaceNullContentWithNotFound]
        [ODataRoute("People({key})")]
        public IHttpActionResult Get([FromODataUri] int key)
        {
            var people = _context.People.Where(p => p.PersonId == key);

            if (!people.Any())
            {
                return NotFound();
            }

            return Ok(SingleResult.Create(people));
        }


        [HttpGet]
        [ODataRoute("People({key})/Email")]
        [ODataRoute("People({key})/FirstName")]
        [ODataRoute("People({key})/LastName")]
        [ODataRoute("People({key})/DateOfBirth")]
        [ODataRoute("People({key})/Gender")]
        public IHttpActionResult GetPersonProperty([FromODataUri] int key)
        {
            var person = _context.People.FirstOrDefault(p => p.PersonId == key);
            if (person == null)
            {
                return NotFound();
            }

            var propertyToGet = Url.Request.RequestUri.Segments.Last();
            if (!person.HasProperty(propertyToGet))
            {
                return NotFound();
            }

            var propertyValue = person.GetValue(propertyToGet);

            if (propertyValue == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }

            return this.CreateOKHttpActionResult(propertyValue);
        }


        [HttpGet]
        [ReplaceNullContentWithNotFound]
        [ODataRoute("People({key})/VinylRecords")]
        public IHttpActionResult GetVinylRecordsForPerson([FromODataUri] int key)
        {
            var person = _context.People.FirstOrDefault(p => p.PersonId == key);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(_context.VinylRecords.Where(v => v.Person.PersonId == key));
        }

        [HttpGet]
        [ReplaceNullContentWithNotFound]
        [ODataRoute("People({key})/Friends")]
        [ODataRoute("People({key})/VinylRecords")]
        public IHttpActionResult GetPersonCollectionProperty([FromODataUri] int key)
        {
            var collectionPropertyToGet = Url.Request.RequestUri.Segments.Last();
            var person = _context.People.Include(collectionPropertyToGet).FirstOrDefault(p => p.PersonId == key);
            if (person == null)
            {
                return NotFound();
            }

            var collectionPropertyValue = person.GetValue(collectionPropertyToGet);
            return this.CreateOKHttpActionResult(collectionPropertyValue);

        }

        // alternative: attribute routing
        // [HttpPost]
        // [ODataRoute("People")]
        // POST odata/People
        public IHttpActionResult Post(Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // add the person to the People collection
            _context.People.Add(person);
            _context.SaveChanges();

            // return the created person 
            return Created(person);
        }

        // PUT odata/People('key')
        // alternative: attribute routing
        // [HttpPut]
        // [ODataRoute("People({key})")]
        // PUT is for full updates
        public IHttpActionResult Put([FromODataUri] int key, Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // PUT is for full updates: find the person
            var currentPerson = _context.People.FirstOrDefault(p => p.PersonId == key);

            if (currentPerson == null)
            {
                return NotFound();
            }

            // Alternative: if the person isn't found: Upsert.  This must only
            // be used if the responsibility for creating the key isn't at 
            // server-level.  In our case, we're using auto-increment fields,
            // so this isn't allowed - code is for illustration purposes only!
            //if (currentPerson == null)
            //{
            //    // the key from the URI is the key we should use
            //    person.PersonId = key;
            //    _context.People.Add(person);
            //    _context.SaveChanges();
            //    return Created(person);
            //}

            // if there's an ID property, this should be ignored. But if we try
            // to call SetValues with a different Key value, SetValues will throw an error.
            // Therefore, we set the person's ID to the key.
            person.PersonId = currentPerson.PersonId;
            _context.Entry(currentPerson).CurrentValues.SetValues(person);
            _context.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // PATCH odata/People('key')
        // alternative: attribute routing
        // [HttpPatch]
        // [ODataRoute("People({key})")]
        // PATCH is for partial updates
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Person> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // find a matching person
            var currentPerson = _context.People.FirstOrDefault(p => p.PersonId == key);

            if (currentPerson == null)
            {
                return NotFound();
            }

            // Alternative: if the person isn't found: Upsert.  This must only
            // be used if the responsibility for creating the key isn't at 
            // server-level.  In our case, we're using auto-increment fields,
            // so this isn't allowed - code is for illustration purposes only!
            //if (currentPerson == null)
            //{
            //    var person = new Person();
            //    person.PersonId = key;
            //    patch.Patch(person);
            //    _context.People.Add(person);
            //    _context.SaveChanges();
            //    return Created(person);
            //}

            // apply the changeset to the matching person
            patch.Patch(currentPerson);
            _context.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE odata/People('key')
        // alternative: attribute routing
        // [HttpDelete]
        // [ODataRoute("People({key})")]
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            var currentPerson = _context.People.Include("Friends").FirstOrDefault(p => p.PersonId == key);
            if (currentPerson == null)
            {
                return NotFound();
            }

            // this person might be another person's friend, we
            // need to this person from their friend collections
            var peopleWithCurrentPersonAsFriend =
                _context.People.Include("Friends")
                .Where(p => p.Friends.Select(f => f.PersonId).AsQueryable().Contains(key));

            foreach (var person in peopleWithCurrentPersonAsFriend.ToList())
            {
                person.Friends.Remove(currentPerson);
            }

            _context.People.Remove(currentPerson);
            _context.SaveChanges();

            // return No Content
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST odata/People('key')/Friends/$ref
        [HttpPost]
        [ODataRoute("People({key})/Friends/$ref")]
        public IHttpActionResult CreateLinkToFriend([FromODataUri] int key, [FromBody] Uri link)
        {
            // get the current person, including friends as we need to check those
            var currentPerson = _context.People.Include("Friends").FirstOrDefault(p => p.PersonId == key);
            if (currentPerson == null)
            {
                return NotFound();
            }

            // we need the key value from the passed-in link Uri
            int keyOfFriendToAdd = Request.GetKeyValue<int>(link);

            if (currentPerson.Friends.Any(item => item.PersonId == keyOfFriendToAdd))
            {
                return BadRequest(string.Format("The person with Id {0} is already linked to the person with Id {1}",
                    key, keyOfFriendToAdd));
            }

            // find the friend
            var friendToLinkTo = _context.People.FirstOrDefault(p => p.PersonId == keyOfFriendToAdd);
            if (friendToLinkTo == null)
            {
                return NotFound();
            }

            // add the friend
            currentPerson.Friends.Add(friendToLinkTo);
            _context.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }


        // PUT odata/People('key')/Friends/$ref?$id={'relatedKey'}
        [HttpPut]
        [ODataRoute("People({key})/Friends({relatedKey})/$ref")]
        public IHttpActionResult UpdateLinkToFriend([FromODataUri] int key,
            [FromODataUri] int relatedKey, [FromBody] Uri link)
        {
            // get the current person, including friends as we need to check those
            var currentPerson = _context.People.Include("Friends").FirstOrDefault(p => p.PersonId == key);
            if (currentPerson == null)
            {
                return NotFound();
            }

            // find the current friend
            var currentfriend = currentPerson.Friends.FirstOrDefault(item => item.PersonId == relatedKey);
            if (currentfriend == null)
            {
                return NotFound();
            }

            // check if the person isn't already linked to this friend

            // we need the key value from the passed-in link Uri
            int keyOfFriendToAdd = Request.GetKeyValue<int>(link);
            if (currentPerson.Friends.Any(item => item.PersonId == keyOfFriendToAdd))
            {
                return BadRequest(string.Format("The person with Id {0} is already linked to the person with Id {1}",
                    key, keyOfFriendToAdd));
            }

            // find the new friend
            var friendToLinkTo = _context.People.FirstOrDefault(p => p.PersonId == keyOfFriendToAdd);
            if (friendToLinkTo == null)
            {
                return NotFound();
            }

            // remove the old friend, add the new friend
            currentPerson.Friends.Remove(currentfriend);
            currentPerson.Friends.Add(friendToLinkTo);
            _context.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE odata/People('key')/Friends/$ref?$id={'relatedUriWithRelatedKey'}
        [HttpDelete]
        [ODataRoute("People({key})/Friends({relatedKey})/$ref")]
        public IHttpActionResult DeleteLinkToFriend([FromODataUri] int key, [FromODataUri] int relatedKey)
        {
            // get the current person, including friends as we need to check those
            var currentPerson = _context.People.Include("Friends").FirstOrDefault(p => p.PersonId == key);
            if (currentPerson == null)
            {
                return NotFound();
            }

            var friend = currentPerson.Friends.FirstOrDefault(item => item.PersonId == relatedKey);
            if (friend == null)
            {
                return NotFound();
            }

            currentPerson.Friends.Remove(friend);
            _context.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }


        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}