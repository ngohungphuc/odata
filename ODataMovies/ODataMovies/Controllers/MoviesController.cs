using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using ODataMovies.Business;
using ODataMovies.Models;
using System.Diagnostics;

namespace ODataMovies.Controllers
{
    public class MoviesController : ODataController
    {
        [EnableQuery]
        public IList<Movie> Get()
        {
            return m_service.Movies;
        }

        public Movie Get([FromODataUri] int key)
        {
            IEnumerable<Movie> movie = m_service.Movies.Where(m => m.Id == key);
            if (movie.Count() == 0)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            else
                return movie.FirstOrDefault();
        }

        /// <summary>
        /// Creates a new movie.
        /// Use the POST http verb.
        /// Set Content-Type:Application/Json
        /// Set body as: { "Id":0,"Title":"A new movie","ReleaseDate":"2015-10-25T00:00:00+05:30","Rating":"FourStar" }
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public IHttpActionResult Post([FromBody] Movie movie)
        {
            try
            {
                return Ok<Movie>(m_service.Add(movie));
            }
            catch (ArgumentNullException e)
            {
                Debugger.Log(1, "Error", e.Message);
                return BadRequest();
            }
            catch (ArgumentException e)
            {
                Debugger.Log(1, "Error", e.Message);
                return BadRequest();
            }
            catch (InvalidOperationException e)
            {
                Debugger.Log(1, "Error", e.Message);
                return Conflict();
            }
        }

        /// <summary>
        /// Saves the entire Movie object to the object specified by key (id). Is supposed to overwrite all properties
        /// Use the PUT http verb
        /// Set Content-Type:Application/Json
        /// Set body as: { "Id":0,"Title":"StarWars - The Force Awakens","ReleaseDate":"2015-10-25T00:00:00+05:30","Rating":"FourStar" }
        /// </summary>
        /// <param name="key"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        public IHttpActionResult Put(int key, Movie movie)
        {
            try
            {
                movie.Id = key;
                return Ok<Movie>(m_service.Save(movie));
            }
            catch (ArgumentNullException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Use the DELETE http verb
        /// Request for odata/Movies(1)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IHttpActionResult Delete(int key)
        {
            if (m_service.Remove(key))
                return Ok();
            else
                return NotFound();
        }

        /// <summary>
        /// Use the PATCH http Verb
        /// Set Content-Type:Application/Json
        /// Call this using following in request body: { "Rating":"ThreeStar" }        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="moviePatch"></param>
        /// <returns></returns>
        public IHttpActionResult Patch(int key, Delta<Movie> moviePatch)
        {
            Movie movie = m_service.Find(key);
            if (movie == null) return NotFound();
            moviePatch.CopyChangedValues(movie);
            return Ok<Movie>(m_service.Save(movie));
        }

        private DataService m_service = new DataService();
    }
}