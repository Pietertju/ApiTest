using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest.Context
{
    using ApiTest.Models;
    using Microsoft.EntityFrameworkCore;

    public class TodoContext
        : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {

        }

        public DbSet<TodoItem> TodoEntries { get; set; }
    }
}
