using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Web_api_2.Models
{
    public class NotesContext : DbContext
    {
        public NotesContext (DbContextOptions<NotesContext> options)
            : base(options)
        {
        }

        public DbSet<Web_api_2.Models.Note> Note { get; set; }
    }
}
