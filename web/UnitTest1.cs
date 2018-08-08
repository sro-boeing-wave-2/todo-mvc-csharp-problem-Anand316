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
    public class UnitTest1
    {
        private NotesController controller;

        public UnitTest1()
        {
            var optionBuilder = new DbContextOptionsBuilder<NotesContext>();
            optionBuilder.UseInMemoryDatabase("TestDB");
            var todocontext = new NotesContext(optionBuilder.Options);
            controller = new NotesController(todocontext);
            CreateData(todocontext);
        }

        private void CreateData(NotesContext todocontext)
        {
            var notes = new List<Note>()
            { new Note()
            {
                Title = "Mobiles",
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
                Labels = new List<Labels>() { new Labels {Text="Label_1" },new Labels { Text="Label_2"} },
                CList=new List<CheckList>() { new CheckList {ListOne="1",ListTwo="2"} }
            } };

            todocontext.AddRange(notes);
            todocontext.SaveChanges();
        }

      
        [Fact]
        public void TestGetAllNotes()
        {

            var result = controller.GetNote().ToList();
            Console.WriteLine(result.Count);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task TestGetByTitle()
        {
            var result = await controller.GetNoteByTitle("Mobiles");
            var resultAsOkObjectResult = result as OkObjectResult;
            //Assert.True(condition: result, OkObjectResult);
            var notes = resultAsOkObjectResult.Value as List<Note>;
            //Assert.Equal(notes.Select(x=>x.Title=="Anand").Count,notes.Count);
            Assert.NotNull(notes);
        }

        [Fact]
        public async Task TestGetByPin()
        {
            var result = await controller.GetNoteByPin(true);
            var resultAsOkObjectResult = result as OkObjectResult;
            //Assert.True(condition: result, OkObjectResult);
            var notes = resultAsOkObjectResult.Value as List<Note>;

            Assert.NotNull(result);
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
            Console.WriteLine(result);
            var okResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var notes = okResult.Value.Should().BeAssignableTo<Note>().Subject;
            //var notes = controller.GetNote().ToList();
           // Assert.Equal(2, notes.Count);
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
            //Assert.True(condition: result, OkObjectResult);
            var note = resultAsOkObjectResult.Value as Note;
            //Assert.NotNull(note);
            Assert.Equal(note.Title, notes.Title);
        }
    }
}
