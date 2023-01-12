﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection.PortableExecutable;


namespace EFC_project
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Storage> Storages { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Document> Documents { get; set; } = null!;
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<Visiting> Visitings { get; set; } = null!;
        public DbSet<Worker> Workers { get; set; } = null!;
        public DbSet<Owner> Owners { get; set; } = null!;


        public ApplicationContext()
        {
            Database.EnsureCreated();   
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Storage>(entity =>
            {
                entity.HasKey(u => u.Number).HasName("PK_Storage");
                entity.Ignore(u => u.NameOfOwner);
                entity.ToTable("Storage");
                entity.Property(u => u.NumberOfRooms).IsRequired();
                entity.Property(u => u.Size).IsRequired();
                entity.HasCheckConstraint("NumberOfRooms", "NumberOfRooms > 0 AND NumberOfRooms < 80");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(u => u.Number).HasName("PK_Room");
                entity.ToTable("Room");
                entity.Property(u => u.TemperatureRange).IsRequired();
                entity.Property(u => u.TemperatureRange).HasMaxLength(25);
                entity.HasOne(p => p.Storage).WithMany(u => u.Rooms);
            });

            modelBuilder.Entity<Visiting>(entity =>
            {
                entity.HasKey(u => u.VisitingId).HasName("PK_Visiting");
                entity.ToTable("Visiting");
                entity.Property(u => u.IdOfClient).IsRequired();
                entity.HasOne(p => p.Storage).WithMany(u => u.Visitings).HasForeignKey(d => d.NumberOfStorage).HasConstraintName("FK_Visiting_NumberOfStorage");
                entity.HasOne(p => p.Client).WithMany(u => u.Visitings).HasForeignKey(d => d.IdOfClient).HasConstraintName("FK_Visiting_IdOfClient");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(u => new { u.PassportSeria, u.PassportNumber });
                entity.HasMany(p => p.Documents).WithOne(u => u.Client);
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.HasKey(u => new { u.PassportSeria, u.PassportNumber });
                entity.HasOne(p => p.Storage).WithMany(u => u.Workers);

            });

            modelBuilder.Entity<Owner>(entity =>
            {
                entity.ToTable("Owner");
            });


            Storage Storage1 = new Storage 
            { 
                Number = 1, 
                NumberOfRooms = 2, 
                IdOfWorker = 111, 
                Size = 20,
                WorkerPassportNumber = "112233",
                WorkerPassportSeria = "112234"
            };
            Storage Storage2 = new Storage 
            { 
                Number = 2, 
                NumberOfRooms = 2, 
                IdOfWorker = 112, 
                Size = 20,
                WorkerPassportNumber = "122233",
                WorkerPassportSeria = "122234"
            };
            Storage Storage3 = new Storage 
            { 
                Number = 3, 
                NumberOfRooms = 2, 
                IdOfWorker = 113, 
                Size = 20,
                WorkerPassportNumber = "132233",
                WorkerPassportSeria = "132234"
            };

            Worker Worker1 = new Worker
            {
                WorkerId = 111,
                NumberOfStorage = Storage1.Number,
                Name = "Alex",
                PhoneNumber = 3854895,
                WorkShedule = "11:00 - 20:00",
                Position = "Packer",
                PassportNumber = "112233",
                PassportSeria = "112234"
            };
            Worker Worker2 = new Worker 
            {
                WorkerId = 112,
                NumberOfStorage = Storage2.Number,
                Name = "Bob",
                PhoneNumber = 38524346,
                WorkShedule = "8:00 - 20:00",
                Position = "Manager",
                PassportNumber = "122233",
                PassportSeria = "122234"
            };
            Worker Worker3 = new Worker 
            {
                WorkerId = 113,
                NumberOfStorage = Storage3.Number,
                Name = "Jack",
                PhoneNumber = 47554895,
                WorkShedule = "11:00 - 21:00",
                Position = "Boss",
                PassportNumber = "132233",
                PassportSeria = "132234"
            };

            Room Room1 = new Room
            {
                Number = 1,
                NumberOfStorage = Storage1.Number,
                NumberOfRows = 3,
                TemperatureRange = "-10 °C - 0 °C"
            };
            Room Room2 = new Room
            {
                Number = 2,
                NumberOfStorage = Storage2.Number,
                NumberOfRows = 3,
                TemperatureRange = "-10 °C - 0 °C"
            };
            Room Room3 = new Room
            {
                Number = 3,
                NumberOfStorage = Storage3.Number,
                NumberOfRows = 5,
                TemperatureRange = "10 °C - 50 °C"
            };
            /*Room Room4 = new Room
            {
                Number = 4,
                NumberOfStorage = Storage2.Number,
                NumberOfRows = 5,
                TemperatureRange = "10 °C - 50 °C"
            };
            Room Room5 = new Room
            {
                Number = 5,
                NumberOfStorage = Storage3.Number,
                NumberOfRows = 10,
                TemperatureRange = "50 °C - 100 °C"
            };
            Room Room6 = new Room
            {
                Number = 6,
                NumberOfStorage = Storage3.Number,
                NumberOfRows = 10,
                TemperatureRange = "50 °C - 100 °C"
            };*/
            Client Client1 = new Client
            {
                ClientId = 1111,
                Name = "George",
                PhoneNumber = 1354551,
                Email = "client1@gmail.com",
                IdOfDocument = 11,
                PassportNumber = "223344",
                PassportSeria = "223345"
            };
            Client Client2 = new Client
            {
                ClientId = 1112,
                Name = "David",
                PhoneNumber = 13346551,
                Email = "client2@gmail.com",
                IdOfDocument = 12,
                PassportNumber = "233344",
                PassportSeria = "233345"
            };
            Client Client3 = new Client
            {
                ClientId = 1113,
                Name = "Woods",
                PhoneNumber = 131354551,
                Email = "client3@gmail.com",
                IdOfDocument = 13,
                PassportNumber = "243344",
                PassportSeria = "243344"
            };

            Document Document1 = new Document
            {
                DocumentId = 11,
                IdOfClient = 1111,
                DateOfTakeRent = DateTime.Now,
                DateOfCancelRent = DateTime.Now.AddYears(10),
                NumberOfCell = 3, 
                ClientId = 1111
            };
            Document Document2 = new Document
            {
                DocumentId = 12,
                IdOfClient = 1112,
                DateOfTakeRent = DateTime.Now.AddYears(1),
                DateOfCancelRent = DateTime.Now.AddYears(20),
                NumberOfCell = 5
            };
            Document Document3 = new Document
            {
                DocumentId = 13,
                IdOfClient = 1113,
                DateOfTakeRent = DateTime.Now.AddYears(2),
                DateOfCancelRent = DateTime.Now.AddYears(30),
                NumberOfCell = 1
            };
       
            Visiting Visiting1 = new Visiting
            {
                VisitingId = 11111,
                IdOfClient = Client1.ClientId,
                NumberOfStorage = Storage1.Number,
                DateOfEntry = "12.04.2022",
                DateOfExit = "13.04.2022"
            };
            Visiting Visiting2 = new Visiting
            {
                VisitingId = 11112,
                IdOfClient = Client2.ClientId,
                NumberOfStorage = Storage2.Number,
                DateOfEntry = "12.12.2022",
                DateOfExit = "13.12.2022"
            };
            Visiting Visiting3 = new Visiting
            {
                VisitingId = 11113,
                IdOfClient = Client3.ClientId,
                NumberOfStorage = Storage3.Number,
                DateOfEntry = "12.03.2022",
                DateOfExit = "13.03.2022"
            };

            modelBuilder.Entity<Storage>().HasData(Storage1, Storage2, Storage3);
            modelBuilder.Entity<Client>().HasData(Client1, Client2, Client3);
            modelBuilder.Entity<Document>().HasData(Document1, Document2, Document3);
            modelBuilder.Entity<Worker>().HasData(Worker1, Worker2, Worker3);
            modelBuilder.Entity<Visiting>().HasData(Visiting1, Visiting2, Visiting3);
            modelBuilder.Entity<Room>().HasData(Room1, Room2, Room3);
        }


    }
}