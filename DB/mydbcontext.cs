using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ice_cream.Models;



namespace ice_cream.DB
{
    public class mydbcontext : DbContext
    {
    
        public mydbcontext(DbContextOptions<mydbcontext> options) : base(options)
        {

        }

        public virtual DbSet<Userregister> userregister { get; set; }

        public virtual DbSet<Adminregister> adminregister { get; set; }

        public virtual DbSet<Icecreams> icecreams { get; set; }

        public virtual DbSet<Recipes> recipes { get; set; }

        public virtual DbSet<Books> books { get; set; }

        public virtual DbSet<BooksOrder> booksorder { get; set; }

        public virtual DbSet<Icecreamorder> icecreamorder { get; set; }

        public virtual DbSet<Contactus> contactus { get; set; }

        public virtual DbSet<Feedback> feedback { get; set; }


    }
}
