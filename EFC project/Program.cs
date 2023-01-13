using System;
using System.Collections.Generic;
using EFC_project;
using EFC_project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;

using (ApplicationContext db = new ApplicationContext())
{
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
}

static void Create()
{
    ApplicationContext db = new ApplicationContext();
    MainStorage Storage1 = new MainStorage
    {
        //Number = 1,
        NumberOfRooms = 2,
        IdOfWorker = 111,
        Size = 20,
        WorkerPassportNumber = "112233",
        WorkerPassportSeria = "112234"
    };
    MainStorage Storage2 = new MainStorage
    {
       // Number = 2,
        NumberOfRooms = 2,
        IdOfWorker = 112,
        Size = 20,
        WorkerPassportNumber = "122233",
        WorkerPassportSeria = "122234"
    };
    MainStorage Storage3 = new MainStorage
    {
       // Number = 3,
        NumberOfRooms = 2,
        IdOfWorker = 113,
        Size = 20,
        WorkerPassportNumber = "132233",
        WorkerPassportSeria = "132234"
    };

    db.Storages.AddRange(Storage1, Storage2, Storage3);
    db.SaveChanges();

    var storages = db.Storages.ToList();
    Console.WriteLine("Storages list:");
    foreach (MainStorage u in storages)
    {
        Console.WriteLine($"{u.Number}, {u.NumberOfRooms}, {u.IdOfWorker}, {u.Size}");
    }
}
static void Update()
{
    ApplicationContext db = new ApplicationContext();
    var temp = db.Storages.Where(u => u.Number == 1).First();
    temp.NumberOfRooms = 10;
    db.SaveChanges();

    var storages = db.Storages.ToList();
    Console.WriteLine("Storages list:");
    foreach (MainStorage u in storages)
    {
        Console.WriteLine($"{u.Number}, {u.NumberOfRooms}, {u.IdOfWorker}, {u.Size}");
    }
}
static void Delete()
{
    ApplicationContext db = new ApplicationContext();
    var temp = db.Storages.Where(u => u.Number == 1).Single();
    db.Storages.Remove(temp);
    db.SaveChanges();

    var storages = db.Storages.ToList();
    Console.WriteLine("Storages list:");
    foreach (MainStorage u in storages)
    {
        Console.WriteLine($"{u.Number}, {u.NumberOfRooms}, {u.IdOfWorker}, {u.Size}");
    }
}

using (ApplicationContext db = new ApplicationContext())
{
    var visitings = db.Visitings.ToList();
    Console.WriteLine("Visitings list:");
    foreach (Visiting u in visitings)
    {
        Console.WriteLine($"" +
            $"{u.VisitingId}, " +
            $"{u.IdOfClient}, " +
            $"{u.NumberOfStorage}, " +
            $"{u.DateOfEntry}, " +
            $"{u.DateOfExit}, " +
            $"ClientPhoneNumber - {u.ClientPhoneNumber}, " +
            $"{u.ClientPassportNumber}, " +
            $"{u.ClientPassportSeria}, ");
    }

    var clients = db.Clients.ToList();
    Console.WriteLine("Clients list:");
    foreach (Client u in clients)
    {
        Console.WriteLine($"" +
            $"{u.ClientId}, " +
            $"{u.Name}, " +
            $"PhoneNumber - {u.PhoneNumber}, " +
            $"{u.Email}, " +
            $"{u.IdOfDocument}, " +
            $"{u.PassportNumber}, " +
            $"{u.PassportSeria}, ");
    }

    //Create();
    //Update();
    //Delete();
}