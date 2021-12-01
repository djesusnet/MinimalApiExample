using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["SqlConnection:SqlConnectionString"];

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddTransient<CustomerRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/customers", ([FromServicesAttribute] CustomerRepository customerRepository) => customerRepository.GetAll())
    .WithName("GetAllCustomers");

app.MapGet("/customers/{id}", ([FromServicesAttribute] CustomerRepository customerRepository, int id) =>
    {
        var customer = customerRepository.GetById(id);
        return customer is not null ? Results.Ok(customer) : Results.NotFound();
    })
    .WithName("GetCustomerById");

app.MapPost("/customers", ([FromServicesAttribute] CustomerRepository customerRepository, Customer customer) =>
    {
        customerRepository.Insert(customer);
        return Results.Created($"/customers/{customer.Id}", customer);
    })
    .WithName("InsertCustomer");


app.MapPut("/customers", ([FromServicesAttribute] CustomerRepository customerRepository, Customer customer) =>
    {
        customerRepository.Update(customer);
        return Results.Ok(customer);
    })
    .WithName("UpdateCustomer");

app.MapDelete("/customers/{id}", ([FromServicesAttribute] CustomerRepository customerRepository, int id) =>
    {
        customerRepository.Delete(id);
        return Results.Ok($"Usuário - {id} foi excluido com sucesso");
    })
    .WithName("DeleteCustomer");


app.Run();


class CustomerRepository
{

    private readonly ApplicationDbContext _applicationDbContext;
    private DbSet<Customer> customers;

    public CustomerRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        customers = _applicationDbContext.Set<Customer>();
    }

    public void Delete(int id)
    {
        var customer = GetById(id);

        if (customer == null)
        {
            throw new ArgumentNullException("customer");
        }
        this.customers.Remove(customer);
        _applicationDbContext.SaveChanges();
    }

    public Customer GetById(int Id)
    {
        return customers.SingleOrDefault(c => c.Id == Id);
    }

    public IEnumerable<Customer> GetAll()
    {
        return customers.AsEnumerable();
    }

    public void Insert(Customer customer)
    {
        if (customer == null)
        {
            throw new ArgumentNullException("customer");
        }
        customers.Add(customer);
        _applicationDbContext.SaveChanges();
    }



    public void Update(Customer customer)
    {
        if (customer == null)
        {
            throw new ArgumentNullException("customer");
        }
        customers.Update(customer);
        _applicationDbContext.SaveChanges();
    }
}



class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerMap());
        base.OnModelCreating(modelBuilder);
    }
}

class CustomerMap : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id)
            .HasName("PK_CUSTOMER_ID");

        builder.Property(c => c.Id).ValueGeneratedOnAdd()
            .HasColumnName("ID")
            .HasColumnType("INT");

        builder.Property(c => c.CustomerName)
            .HasColumnName("CUSTOMER_NAME")
            .HasColumnType("NVARCHAR(50)")
            .IsRequired();

        builder.Property(c => c.PurchasesProduct)
            .HasColumnName("PURCHASED_PRODUCT")
            .HasColumnType("NVARCHAR(100)")
            .IsRequired();

        builder.Property(c => c.PaymentType)
            .HasColumnName("PAYMENT_TYPE")
            .HasColumnType("NVARCHAR(50)")
            .IsRequired();

        builder.Property(c => c.CreateDate)
            .HasColumnName("CREATED_DATE")
            .HasColumnType("DATETIME")
            .IsRequired();

        builder.Property(c => c.ModifiedDate)
            .HasColumnName("MODIFIED_DATE")
            .HasColumnType("DATETIME")
            .IsRequired();

        builder.Property(c => c.IsActive)
            .HasColumnName("IS_ACTIVE")
            .HasColumnType("BIT")
            .IsRequired();

    }
}

class Customer : BaseEntity
{
    public string CustomerName { get; set; }
    public string PurchasesProduct { get; set; }
    public string PaymentType { get; set; }
}

class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public bool IsActive { get; set; }
}