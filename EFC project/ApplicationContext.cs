using Microsoft.EntityFrameworkCore;
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
using EFC_project.Models;

namespace EFC_project
{
    public class ApplicationContext : DbContext
    {
        public DbSet<MainStorage> Storages { get; set; } = null!;
        //public IQueryable<MainStorage> GetStorageBySize(int size) => FromExpression(() => GetStorageBySize(size));
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Document> Documents { get; set; } = null!;
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<Visiting> Visitings { get; set; } = null!;
        public DbSet<Worker> Workers { get; set; } = null!;
        public DbSet<Owner> Owners { get; set; } = null!;


        public ApplicationContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();

            /*var createSql = @"
                create function [dbo].[GetStorageBySize] (@size int)
                returns table
                as
                return
                    select * from dbo.MainStorage
                    where Size < @size
            ";

            Database.ExecuteSqlRaw(createSql);*/
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           //modelBuilder.HasDbFunction(() => GetStorageBySize(default));

            modelBuilder.Entity<MainStorage>(entity =>
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
                entity.HasOne(p => p.Storage).WithMany(u => u.Rooms).HasForeignKey(b => b.NumberOfStorage).HasConstraintName("FK_Room_NumberOfStorage");
            });

            modelBuilder.Entity<Visiting>(entity =>
            {
                entity.HasKey(u => u.VisitingId).HasName("PK_Visiting");
                entity.ToTable("Visiting");
                entity.Property(u => u.IdOfClient).IsRequired();
                entity.HasOne(p => p.Storage).WithMany(u => u.Visitings).HasForeignKey(d => d.NumberOfStorage).HasConstraintName("FK_Visiting_NumberOfStorage");
                entity.HasOne(p => p.Client).WithMany(u => u.Visitings).HasForeignKey(d => d.ClientPhoneNumber).HasConstraintName("FK_Visiting_ClientPhoneNumber").HasPrincipalKey(c => c.PhoneNumber);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasMany(p => p.Documents).WithOne(u => u.Client).HasForeignKey(a => a.IdOfClient).HasConstraintName("FK_Document_IdOfClient");
                entity.HasAlternateKey(u => u.PhoneNumber);              
                entity.HasAlternateKey(u => u.Email);
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.HasKey(u => new { u.PassportSeria, u.PassportNumber });
                entity.HasOne(p => p.Storage).WithMany(u => u.Workers).HasForeignKey(b => b.NumberOfStorage).HasConstraintName("FK_Worker_NumberOfStorage");

            });

            modelBuilder.Entity<Owner>(entity =>
            {
                entity.ToTable("Owner");
            });


            MainStorage Storage1 = new MainStorage
            { 
                Number = 1, 
                NumberOfRooms = 2, 
                IdOfWorker = 111, 
                Size = 100,
                WorkerPassportNumber = "112233",
                WorkerPassportSeria = "112234"
            };
            MainStorage Storage2 = new MainStorage
            { 
                Number = 2, 
                NumberOfRooms = 2, 
                IdOfWorker = 112, 
                Size = 20,
                WorkerPassportNumber = "122233",
                WorkerPassportSeria = "122234"
            };
            MainStorage Storage3 = new MainStorage
            { 
                Number = 3, 
                NumberOfRooms = 2, 
                IdOfWorker = 113, 
                Size = 35,
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
                TemperatureRange = "-10 °C - 0 °C",
                StorageNumber = Storage1.Number
            };
            Room Room2 = new Room
            {
                Number = 2,
                NumberOfStorage = Storage2.Number,
                NumberOfRows = 10,
                TemperatureRange = "-10 °C - 0 °C",
                StorageNumber = Storage2.Number
            };
            Room Room3 = new Room
            {
                Number = 3,
                NumberOfStorage = Storage3.Number,
                NumberOfRows = 5,
                TemperatureRange = "10 °C - 50 °C",
                StorageNumber = Storage3.Number
            };
            
            Client Client1 = new Client
            {
                ClientId = 1111,
                Name = "George",
                PhoneNumber = "1354551",
                Email = "client1@gmail.com",
                IdOfDocument = 11,
                PassportNumber = "223344",
                PassportSeria = "223345"
            };
            Client Client2 = new Client
            {
                ClientId = 1112,
                Name = "David",
                PhoneNumber = "13346551",
                Email = "client2@gmail.com",
                IdOfDocument = 12,
                PassportNumber = "233344",
                PassportSeria = "233345"
            };
            Client Client3 = new Client
            {
                ClientId = 1113,
                Name = "Woods",
                PhoneNumber = "131354551",
                Email = "client3@gmail.com",
                IdOfDocument = 13,
                PassportNumber = "243344",
                PassportSeria = "243344"
            };
            Client Client4 = new Client
            {
                ClientId = 1114,
                Name = "Hudson",
                PhoneNumber = "1365378965",
                Email = "client4@gmail.com",
                IdOfDocument = 14,
                PassportNumber = "253344",
                PassportSeria = "253344"
            };
            Client Client5 = new Client
            {
                ClientId = 1115,
                Name = "Mykola",
                PhoneNumber = "62586586",
                Email = "client5@gmail.com",
                IdOfDocument = 15,
                PassportNumber = "263344",
                PassportSeria = "263344"
            };
            Client Client6 = new Client
            {
                ClientId = 1116,
                Name = "Shtainer",
                PhoneNumber = "852368523",
                Email = "client6@gmail.com",
                IdOfDocument = 16,
                PassportNumber = "273344",
                PassportSeria = "273344"
            };
            Client Client7 = new Client
            {
                ClientId = 1117,
                Name = "Menendes",
                PhoneNumber = "02369842",
                Email = "client7@gmail.com",
                IdOfDocument = 17,
                PassportNumber = "283344",
                PassportSeria = "283344"
            };

            Document Document1 = new Document
            {
                DocumentId = 11,
                IdOfClient = 1111,
                DateOfTakeRent = DateTime.Now,
                DateOfCancelRent = DateTime.Now.AddYears(1),
                NumberOfCell = 3
            };
            Document Document2 = new Document
            {
                DocumentId = 12,
                IdOfClient = 1112,
                DateOfTakeRent = DateTime.Now.AddYears(1),
                DateOfCancelRent = DateTime.Now.AddYears(2),
                NumberOfCell = 5
            };
            Document Document3 = new Document
            {
                DocumentId = 13,
                IdOfClient = 1113,
                DateOfTakeRent = DateTime.Now.AddYears(2),
                DateOfCancelRent = DateTime.Now.AddYears(4),
                NumberOfCell = 1
            };
            Document Document4 = new Document
            {
                DocumentId = 14,
                IdOfClient = 1114,
                DateOfTakeRent = DateTime.Now.AddYears(3),
                DateOfCancelRent = DateTime.Now.AddYears(4),
                NumberOfCell = 10
            };
            Document Document5 = new Document
            {
                DocumentId = 15,
                IdOfClient = 1115,
                DateOfTakeRent = DateTime.Now.AddYears(4),
                DateOfCancelRent = DateTime.Now.AddYears(7),
                NumberOfCell = 15
            };
            Document Document6 = new Document
            {
                DocumentId = 16,
                IdOfClient = 1116,
                DateOfTakeRent = DateTime.Now.AddYears(5),
                DateOfCancelRent = DateTime.Now.AddYears(10),
                NumberOfCell = 4
            };
            Document Document7 = new Document
            {
                DocumentId = 17,
                IdOfClient = 1117,
                DateOfTakeRent = DateTime.Now.AddYears(1),
                DateOfCancelRent = DateTime.Now.AddYears(15),
                NumberOfCell = 7
            };

            Visiting Visiting1 = new Visiting
            {
                VisitingId = 11111,
                IdOfClient = Client1.ClientId,
                NumberOfStorage = Storage1.Number,
                DateOfEntry = "12.04.2022",
                DateOfExit = "13.04.2022",
                ClientPhoneNumber = Client1.PhoneNumber,
                ClientPassportNumber = Client1.PassportNumber,
                ClientPassportSeria = Client1.PassportSeria

            };
            Visiting Visiting2 = new Visiting
            {
                VisitingId = 11112,
                IdOfClient = Client2.ClientId,
                NumberOfStorage = Storage1.Number,
                DateOfEntry = "12.12.2022",
                DateOfExit = "13.12.2022",
                ClientPhoneNumber = Client2.PhoneNumber,
                ClientPassportNumber = Client2.PassportNumber,
                ClientPassportSeria = Client2.PassportSeria
            };
            Visiting Visiting3 = new Visiting
            {
                VisitingId = 11113,
                IdOfClient = Client3.ClientId,
                NumberOfStorage = Storage1.Number,
                DateOfEntry = "12.03.2022",
                DateOfExit = "13.03.2022",
                ClientPhoneNumber = Client3.PhoneNumber,
                ClientPassportNumber = Client3.PassportNumber,
                ClientPassportSeria = Client3.PassportSeria
            };
            Visiting Visiting4 = new Visiting
            {
                VisitingId = 11114,
                IdOfClient = Client4.ClientId,
                NumberOfStorage = Storage2.Number,
                DateOfEntry = "12.04.2022",
                DateOfExit = "13.04.2022",
                ClientPhoneNumber = Client4.PhoneNumber,
                ClientPassportNumber = Client4.PassportNumber,
                ClientPassportSeria = Client4.PassportSeria
            };
            Visiting Visiting5 = new Visiting
            {
                VisitingId = 11115,
                IdOfClient = Client5.ClientId,
                NumberOfStorage = Storage2.Number,
                DateOfEntry = "12.05.2022",
                DateOfExit = "13.05.2022",
                ClientPhoneNumber = Client5.PhoneNumber,
                ClientPassportNumber = Client5.PassportNumber,
                ClientPassportSeria = Client5.PassportSeria
            };
            Visiting Visiting6 = new Visiting
            {
                VisitingId = 11116,
                IdOfClient = Client6.ClientId,
                NumberOfStorage = Storage2.Number,
                DateOfEntry = "12.06.2022",
                DateOfExit = "13.06.2022",
                ClientPhoneNumber = Client6.PhoneNumber,
                ClientPassportNumber = Client6.PassportNumber,
                ClientPassportSeria = Client6.PassportSeria
            };
            Visiting Visiting7 = new Visiting
            {
                VisitingId = 11117,
                IdOfClient = Client7.ClientId,
                NumberOfStorage = Storage3.Number,
                DateOfEntry = "12.07.2021",
                DateOfExit = "13.07.2021",
                ClientPhoneNumber = Client7.PhoneNumber,
                ClientPassportNumber = Client7.PassportNumber,
                ClientPassportSeria = Client7.PassportSeria
            };

            modelBuilder.Entity<MainStorage>().HasData(Storage1, Storage2, Storage3);
            modelBuilder.Entity<Client>().HasData(Client1, Client2, Client3, Client4, Client5, Client6, Client7);
            modelBuilder.Entity<Document>().HasData(Document1, Document2, Document3, Document4, Document5, Document6, Document7);
            modelBuilder.Entity<Worker>().HasData(Worker1, Worker2, Worker3);
            modelBuilder.Entity<Visiting>().HasData(Visiting1, Visiting2, Visiting3, Visiting4, Visiting5, Visiting6, Visiting7);
            modelBuilder.Entity<Room>().HasData(Room1, Room2, Room3);
        }
    }
}