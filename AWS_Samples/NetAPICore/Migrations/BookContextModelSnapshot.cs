// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetAPICore.Data;

#nullable disable

namespace NetAPICore.Migrations
{
    [DbContext(typeof(BookContext))]
    partial class BookContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("NetAPICore.Entities.Author", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuthorId"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AuthorId");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            AuthorId = 1,
                            Name = "Sanjay Gupta"
                        },
                        new
                        {
                            AuthorId = 2,
                            Name = "Vince Kelly"
                        },
                        new
                        {
                            AuthorId = 3,
                            Name = "Minda Harts"
                        },
                        new
                        {
                            AuthorId = 4,
                            Name = "Susanne Tedrick"
                        });
                });

            modelBuilder.Entity("NetAPICore.Entities.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookId"), 1L, 1);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BookId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            BookId = 1,
                            AuthorId = 1,
                            Title = "Keep Sharp"
                        },
                        new
                        {
                            BookId = 2,
                            AuthorId = 1,
                            Title = "Healthy Habits"
                        },
                        new
                        {
                            BookId = 3,
                            AuthorId = 2,
                            Title = "Call of Duty"
                        },
                        new
                        {
                            BookId = 4,
                            AuthorId = 3,
                            Title = "Women of Color in Tech"
                        },
                        new
                        {
                            BookId = 5,
                            AuthorId = 4,
                            Title = "No books"
                        });
                });

            modelBuilder.Entity("NetAPICore.Entities.Book", b =>
                {
                    b.HasOne("NetAPICore.Entities.Author", null)
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NetAPICore.Entities.Author", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
