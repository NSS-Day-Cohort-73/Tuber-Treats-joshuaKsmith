using TuberTreats.Models;
using TuberTreats.Models.DTOs;

List<TuberOrder> tuberOrders = new List<TuberOrder>
{
    new TuberOrder()
    {
        Id = 1,
        OrderPlacedOnDate = DateTime.Today,
        CustomerId = 1,
        TuberDriverId = null,
        DeliveredOnDate = null,
    },
    new TuberOrder()
    {
        Id = 2,
        OrderPlacedOnDate = DateTime.Today,
        CustomerId = 2,
        TuberDriverId = null,
        DeliveredOnDate = null,
        Toppings = {}
    },
    new TuberOrder()
    {
        Id = 3,
        OrderPlacedOnDate = DateTime.Today,
        CustomerId = 4,
        TuberDriverId = 1,
        DeliveredOnDate = null,
    }
};
List<Topping> toppings = new List<Topping>
{
    new Topping()
    {
        Id = 1,
        Name = "Sour Cream"
    },
    new Topping()
    {
        Id = 2,
        Name = "Butter"
    },
    new Topping()
    {
        Id = 3,
        Name = "Cheese Sauce"
    },
    new Topping()
    {
        Id = 4,
        Name = "Chives"
    },
    new Topping()
    {
        Id = 5,
        Name = "Bacon"
    },

};
List<TuberTopping> tuberToppings = new List<TuberTopping>
{
    new TuberTopping()
    {
        Id = 1,
        TuberOrderId = 1,
        ToppingId = 1
    },
    new TuberTopping()
    {
        Id = 2,
        TuberOrderId = 1,
        ToppingId = 2
    },
    new TuberTopping()
    {  
        Id = 3,
        TuberOrderId = 3,
        ToppingId = 1
    }
};
List<TuberDriver> tuberDrivers = new List<TuberDriver>
{
    new TuberDriver()
    {
        Id = 1,
        Name = "Smooth Kev",
    },
    new TuberDriver()
    {
        Id = 2,
        Name = "Ricky",
    },
    new TuberDriver()
    {
        Id = 3,
        Name = "Kekish",
    }
};
List<Customer> customers = new List<Customer>
{
    new Customer()
    {
        Id = 1,
        Name = "Arnold",
        Address = "",
    },
    new Customer()
    {
        Id = 2,
        Name = "Gisella",
        Address = "",
    },
    new Customer()
    {
        Id = 3,
        Name = "Bruno",
        Address = "",
    },
    new Customer()
    {
        Id = 4,
        Name = "Mark",
        Address = "",
    },
    new Customer()
    {
        Id = 5,
        Name = "Doug",
        Address = "",
    }
};


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


//add endpoints here









////////////////////////////////////////
app.Run();
//don't touch or move this!
public partial class Program { }