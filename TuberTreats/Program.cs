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
        TuberDriverId = 2,
        DeliveredOnDate = DateTime.Now,
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


// /tuberorders

//endpoint for getting all orders
app.MapGet("/tuberorders", () => 
{
    return tuberOrders.Select(to => 
    {
        TuberDriver employee = tuberDrivers.FirstOrDefault(td => td.Id == to.TuberDriverId);
        Customer customer = customers.FirstOrDefault(c => c.Id == to.CustomerId);
        List<TuberTopping> tuberTopping = tuberToppings.Where(tt => tt.TuberOrderId == to.Id).ToList();
        return new TuberOrderDTO
        {
            Id = to.Id,
            OrderPlacedOnDate = to.OrderPlacedOnDate,
            CustomerId = to.CustomerId,
            Customer = customer == null ? null : new CustomerDTO
            {
                Id = customer.Id,
                Name = customer.Name,
                Address = customer.Address
            },
            TuberDriverId = to.TuberDriverId,
            TuberDriver = employee == null ? null : new TuberDriverDTO
            {
                Id = employee.Id,
                Name = employee.Name,
            },
            DeliveredOnDate = to.DeliveredOnDate,
        };
    });
});

//endpoint for getting an order by id

//endpoint for submitting a new order

//endpoint for assigning a driver to an order

//endpoint for completing an order



// /toppings

//endpoint for getting all toppings
app.MapGet("/toppings", () => 
{
    return toppings.Select(t => new ToppingDTO
    {
        Id = t.Id,
        Name = t.Name
    });
});


//endpoint for getting a topping by id
app.MapGet("/toppings/{id}", (int id) => 
{
    Topping topping = toppings.FirstOrDefault(t => t.Id == id);
    if (topping == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(new ToppingDTO
    {
        Id = topping.Id,
        Name = topping.Name
    });
});


// /tubertoppings

//endpoint for getting all tubertoppings
app.MapGet("/tubertoppings", () =>
{
    return tuberToppings.Select(tt => new TuberToppingDTO
    {
        Id = tt.Id,
        TuberOrderId = tt.TuberOrderId,
        ToppingId = tt.ToppingId
    });
});

//endpoint for adding a topping to a tuberorder

//endpoint for removing a topping from a tuberorder



// /customers

//endpoint for getting all customers
app.MapGet("/customers", () => 
{
    return customers.Select(c => new CustomerDTO
    {
        Id = c.Id,
        Name = c.Name,
        Address = c.Address
    });
});

//endpoint for getting a customer by id
app.MapGet("/customers/{id}", (int id) => 
{
    Customer customer = customers.FirstOrDefault(c => c.Id == id);
    if (customer == null)
    {
        return Results.NotFound();
    }
    List<TuberOrder> orders = tuberOrders.Where(to => to.TuberDriverId == id).ToList();
    return Results.Ok(new CustomerDTO
    {
        Id = customer.Id,
        Name = customer.Name,
        Address = customer.Address,
        TuberOrders = orders.Select(o => new TuberOrderDTO
        {
            Id = o.Id,
            OrderPlacedOnDate = o.OrderPlacedOnDate,
            CustomerId = o.CustomerId,
            TuberDriverId = o.TuberDriverId,
            DeliveredOnDate = o.DeliveredOnDate
        }).ToList()
    });
});

//endpoint for adding a customer

//endpoint for deleting a customer



// /tuberdrivers

//endpoint for getting all employees
app.MapGet("/tuberdrivers", () => 
{
    return tuberDrivers.Select(td => new TuberDriverDTO
    {
        Id = td.Id,
        Name = td.Name
    });
});

//endpoint for getting an employee by id
app.MapGet("/tuberDrivers/{id}", (int id) => 
{
    TuberDriver employee = tuberDrivers.FirstOrDefault(td => td.Id == id);
    if (employee == null)
    {
        return Results.NotFound();
    }
    List<TuberOrder> deliveries = tuberOrders.Where(to => to.TuberDriverId == id).ToList(); 
    return Results.Ok(new TuberDriverDTO
    {
        Id = employee.Id,
        Name = employee.Name,
        TuberDeliveries = deliveries.Select(d => new TuberOrderDTO
        {
            Id = d.Id,
            OrderPlacedOnDate = d.OrderPlacedOnDate,
            CustomerId = d.CustomerId,
            TuberDriverId = d.TuberDriverId,
            DeliveredOnDate = d.DeliveredOnDate
        }).ToList()
    });
});






////////////////////////////////////////
app.Run();
//don't touch or move this!
public partial class Program { }