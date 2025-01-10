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
app.MapGet("/tuberorders/{id}", (int id) => 
{
    TuberOrder order = tuberOrders.FirstOrDefault(to => to.Id == id);
    if (order == null)
    {
        return Results.NotFound();
    }
    TuberDriver employee = tuberDrivers.FirstOrDefault(td => td.Id == order.TuberDriverId);
    Customer customer = customers.FirstOrDefault(c => c.Id == order.CustomerId);
    List<TuberTopping> tuberTopping = tuberToppings.Where(tt => tt.TuberOrderId == id).ToList();
    List<Topping> topping = [];
    foreach (TuberTopping tt in tuberTopping)
    {
        Topping toppingToAdd = toppings.FirstOrDefault(t => tt.ToppingId == t.Id);
        topping.Add(toppingToAdd);
    }
    return Results.Ok(new TuberOrderDTO
    {
        Id = order.Id,
        OrderPlacedOnDate = order.OrderPlacedOnDate,
        CustomerId = order.CustomerId,
        Customer = customer == null ? null : new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address
        },
        TuberDriverId = order.TuberDriverId,
        TuberDriver = employee == null ? null : new TuberDriverDTO
        {
            Id = employee.Id,
            Name = employee.Name,
        },
        DeliveredOnDate = order.DeliveredOnDate,
        Toppings = topping.Select(t => new ToppingDTO
        {
            Id = t.Id,
            Name = t.Name
        }).ToList()
    });
});

//endpoint for submitting a new order
app.MapPost("/tuberorders", (TuberOrder order) => 
{
    order.Id = tuberOrders.Max(to => to.Id) + 1;
    order.OrderPlacedOnDate = DateTime.Now;
    tuberOrders.Add(order);
    Customer customer = customers.FirstOrDefault(c => c.Id == order.CustomerId);

    return Results.Created($"/tuberorders/{order.Id}", new TuberOrderDTO
    {
        Id = order.Id,
        OrderPlacedOnDate = order.OrderPlacedOnDate,
        CustomerId = order.CustomerId,
        Customer = customer == null ? null : new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address
        }
    });
});

//endpoint for assigning a driver to an order
app.MapPut("/tuberorders/{id}", (int id, TuberOrder tuberOrder) => 
{
    TuberOrder orderToUpdate = tuberOrders.FirstOrDefault(to => to.Id == id);
    if (orderToUpdate == null)
    {
        return Results.NotFound();
    }
    orderToUpdate.TuberDriverId = tuberOrder.TuberDriverId;
    return Results.NoContent();
});


//endpoint for completing an order
app.MapPost("/tuberorders/{id}/complete", (int id) => 
{
    TuberOrder orderToComplete = tuberOrders.FirstOrDefault(to => to.Id == id);
    orderToComplete.DeliveredOnDate = DateTime.Now;
    return Results.NoContent();
});


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
app.MapPost("/tubertoppings", (TuberTopping tuberTopping) => 
{
    tuberTopping.Id = tuberToppings.Max(tt => tt.Id) + 1;
    tuberToppings.Add(tuberTopping);

    return Results.Created($"/tubertoppings/{tuberTopping.Id}", new TuberToppingDTO
    {
        Id = tuberTopping.Id,
        TuberOrderId = tuberTopping.TuberOrderId,
        ToppingId = tuberTopping.ToppingId
    });
});

//endpoint for removing a topping from a tuberorder
app.MapDelete("/tubertoppings/{id}", (int id) => 
{
    TuberTopping toppingToDelete = tuberToppings.FirstOrDefault(tt => tt.Id == id);
    if (toppingToDelete == null)
    {
        return Results.NotFound();
    }
    tuberToppings.Remove(toppingToDelete);
    return Results.NoContent();
});


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
app.MapPost("/customers", (Customer customer) => 
{
    customer.Id = customers.Max(c => c.Id) + 1;
    customers.Add(customer);
    return Results.Created($"/customers/{customer.Id}", new CustomerDTO
    {
        Id = customer.Id,
        Name = customer.Name,
        Address = customer.Address
    });
});

//endpoint for deleting a customer
app.MapDelete("/customers/{id}", (int id) => 
{
    Customer customerToDelete = customers.FirstOrDefault(c => c.Id == id);
    if (customerToDelete == null)
    {
        return Results.NotFound();
    }
    if (customerToDelete.Id != id)
    {
        return Results.BadRequest();
    }
    customers.Remove(customerToDelete);
    return Results.NoContent();
});


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