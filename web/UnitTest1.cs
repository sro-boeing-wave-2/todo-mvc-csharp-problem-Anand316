using System;
using Web_api_2.Controllers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Web_api_2.Models;
using FluentAssertions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace web
{
    public class NotesControllerUnitTest
    {
        private readonly NotesContext todocontext;
        private NotesController controller;

        public NotesControllerUnitTest()
        {
            var optionBuilder = new DbContextOptionsBuilder<NotesContext>();
            optionBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            todocontext = new NotesContext(optionBuilder.Options);
            controller = new NotesController(todocontext);
            CreateData();
        }

        private void CreateData()
        {
            List<Note> notes = new List<Note>()
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

            todocontext.Note.AddRange(notes);
            todocontext.SaveChanges();
        }

      
        [Fact]
        public void TestGetAllNotes()
        {

            var result = controller.GetNote().ToList();
            Console.WriteLine(result.Count);
            foreach(Note x in result)
            {
                Console.WriteLine(x.Title);
            }
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task TestGetByTitle()
        {
            var result = await controller.GetNoteByTitle("Cars");
            var resultAsOkObjectResult = result as OkObjectResult;
            var notes = resultAsOkObjectResult.Value as List<Note>;
            foreach(Note note in notes)
            {
                Assert.Equal("Cars", note.Title);
            }
        }

        [Fact]
        public async Task TestGetByPin()
        {
            var result = await controller.GetNoteByPin(true);
            var resultAsOkObjectResult = result as OkObjectResult;
            var notes = resultAsOkObjectResult.Value as List<Note>;

            foreach(Note note in notes)
            {
                Assert.True(note.Pin);
            }
        }

        [Fact]
        public async Task TestGetByLabel()
        {
            var result = await controller.GetNoteByLabel("Label_3");
            var resultAsOkObjectResult = result as OkObjectResult;
            var notes = resultAsOkObjectResult.Value as List<Note>;
            Assert.NotEmpty(notes);
        }

        [Fact]
        public async Task TestPost()
        {
            var note = new Note
            {
                Title = "Planes",
                PlainText = "New Planes",
                Pin = true,
                Labels = new List<Labels>() { new Labels { Text = "Label_1" }, new Labels { Text = "Label_2" } },
                CList = new List<CheckList>() { new CheckList { ListOne = "1", ListTwo = "2" } }

            };
            var result = await controller.PostNote(note);
            var resultAsOkObjectResult = result as CreatedAtActionResult;
            var noteReturn = resultAsOkObjectResult.Value as Note;
            Assert.Equal(note.PlainText, noteReturn.PlainText);
            
        }

        [Fact]
        public async Task TestPut()
        {
            var notes = new Note
            {
                Title = "Cars",
                PlainText = "New Cars",
                Pin = true,
                Labels = new List<Labels>() { new Labels { Text = "Label_1" }, new Labels { Text = "Label_2" } },
                CList = new List<CheckList>() { new CheckList { ListOne = "1", ListTwo = "2" } }

            };

            var result = await controller.PostNote(notes);
            var resultAsOkObjectResult = result as CreatedAtActionResult;
            var note = resultAsOkObjectResult.Value as Note;
            Assert.Equal(note.Title, notes.Title);
        }

        [Fact]
        public async Task TestDelete()
        {
            var result = await controller.DeleteNote("Cars");
            var resultAsOkObjectResult = result as OkObjectResult;
            var notes = resultAsOkObjectResult.Value as List<Note>;
            foreach(Note note in notes)
            {
                Assert.Equal("Cars", note.Title);
            }

        }
    }
}
