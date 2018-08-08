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
using FluentAssertions;

namespace ToDoAssignment.Tests
{
    public class NotesControllerIntegrationTest
    {
        private HttpClient _client;
        private readonly NotesContext _context;
        public NotesControllerIntegrationTest()
        {
            var host = new TestServer(new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>());
            _context = host.Host.Services.GetService(typeof(NotesContext)) as NotesContext;
            _client = host.CreateClient();
            CreateData();
        }

        private void CreateData()
        {
            var notes = new List<Note>()
            { new Note()
            {
                Title = "Cars",
                PlainText = "New Moibiles",
                Pin = true,
                Labels = new List<Labels>() { new Labels {Text="Label_1" },new Labels { Text="Label_2"} },
                CList=new List<CheckList>() { new CheckList {ListOne="1",ListTwo="2"} }

            },
            new Note()
            {
                Title = "Bikes",
                PlainText = "New Bikes",
                Pin = true,
                Labels = new List<Labels>() { new Labels {Text="Label_3" },new Labels { Text="Label_4"} },
                CList=new List<CheckList>() { new CheckList {ListOne="1",ListTwo="2"} }
            } };
            _context.Note.AddRange(notes);
            _context.SaveChanges();
        }

        [Fact]
        public async Task TestGetRequestAsync()
        {
            var Response = await _client.GetAsync("/api/notes");
            string jsonResult = await Response.Content.ReadAsStringAsync();
            List<Note> userFromJson = JsonConvert.DeserializeObject<List<Note>>(jsonResult);
            Assert.Equal(2, userFromJson.Count);
            Response.EnsureSuccessStatusCode();
        }


        [Fact]
        public async Task TestGetByTitleAsync()
        {

            var Response = await _client.GetAsync("/api/notes/getLabel/Label_1");
            Assert.Equal(HttpStatusCode.OK, Response.StatusCode);
            string jsonResult = await Response.Content.ReadAsStringAsync();
            List<Note> notes = JsonConvert.DeserializeObject<List<Note>>(jsonResult);

            Response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestGetByLabelAsync()
        {

            var Response = await _client.GetAsync("/api/notes/Bikes");
            Assert.Equal(HttpStatusCode.OK, Response.StatusCode);
            string jsonResult = await Response.Content.ReadAsStringAsync();
            List<Note> userFromJson = JsonConvert.DeserializeObject<List<Note>>(jsonResult);

            Response.EnsureSuccessStatusCode();            
        }



        [Fact]
        public async Task TestPostRequestAsync()
        {
            var notes =
                new Note()
                {
                    ID = 5000,
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
            string jsonResult = await Response.Content.ReadAsStringAsync();
            Note userFromJson = JsonConvert.DeserializeObject<Note>(jsonResult);
            Console.WriteLine("Post id" + userFromJson.ID);

            Response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestPutRequestAsync()
        {
            var note = new Note
            {
                ID = 1,
                Title = "Bikes",
                PlainText = "New Bikes",
                Pin = true,
                Labels = new List<Labels>() { new Labels { Text = "Label_1" }, new Labels { Text = "Label_2" } },
                CList = new List<CheckList>() { new CheckList { ListOne = "1", ListTwo = "2" } }
            };
            var json = JsonConvert.SerializeObject(note);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var Response = await _client.PutAsync("/api/Notes/1", stringContent);

            string jsonResult = await Response.Content.ReadAsStringAsync();
            var notes = JsonConvert.DeserializeObject<Note>(jsonResult);

            Response.EnsureSuccessStatusCode();
        }

          [Fact]
         public async Task TestDeleteRequestAsync()
         {
            var Response = await _client.DeleteAsync("/api/notes/delete/Bikes");
            Response.EnsureSuccessStatusCode();
         }


        [Fact]
        public async Task PostNoteInvalidAsync()
        {
            // Arrange
            var noteToAdd = new Note { PlainText = "Planes" };
            var content = JsonConvert.SerializeObject(noteToAdd);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Notes", stringContent);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
          
        }

        [Fact]
        public async Task PutNoteInvalidAsync()
        {
            // Arrange
            var noteToChange = new Note { PlainText = "Bicycle" };
            var content = JsonConvert.SerializeObject(noteToChange);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/Notes/5", stringContent);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }


    }
}