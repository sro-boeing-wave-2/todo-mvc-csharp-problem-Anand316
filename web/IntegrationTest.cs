using System;
using Web_api_2.Controllers;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using Web_api_2;
using Web_api_2.Models;
using Newtonsoft;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting.Server;
namespace ToDoAssignment.Tests
{
    public class IntegrationTest
    {
        private HttpClient _client;
        private readonly NotesContext _context;
        public IntegrationTest()
        {
            var host = new TestServer(new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>());
            _context = host.Host.Services.GetService(typeof(NotesContext)) as NotesContext;
            _client = host.CreateClient();
        }
        [Fact]
        public async Task TestGetRequestAsync()
        {
            var Response = await _client.GetAsync("/api/notes");
            var ResponseBody = Response;
            Console.WriteLine(ResponseBody.ToString());
            //Assert.Equal(2, ResponseBody.Length);
        }
        [Fact]
        public async Task TestGetByTitle()
        {
            var notes = new Note
            {
                Title = "Cars",
                PlainText = "New Cars",
                Pin = true,
                Labels = new List<Labels>() { new Labels { Text = "Label_1" }, new Labels { Text = "Label_2" } },
                CList = new List<CheckList>() { new CheckList { ListOne = "1", ListTwo = "2" } }
            };
            _context.Note.Add(notes);
            _context.SaveChanges();
            var Response = await _client.GetAsync("/api/notes/Cars");
            Assert.Equal(HttpStatusCode.OK, Response.StatusCode);
            string jsonResult = await Response.Content.ReadAsStringAsync();
            List<Note> userFromJson = JsonConvert.DeserializeObject<List<Note>>(jsonResult);

            Console.WriteLine(userFromJson.Count);
            //Assert.Equal(notes.Title, userFromJson.Title);
            //Assert.Equal(user.Email, userFromJson.Email);


            Assert.Equal("OK", Response.StatusCode.ToString());
        }
        [Fact]
        public async Task TestPostRequestAsync()
        {
            var notes =
                new Note()
                {
                    Title = "My First Note",
                    PlainText = "This is my plaintext",
                    Pin = true,
                    CList = new List<CheckList>()
                    {
                        new CheckList()
                        {
                             ListOne="checklist data 1",
                            ListTwo="true"
                        }
                    },
                    Labels = new List<Labels>()
                    {
                        new Labels()
                        {
                            Text ="labeldata 1"
                        }
                    }
                };
            var json = JsonConvert.SerializeObject(notes);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var Response = await _client.PostAsync("/api/notes", stringContent);
            var ResponseGet = await _client.GetAsync("/api/notes");
            string jsonResult = await ResponseGet.Content.ReadAsStringAsync();
            List<Note> userFromJson = JsonConvert.DeserializeObject<List<Note>>(jsonResult);
            _context.AddRange(notes);
            _context.SaveChanges();
            foreach (Note x in userFromJson)
            {
                Console.WriteLine(x.PlainText);
            }
            Response.EnsureSuccessStatusCode();
            var y = await _context.Note.ToListAsync();
            Console.WriteLine(y.Count);
        }

        [Fact]
        public async Task TestPutRequestAsync()
        {
            var notes = new Note
            {

                Title = "Bikes",
                PlainText = "New Bikes",
                Pin = true,
                Labels = new List<Labels>() { new Labels { Text = "Label_1" }, new Labels { Text = "Label_2" } },
                CList = new List<CheckList>() { new CheckList { ListOne = "1", ListTwo = "2" } }
            };
            var json = JsonConvert.SerializeObject(notes);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var Response = await _client.PutAsync("/api/notes/1", stringContent);
            _context.Note.Update(notes);
            _context.SaveChanges();
            var ResponseGet = await _client.GetAsync("/api/notes");
            var y = await _context.Note.ToListAsync();
            foreach (Note x in y)
            {
                Console.WriteLine(x.Title);
            }
            ResponseGet.EnsureSuccessStatusCode();
        }

          [Fact]
         public async Task TestDeleteRequestAsync()
         {
             var Response = await _client.DeleteAsync("/api/notes/delete/Bikes");
            //Assert.True(condition: result, OkObjectResult);
            //var notes = resultAsOkObjectResult.Value as List<Note>;
            //Assert.Equal(, .StatusCode.ToString());
            Response.EnsureSuccessStatusCode();
         }

        
    }
}