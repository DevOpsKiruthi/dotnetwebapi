using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using dotnetapp.Controllers;
using dotnetapp.Models;

namespace TestProject
{
    public class Tests
    {
        private ApplicationDbContext _context;
        private FilmsController _filmsController;
        private HttpClient _httpClient;

        [SetUp]
        public void SetUp()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8080/");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _filmsController = new FilmsController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task Backend_Test_GetAllFilms_ReturnsSuccess()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/films");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.IsNotEmpty(responseBody);
        }

        [Test]
        public async Task Backend_Test_AddFilm_ReturnsSuccess()
        {
            var newFilmData = new Dictionary<string, object>
            {
                { "Title", "The Mysterious Forest" },
                { "Director", "Ava Smith" },
                { "Duration", 25 },
                { "Synopsis", "A journey into the unknown." },
                { "Genre", "Mystery" },
                { "VoteCount", 0 }
            };

            var newFilm = CreateFilmObject(newFilmData);
            var result = await _filmsController.AddFilm(newFilm);
            Assert.IsNotNull(result);

            var createdFilm = GetEntityFromDatabase<Film>(_context, "Films", 1);
            Assert.IsNotNull(createdFilm);
            Assert.AreEqual(newFilm.Title, createdFilm.Title);
        }

        private Film CreateFilmObject(Dictionary<string, object> filmData)
        {
            var film = new Film();
            foreach (var kvp in filmData)
            {
                var property = typeof(Film).GetProperty(kvp.Key);
                if (property != null)
                {
                    var value = Convert.ChangeType(kvp.Value, property.PropertyType);
                    property.SetValue(film, value);
                }
            }
            return film;
        }

        private TEntity GetEntityFromDatabase<TEntity>(DbContext context, string collectionName, int id)
        {
            var entityType = typeof(TEntity);
            var propertyInfoId = entityType.GetProperty("Id");
            var propertyInfoCollection = context.GetType().GetProperty(collectionName);
            var entities = propertyInfoCollection.GetValue(context, null) as IEnumerable<TEntity>;
            var entity = entities.FirstOrDefault(e => (int)propertyInfoId.GetValue(e) == id);
            return entity;
        }

        [Test]
        public void Backend_Film_Id_PropertyExists_ReturnExpectedDataTypes_int()
        {
            var type = typeof(Film);
            var propertyInfo = type.GetProperty("Id");
            Assert.IsNotNull(propertyInfo);
            Assert.AreEqual(typeof(int), propertyInfo.PropertyType);
        }

        [Test]
        public void Backend_Film_Title_PropertyExists_ReturnExpectedDataTypes_string()
        {
            var propertyInfo = typeof(Film).GetProperty("Title");
            Assert.IsNotNull(propertyInfo);
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType);
        }

        [Test]
        public void Backend_Film_Director_PropertyExists_ReturnExpectedDataTypes_string()
        {
            var propertyInfo = typeof(Film).GetProperty("Director");
            Assert.IsNotNull(propertyInfo);
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType);
        }

        [Test]
        public void Backend_Film_Duration_PropertyExists_ReturnExpectedDataTypes_int()
        {
            var propertyInfo = typeof(Film).GetProperty("Duration");
            Assert.IsNotNull(propertyInfo);
            Assert.AreEqual(typeof(int), propertyInfo.PropertyType);
        }

        [Test]
        public void Backend_Film_Synopsis_PropertyExists_ReturnExpectedDataTypes_string()
        {
            var propertyInfo = typeof(Film).GetProperty("Synopsis");
            Assert.IsNotNull(propertyInfo);
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType);
        }

        [Test]
        public void Backend_Film_Genre_PropertyExists_ReturnExpectedDataTypes_string()
        {
            var propertyInfo = typeof(Film).GetProperty("Genre");
            Assert.IsNotNull(propertyInfo);
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType);
        }

        [Test]
        public void Backend_Film_VoteCount_PropertyExists_ReturnExpectedDataTypes_int()
        {
            var propertyInfo = typeof(Film).GetProperty("VoteCount");
            Assert.IsNotNull(propertyInfo);
            Assert.AreEqual(typeof(int), propertyInfo.PropertyType);
        }

        [Test]
        public void Backend_Test_FilmsController_Class_Exists()
        {
            var controllerType = typeof(FilmsController);
            Assert.NotNull(controllerType);
        }

        [Test]
        public void Backend_Test_GetAllFilms_Method_Exists()
        {
            var methodInfo = typeof(FilmsController).GetMethod("GetAllFilms");
            Assert.NotNull(methodInfo);
        }

        [Test]
        public void Backend_Test_GetAllFilms_Method_HasHttpGetAttribute()
        {
            var methodInfo = typeof(FilmsController).GetMethod("GetAllFilms");
            var attribute = methodInfo.GetCustomAttributes(typeof(HttpGetAttribute), true).FirstOrDefault();
            Assert.NotNull(attribute);
        }

        [Test]
        public void Backend_Test_AddFilm_Method_Exists()
        {
            var methodInfo = typeof(FilmsController).GetMethod("AddFilm");
            Assert.NotNull(methodInfo);
        }

        [Test]
        public void Backend_Test_AddFilm_Method_HasHttpPostAttribute()
        {
            var methodInfo = typeof(FilmsController).GetMethod("AddFilm");
            var attribute = methodInfo.GetCustomAttributes(typeof(HttpPostAttribute), true).FirstOrDefault();
            Assert.NotNull(attribute);
        }

        [Test]
        public async Task Backend_Test_AddFilm_Success()
        {
            var newFilm = new Film
            {
                Title = "The Final Hour",
                Director = "Liam Scott",
                Duration = 18,
                Synopsis = "A gripping tale of time.",
                Genre = "Thriller",
                VoteCount = 0
            };

            var result = await _filmsController.AddFilm(newFilm);
            var createdResult = result.Result as CreatedAtActionResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual((int)HttpStatusCode.Created, createdResult.StatusCode);

            var addedFilm = createdResult.Value as Film;
            Assert.IsNotNull(addedFilm);
            Assert.AreEqual(newFilm.Title, addedFilm.Title);
            Assert.AreEqual(newFilm.Director, addedFilm.Director);
        }
    }
}
