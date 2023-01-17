using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Xml.Linq;
using EFC_project;
using EFC_project.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

static void Union()
{
    ApplicationContext db = new ApplicationContext();
    var query = (from t in db.Storages where t.Number < 2 select t.Number).
        Union(from t in db.Storages where t.Number > 2 select t.Number).ToList();
    Console.WriteLine($"Union result:");
    foreach (var t in query)
    {
        Console.WriteLine($"Number - {t}");
    }
}
static void Except()
{
    ApplicationContext db = new ApplicationContext();
    var query = (from t in db.Storages where t.Number > 1 select t.Number)
                        .Except(from t in db.Storages where t.Number < 2 select t.Number);
    Console.WriteLine($"\nExcept result:");
    foreach (var t in query)
    {
        Console.WriteLine($"Number - {t}");
    }
}
static void Intersect()
{
    ApplicationContext db = new ApplicationContext();
    var query = (from t in db.Rooms where t.NumberOfRows > 4 select t.Number)
                        .Except(from t in db.Rooms where t.NumberOfStorage != 2 select t.Number);
    Console.WriteLine($"\nIntersect result:");
    foreach (var t in query)
    {
        Console.WriteLine($"Number - {t}");
    }
}
static void Join()
{
    ApplicationContext db = new ApplicationContext();
    var query = from a in db.Clients
                join b in db.Visitings
                on a.PhoneNumber equals b.ClientPhoneNumber
                select new { a.Name, a.PhoneNumber, b.VisitingId, b.ClientPhoneNumber };
    Console.WriteLine($"\nJoin result:");
    foreach (var ab in query)
    {
        Console.WriteLine("Client: " + ab.Name + " " + ab.PhoneNumber + "\nVisiting: " + ab.VisitingId + " " + ab.ClientPhoneNumber);
    }
}
static void Distinct()
{
    ApplicationContext db = new ApplicationContext();
    var query = (from t in db.Rooms select t.TemperatureRange).Distinct().ToList();
    Console.WriteLine($"\nDistinct result:");
    foreach (var it in query)
    {
        Console.WriteLine($"TemperatureRange: {it}");
    }
}
static void GroupBy()
{
    ApplicationContext db = new ApplicationContext();
    var query = from f in db.Documents
                group f by f.NumberOfCell into g
                select new
                {
                    g.Key,
                    Count = g.Count()
                };
    Console.WriteLine($"\nGroupBy result:");
    foreach (var it in query)
    {
        Console.WriteLine($"NumberOfCells - {it.Key}, Count - {it.Count}");
    }
}
static void AgrFunctions()
{
    ApplicationContext db = new ApplicationContext();
    int minSize = db.Storages.Min(a => a.Size);
    int maxSize = db.Storages.Max(a => a.Size);
    int countStorages = db.Storages.Count();
    int avgSize = (int)db.Storages.Average(a => a.Size);
    int sumSize = db.Storages.Sum(a => a.Size);

    Console.WriteLine($"\nAgrFunctions result:");
    Console.WriteLine($"Min Size - {minSize}, \nMax Size - {maxSize}, \nAvg Size - {avgSize}, \nSummary Size - {sumSize}, \nNumber of Storages - {countStorages}");

}

static void EagerLoading()
{
    ApplicationContext db = new ApplicationContext();
    var workers = db.Workers.Include(u => u.Storage).ToList();
    foreach (var worker in workers)
        Console.WriteLine($"{worker.Name} - {worker.Storage.Number}");
}
static void ExplicitLoading()
{
    ApplicationContext db = new ApplicationContext();
    Client client = db.Clients.FirstOrDefault();
    if (client != null)
    {
        db.Documents.Where(u => u.DocumentId == client.IdOfDocument).Load();

        Console.WriteLine($"Client: {client.Name}");
        foreach (var u in client.Documents)
            Console.WriteLine($"Document: {u.DocumentId}");
    }
}
static void LazyLoading()
{
    ApplicationContext db = new ApplicationContext();
    var workers = db.Workers.ToList();
    foreach (Worker worker in workers)
        Console.WriteLine($"{worker.Name} - {worker.Storage.Number}");
}

static void ANTracking()
{
    ApplicationContext db = new ApplicationContext();
    var workers = db.Workers.AsNoTracking().FirstOrDefault();
    //var workers = db.Workers.FirstOrDefault();
    if (workers != null)
    {
        workers.Name = "Stepan";
        db.SaveChanges();
    }
    var workersss = db.Workers.ToList();
    foreach (var a in workersss)
    {
        Console.WriteLine(a.Name);
    }
}
static void Changetracker()
{
    ApplicationContext db = new ApplicationContext();
    db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    var worker = db.Workers.FirstOrDefault();
    if (worker != null)
    {
        worker.Name = "DefaultName";
        db.SaveChanges();
    }

    var workers = db.Workers.ToList();
    foreach (var u in workers)
        Console.WriteLine($"{u.Name} ({u.WorkerId})");
}

static void SavedFunction()
{
    ApplicationContext db = new ApplicationContext();

    var createSql = @"
                create function [dbo].[GetStorageBySize] (@size int)
                returns table
                as
                return
                    select * from dbo.Storage
                    where Size < @size
            ";

    db.Database.ExecuteSqlRaw(createSql);

    var storages = db.Storages.FromSqlRaw("SELECT * FROM dbo.GetStorageBySize(100)").ToList();
    foreach (var u in storages)
        Console.WriteLine($"{u.Number} - {u.Size}");
}
static void SavedProcedure()
{
    ApplicationContext db = new ApplicationContext();

    var createSql = @"
                create procedure [dbo].[GetClientsByName] as
                begin
                    select * from dbo.Client
                    order by Name 
                end
            ";

    db.Database.ExecuteSqlRaw(createSql);

    var clients = db.Clients.FromSqlRaw("EXECUTE dbo.GetClientsByName").ToList();
    foreach (var u in clients)
        Console.WriteLine($"{u.Name} - {u.ClientId}");
}

static async Task AsyncAdd()
{
    ApplicationContext db = new ApplicationContext();
    for (int i = 0; i < 10; i++)
    {
        await db.Rooms.AddAsync(new Room
        {
            NumberOfStorage = 3,
            StorageNumber = 3,
            NumberOfRows = i,
            TemperatureRange = i.ToString()
        });       
        await db.SaveChangesAsync();
        Console.WriteLine(i);
    }
}
static async Task AsyncRead()
{
    ApplicationContext db = new ApplicationContext();
    var workers = await db.Workers.ToListAsync();
    Console.WriteLine("AsyncRead result:");
    foreach (var worker in workers)
    {
        Console.WriteLine(worker.Name);
    }
}
static void LockExample()
{
    ApplicationContext db = new ApplicationContext();
    object locker = new object();
    int v = 4;
    //int a = 1;
    int f = 0; 
    for (int i = 0; i < 10; i++)
    {
        Thread newThread = new(() =>
        {
            for (int j = 0; j < 100; j++)
            {
                lock (locker)
                {
                    db.Rooms.Add(new Room
                    {
                        NumberOfStorage = 1,
                        StorageNumber = 1,
                        NumberOfRows = v,
                        TemperatureRange = f.ToString()
                    });
                    v++;
                    f++;
                    //Console.WriteLine($"{v} - {f}");
                    db.SaveChanges();
                }
            }
        });
        newThread.Start();
        //Thread.Sleep(100);
    }
}
static void MonitorExample()
{
    ApplicationContext db = new ApplicationContext();
    object locker = new object();
    int v = 4;
    int a = 1;
    int f = 0;
    for (int i = 0; i < 10; i++)
    {
        Thread newThread = new(() =>
        {
            for (int j = 0; j < 100; j++)
            {
                bool lockTaken = false;
                try
                {
                    Monitor.Enter(locker, ref lockTaken);
                    db.Rooms.Add(new Room
                    {
                        NumberOfStorage = a,
                        StorageNumber = a,
                        NumberOfRows = v,
                        TemperatureRange = f.ToString()
                    });
                    v++;
                    f++;
                    //Console.WriteLine($"{v} - {f}");
                    db.SaveChanges();
                }
                finally
                {
                    if (lockTaken) Monitor.Exit(locker);
                }
            }
        });
        newThread.Start();
        //Thread.Sleep(100);
    }
}
static void MutexExample()
{
    ApplicationContext db = new ApplicationContext();
    Mutex mutexObj = new Mutex();
    int v = 4;
    int a = 1;
    int f = 0;
    for (int i = 0; i < 10; i++)
    {
        Thread newThread = new(() =>
        {
            for (int j = 0; j < 100; j++)
            {
                mutexObj.WaitOne();
                db.Rooms.Add(new Room
                {
                    NumberOfStorage = a,
                    StorageNumber = a,
                    NumberOfRows = v,
                    TemperatureRange = f.ToString()
                });
                db.SaveChanges();
                v++;
                f++;
                //Console.WriteLine($"{v} - {f}");
                mutexObj.ReleaseMutex();
            }
        });
        newThread.Start();
        //Thread.Sleep(1000);
    }
}

using (ApplicationContext db = new ApplicationContext())
{
    // захист лаб 2 (Client.PhoneNumber i Client.Name унікальні, зв'язок Visitings-Client через PhoneNumber)
    /*var visitings = db.Visitings.ToList();
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
    var documents = db.Documents.ToList();
    Console.WriteLine("Documents list:");
    foreach (Document u in documents)
    {
        Console.WriteLine($"" +
            $"{u.DocumentId}, " +
            $"{u.IdOfClient}, " +
            $"{u.DateOfTakeRent}, " +
            $"{u.DateOfCancelRent}, " +
            $"{u.NumberOfCell}");
    }*/

    /*Union();
    Except();
    Intersect();
    Join();
    Distinct();
    GroupBy();
    AgrFunctions();*/

    //EagerLoading();
    //ExplicitLoading();
    //LazyLoading();

    //ANTracking();
    //Changetracker();

    //SavedFunction();
    //SavedProcedure();

    // захист лаб 3 (за допомогою linq знайти 5 клієнтів які орендували комірки найбільшу кількість днів)
    var query = db.Clients
        .Join(db.Documents,
        client => client.ClientId,
        document => document.IdOfClient,
        (client, document) => new
        {
            ClientId = client.ClientId,
            Days = (document.DateOfCancelRent - document.DateOfTakeRent).TotalDays
        })
        .ToList()
        .GroupBy(p => p.ClientId, (client, total) => new
        {
            Client = client,
            Days = total.Sum(p => p.Days)
        })
        .OrderByDescending(p => p.Days)
        .Take(5);
    
    Console.WriteLine("Top 5 clients for the longest rent:");
    foreach (var u in query)
    {
        Console.WriteLine($"{u}");
    }

    //захист лаб 4
    Stopwatch stopwatch = new Stopwatch();

    //await AsyncAdd();
    //await AsyncRead();
    /*stopwatch.Start();
    LockExample();
    stopwatch.Stop();
    Console.WriteLine($"Lock - {stopwatch.ElapsedMilliseconds}");*/

    /*stopwatch.Start();
    MonitorExample();
    stopwatch.Stop();
    Console.WriteLine($"Monitor - {stopwatch.ElapsedMilliseconds}");*/

    /*stopwatch.Start();
    MutexExample();
    stopwatch.Stop();
    Console.WriteLine($"Mutex - {stopwatch.ElapsedMilliseconds}");*/
}