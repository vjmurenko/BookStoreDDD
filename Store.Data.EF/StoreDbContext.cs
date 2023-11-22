using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Store.Data.EF;

public class StoreDbContext : DbContext
{
    public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        BuildBooks(modelBuilder);
        BuildOrders(modelBuilder);
        BuildOrderItems(modelBuilder);
    }

    private static void BuildBooks(ModelBuilder builder)
    {
        builder.Entity<BookDto>(model =>
        {
            model.Property(dto => dto.Isbn)
            .HasMaxLength(17)
            .IsRequired();

            model.Property(dto => dto.Price)
            .HasColumnType("money");

            model.Property(dto => dto.Title)
            .IsRequired();

            model.HasData(
            new BookDto()
            {
                Id = 1,
                Title = "True",
                Author = "Kevin",
                Description = "some book",
                Isbn = "ISBN 12345-12345",
                Price = 20m
            },
            new BookDto()
            {
                Id = 2,
                Title = "Forewer",
                Author = "Justin",
                Description = "anouther book",
                Isbn = "ISBN 12345-12346",
                Price = 30m
            }, new BookDto()
            {
                Id = 3,
                Title = "False",
                Author = "Jack",
                Description = "third book",
                Isbn = "ISBN 12345-67891",
                Price = 10m
            });
        });
    }

    private static void BuildOrders(ModelBuilder builder)
    {
        builder.Entity<OrderDto>(model =>
        {
            model.Property(dto => dto.DeliveryServiceName)
            .HasMaxLength(40);

            model.Property(dto => dto.DeliveryPrice)
            .HasColumnType("money");

            model.Property(dto => dto.PaymentServiceName)
            .HasMaxLength(40);

            model.Property(dto => dto.PhoneNumber)
            .HasMaxLength(20);

            model.Property(dto => dto.PaymentParameters)
            .HasConversion(
                value => JsonConvert.SerializeObject(value),
                value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value))
            .Metadata.SetValueComparer(DictionaryComparer);

            model.Property(dto => dto.DeliveryParameters)
            .HasConversion(
                value => JsonConvert.SerializeObject(value),
                value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value))
            .Metadata.SetValueComparer(DictionaryComparer);

        });
    }

    private static readonly ValueComparer DictionaryComparer =
    new ValueComparer<Dictionary<string, string>>(
        (dictionary1, dictionary2) => dictionary1.SequenceEqual(dictionary2),
        dictionary => dictionary.Aggregate(
            0,
            (a, p) => HashCode.Combine(HashCode.Combine(a, p.Key.GetHashCode()), p.Value.GetHashCode())
        )
    );

    private static void BuildOrderItems(ModelBuilder builder)
    {

        builder.Entity<OrderItemDto>(model =>
        {
            model.Property(m => m.Price)
            .HasColumnType("money");

            model
            .HasOne(dto => dto.OrderDto)
            .WithMany(m => m.Items)
            .IsRequired();
        });
    }

}