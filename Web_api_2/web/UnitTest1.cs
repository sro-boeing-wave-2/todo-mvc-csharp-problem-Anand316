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
            var notes = new Note
            {
                Title = "Anand",
                PlainText = "Hello World",
                Pin = true,
                Labels = new List<Labels>() { new Labels {Text="Label_1" },new Labels { Text="Label_2"} },
                CList=new List<CheckList>() { new CheckList {ListOne="1",ListTwo="2"} }

            };

            todocontext.AddRange(notes);
            todocontext.SaveChanges();
        }

      
        [Fact]
        public void TestGetAllNotes()
        {

            var result = controller.GetNote().ToList();
            Console.WriteLine(result.Count);
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void TestGetByTitle()
        {
            var result = controller.GetNoteByTitle("Anand");
            Console.WriteLine(result.Id);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task TestPost()
        {
            var note = new Note
            {
                Title = "Shashi",
                PlainText = "Hello",
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
                Title = "Anand Kumar",
                PlainText = "Hello World",
                Pin = true,
                Labels = new List<Labels>() { new Labels { Text = "Label_1" }, new Labels { Text = "Label_2" } },
                CList = new List<CheckList>() { new CheckList { ListOne = "1", ListTwo = "2" } }

            };

            var result = await controller.PutNote(2, notes);
            var okResult = result.Should().BeOfType<NoContentResult>().Subject;
            //var note = okResult.Value.Should().BeAssignableTo<Note>().Subject;
        }
    }
}
