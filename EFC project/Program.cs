using System;
using System.Collections.Generic;
using EFC_project;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using (ApplicationContext db = new ApplicationContext())
{
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
}
// получение данных
using (ApplicationContext db = new ApplicationContext())
{
    // получаем объекты из бд и выводим на консоль
    var storages = db.Storages.ToList();
    Console.WriteLine("Storages list:");
    foreach (Storage u in storages)
    {
        Console.WriteLine($"{u.Number}, {u.NumberOfRooms}, {u.IdOfWorker}, {u.Size}");
    }
}