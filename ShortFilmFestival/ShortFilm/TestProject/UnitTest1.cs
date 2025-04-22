using System;
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
        private StoreController _storeController;
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
            _storeController = new StoreController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task Backend_Test_GetAllStoreItems_ReturnsSuccess()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/getAllStoreitem");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.IsNotEmpty(responseBody);
        }

        [Test]
        public async Task Backend_Test_AddStoreItem_ReturnsSuccess()
        {
            var newStoreData = new Dictionary<string, object>
            {
                { "ProductName", "Sample Product" },
                { "Category", "Groceries" },
                { "StockQuantity", 10 },
                { "Price", 100 },
                { "ExpiryDate", "2024-07-12" }
            };

            var newStore = CreateStoreObject(newStoreData);
            var result = await _storeController.AddStoreItem(newStore);
            Assert.IsNotNull(result);

            var createdStore = GetEntityFromDatabase<Store>(_context, "Stores", 1);
            Assert.IsNotNull(createdStore);
            Assert.AreEqual(newStore.ProductName, createdStore.ProductName);
        }

        private Store CreateStoreObject(Dictionary<string, object> storeData)
        {
            var store = new Store();
            foreach (var kvp in storeData)
            {
                var property = typeof(Store).GetProperty(kvp.Key);
                if (property != null)
                {
                    var value = Convert.ChangeType(kvp.Value, property.PropertyType);
                    property.SetValue(store, value);
                }
            }
            return store;
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
        public void Backend_Store_Id_PropertyExists_ReturnExpectedDataTypes_int()
        {
            string assemblyName = "dotnetapp";
            string typeName = "dotnetapp.Models.Store";
            Assembly assembly = Assembly.Load(assemblyName);
            Type storeType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = storeType.GetProperty("Id");
            Assert.IsNotNull(propertyInfo, "Property Id does not exist in Store class");
            Assert.AreEqual(typeof(int), propertyInfo.PropertyType);
        }

        [Test]
        public void Backend_Store_ProductName_PropertyExists_ReturnExpectedDataTypes_string()
        {
            string assemblyName = "dotnetapp";
            string typeName = "dotnetapp.Models.Store";
            Assembly assembly = Assembly.Load(assemblyName);
            Type storeType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = storeType.GetProperty("ProductName");
            Assert.IsNotNull(propertyInfo, "Property ProductName does not exist in Store class");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType);
        }

        [Test]
        public void Backend_Store_Category_PropertyExists_ReturnExpectedDataTypes_string()
        {
            string assemblyName = "dotnetapp";
            string typeName = "dotnetapp.Models.Store";
            Assembly assembly = Assembly.Load(assemblyName);
            Type storeType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = storeType.GetProperty("Category");
            Assert.IsNotNull(propertyInfo, "Property Category does not exist in Store class");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType);
        }

        [Test]
        public void Backend_Store_StockQuantity_PropertyExists_ReturnExpectedDataTypes_int()
        {
            string assemblyName = "dotnetapp";
            string typeName = "dotnetapp.Models.Store";
            Assembly assembly = Assembly.Load(assemblyName);
            Type storeType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = storeType.GetProperty("StockQuantity");
            Assert.IsNotNull(propertyInfo, "Property StockQuantity does not exist in Store class");
            Assert.AreEqual(typeof(int), propertyInfo.PropertyType);
        }

        [Test]
        public void Backend_Store_Price_PropertyExists_ReturnExpectedDataTypes_int()
        {
            string assemblyName = "dotnetapp";
            string typeName = "dotnetapp.Models.Store";
            Assembly assembly = Assembly.Load(assemblyName);
            Type storeType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = storeType.GetProperty("Price");
            Assert.IsNotNull(propertyInfo, "Property Price does not exist in Store class");
            Assert.AreEqual(typeof(int), propertyInfo.PropertyType);
        }

        [Test]
        public void Backend_Store_ExpiryDate_PropertyExists_ReturnExpectedDataTypes_string()
        {
            string assemblyName = "dotnetapp";
            string typeName = "dotnetapp.Models.Store";
            Assembly assembly = Assembly.Load(assemblyName);
            Type storeType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = storeType.GetProperty("ExpiryDate");
            Assert.IsNotNull(propertyInfo, "Property ExpiryDate does not exist in Store class");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType);
        }

        [Test]
        public void Backend_Test_StoreController_Class_Exists()
        {
            var _controllerType = typeof(StoreController);
            Assert.NotNull(_controllerType);
        }

        [Test]
        public void Backend_Test_GetAllStoreItems_Method_Exists()
        {
            var _controllerType = typeof(StoreController);
            var methodInfo = _controllerType.GetMethod("GetAllStoreItems");
            Assert.NotNull(methodInfo);
        }

        [Test]
        public void Backend_Test_GetAllStoreItems_Method_HasHttpGetAttribute()
        {
            var _controllerType = typeof(StoreController);
            var methodInfo = _controllerType.GetMethod("GetAllStoreItems");
            var httpGetAttribute = methodInfo.GetCustomAttributes(typeof(HttpGetAttribute), true).FirstOrDefault();
            Assert.NotNull(httpGetAttribute);
        }

        [Test]
        public void Backend_Test_AddStoreItem_Method_Exists()
        {
            var _controllerType = typeof(StoreController);
            var methodInfo = _controllerType.GetMethod("AddStoreItem");
            Assert.NotNull(methodInfo);
        }

        [Test]
        public void Backend_Test_AddStoreItem_Method_HasHttpPostAttribute()
        {
            var _controllerType = typeof(StoreController);
            var methodInfo = _controllerType.GetMethod("AddStoreItem");
            var httpPostAttribute = methodInfo.GetCustomAttributes(typeof(HttpPostAttribute), true).FirstOrDefault();
            Assert.NotNull(httpPostAttribute);
        }

        [Test]
        public async Task Backend_Test_AddStoreItem_Success()
        {
            var newStore = new Store
            {
                ProductName = "Sample Product",
                Category = "Groceries",
                StockQuantity = 10,
                Price = 100,
                ExpiryDate = "2024-07-12"
            };

            var result = await _storeController.AddStoreItem(newStore);
            var createdResult = result.Result as CreatedAtActionResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual((int)HttpStatusCode.Created, createdResult.StatusCode);
            
            var addedStore = createdResult.Value as Store;
            Assert.IsNotNull(addedStore);
            Assert.AreEqual(newStore.ProductName, addedStore.ProductName);
            Assert.AreEqual(newStore.Price, addedStore.Price);
        }
    }
}