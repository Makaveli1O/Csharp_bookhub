﻿using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Data
{
    public static class DataInitializer
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var books = PrepairBookModels();

            modelBuilder.Entity<Book>()
                .HasData(books);
        }

        private static List<Book> PrepairBookModels()
        {
            return new List<Book>()
            {
                new Book
                {
                    Id = 1,
                    Description = "BlaBla",
                    Title = "Title",
                },
            };
        }
    }
}
