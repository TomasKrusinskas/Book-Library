using BookLibrary.Auth.Model;
using BookLibrary.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BookLibrary.Data
{
    public class BookLibraryDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookRating> BookRatings { get; set; }
        public DbSet<UserFavorite> UserFavorites { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public BookLibraryDbContext(DbContextOptions<BookLibraryDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=BookLibraryDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Book-Genre relationship
            builder.Entity<Book>()
                .HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.GenreId);

            // Configure Order relationships
            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Book)
                .WithMany()
                .HasForeignKey(oi => oi.BookId);

            // Seed roles
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
            );

            builder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Fiction", Description = "Fictional stories and novels" },
                new Genre { Id = 2, Name = "Non-Fiction", Description = "Real stories and informational books" },
                new Genre { Id = 3, Name = "Science Fiction", Description = "Science fiction and futuristic stories" },
                new Genre { Id = 4, Name = "Fantasy", Description = "Fantasy and magical stories" },
                new Genre { Id = 5, Name = "Mystery", Description = "Mystery and detective stories" },
                new Genre { Id = 6, Name = "Biography", Description = "Life stories of notable people" },
                new Genre { Id = 7, Name = "History", Description = "Books about historical events and people" },
                new Genre { Id = 8, Name = "Horror", Description = "Scary and supernatural stories" },
                new Genre { Id = 9, Name = "Romance", Description = "Love stories and romantic novels" },
                new Genre { Id = 10, Name = "Self-Help", Description = "Books for personal development" }
            );

            // Add random prices to existing books
            var random = new Random();
            builder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "1984", Author = "George Orwell", ISBN = "978-0452284234", Description = "Dystopian novel", PublicationYear = 1949, GenreId = 1, PhotoUrl = "https://covers.openlibrary.org/b/id/7222246-L.jpg", Price = 12.99m },
                new Book { Id = 2, Title = "The Hobbit", Author = "J.R.R. Tolkien", ISBN = "978-0547928227", Description = "Fantasy adventure", PublicationYear = 1937, GenreId = 4, PhotoUrl = "https://covers.openlibrary.org/b/id/6979861-L.jpg", Price = 15.99m },
                new Book { Id = 3, Title = "A Brief History of Time", Author = "Stephen Hawking", ISBN = "978-0553380163", Description = "Cosmology explained for the general public", PublicationYear = 1988, GenreId = 2, PhotoUrl = "https://covers.openlibrary.org/b/id/240726-L.jpg", Price = 18.99m },
                new Book { Id = 4, Title = "The Martian", Author = "Andy Weir", ISBN = "978-0553418026", Description = "Survival on Mars", PublicationYear = 2011, GenreId = 3, PhotoUrl = "https://covers.openlibrary.org/b/id/8375046-L.jpg", Price = 14.99m },
                new Book { Id = 5, Title = "The Da Vinci Code", Author = "Dan Brown", ISBN = "978-0307474278", Description = "Mystery thriller novel", PublicationYear = 2003, GenreId = 5, PhotoUrl = "https://covers.openlibrary.org/b/id/295577-L.jpg", Price = 13.99m },
                new Book { Id = 6, Title = "To Kill a Mockingbird", Author = "Harper Lee", ISBN = "978-0060935467", Description = "Classic of modern American literature", PublicationYear = 1960, GenreId = 1, PhotoUrl = "https://covers.openlibrary.org/b/id/8228691-L.jpg", Price = 11.99m },
                new Book { Id = 7, Title = "Sapiens: A Brief History of Humankind", Author = "Yuval Noah Harari", ISBN = "978-0062316097", Description = "History of humankind", PublicationYear = 2011, GenreId = 2, PhotoUrl = "https://covers.openlibrary.org/b/id/8167896-L.jpg", Price = 16.99m },
                new Book { Id = 8, Title = "Dune", Author = "Frank Herbert", ISBN = "978-0441172719", Description = "Epic science fiction novel", PublicationYear = 1965, GenreId = 3, PhotoUrl = "https://covers.openlibrary.org/b/id/8101356-L.jpg", Price = 17.99m },
                new Book { Id = 9, Title = "Harry Potter and the Sorcerer's Stone", Author = "J.K. Rowling", ISBN = "978-0590353427", Description = "First book in the Harry Potter series", PublicationYear = 1997, GenreId = 4, PhotoUrl = "https://covers.openlibrary.org/b/id/7984916-L.jpg", Price = 19.99m },
                new Book { Id = 10, Title = "Gone Girl", Author = "Gillian Flynn", ISBN = "978-0307588371", Description = "Psychological thriller and mystery", PublicationYear = 2012, GenreId = 5, PhotoUrl = "https://covers.openlibrary.org/b/id/8231856-L.jpg", Price = 14.99m },
                new Book { Id = 11, Title = "Steve Jobs", Author = "Walter Isaacson", ISBN = "978-1451648539", Description = "Biography of Steve Jobs", PublicationYear = 2011, GenreId = 6, PhotoUrl = "https://covers.openlibrary.org/b/id/7279251-L.jpg", Price = 20.99m },
                new Book { Id = 12, Title = "The Diary of a Young Girl", Author = "Anne Frank", ISBN = "978-0553296983", Description = "Diary of Anne Frank during WWII", PublicationYear = 1947, GenreId = 7, PhotoUrl = "https://covers.openlibrary.org/b/id/8225631-L.jpg", Price = 12.99m },
                new Book { Id = 13, Title = "It", Author = "Stephen King", ISBN = "978-1501142970", Description = "Horror novel about a shapeshifting entity", PublicationYear = 1986, GenreId = 8, PhotoUrl = "https://covers.openlibrary.org/b/id/8231996-L.jpg", Price = 18.99m },
                new Book { Id = 14, Title = "Pride and Prejudice", Author = "Jane Austen", ISBN = "978-1503290563", Description = "Classic romance novel", PublicationYear = 1813, GenreId = 9, PhotoUrl = "https://covers.openlibrary.org/b/id/8226191-L.jpg", Price = 10.99m },
                new Book { Id = 15, Title = "The 7 Habits of Highly Effective People", Author = "Stephen R. Covey", ISBN = "978-0743269513", Description = "Self-help classic", PublicationYear = 1989, GenreId = 10, PhotoUrl = "https://covers.openlibrary.org/b/id/240726-S.jpg", Price = 22.99m },
                new Book { Id = 16, Title = "Brave New World", Author = "Aldous Huxley", ISBN = "978-0060850524", Description = "Dystopian science fiction", PublicationYear = 1932, GenreId = 1, PhotoUrl = "https://covers.openlibrary.org/b/id/8775116-L.jpg", Price = 13.99m },
                new Book { Id = 17, Title = "Educated", Author = "Tara Westover", ISBN = "978-0399590504", Description = "Memoir of a woman who leaves her survivalist family", PublicationYear = 2018, GenreId = 6, PhotoUrl = "https://covers.openlibrary.org/b/id/9254156-L.jpg", Price = 16.99m },
                new Book { Id = 18, Title = "The Guns of August", Author = "Barbara W. Tuchman", ISBN = "978-0345476098", Description = "History of the start of WWI", PublicationYear = 1962, GenreId = 7, PhotoUrl = "https://covers.openlibrary.org/b/id/240726-M.jpg", Price = 19.99m },
                new Book { Id = 19, Title = "Dracula", Author = "Bram Stoker", ISBN = "978-0486411095", Description = "Classic horror novel", PublicationYear = 1897, GenreId = 8, PhotoUrl = "https://covers.openlibrary.org/b/id/8231851-L.jpg", Price = 11.99m },
                new Book { Id = 20, Title = "Me Before You", Author = "Jojo Moyes", ISBN = "978-0143124542", Description = "Romantic drama novel", PublicationYear = 2012, GenreId = 9, PhotoUrl = "https://covers.openlibrary.org/b/id/8231852-L.jpg", Price = 14.99m },
                new Book { Id = 21, Title = "Atomic Habits", Author = "James Clear", ISBN = "978-0735211292", Description = "Guide to building good habits", PublicationYear = 2018, GenreId = 10, PhotoUrl = "https://covers.openlibrary.org/b/id/9254157-L.jpg", Price = 24.99m },
                new Book { Id = 22, Title = "Animal Farm", Author = "George Orwell", ISBN = "978-0451526342", Description = "Political satire", PublicationYear = 1945, GenreId = 1, PhotoUrl = "https://covers.openlibrary.org/b/id/7222246-M.jpg", Price = 9.99m },
                new Book { Id = 23, Title = "Becoming", Author = "Michelle Obama", ISBN = "978-1524763138", Description = "Memoir of the former First Lady", PublicationYear = 2018, GenreId = 6, PhotoUrl = "https://covers.openlibrary.org/b/id/9254158-L.jpg", Price = 25.99m },
                new Book { Id = 24, Title = "SPQR: A History of Ancient Rome", Author = "Mary Beard", ISBN = "978-1631492228", Description = "History of Rome", PublicationYear = 2015, GenreId = 7, PhotoUrl = "https://covers.openlibrary.org/b/id/9254159-L.jpg", Price = 28.99m },
                new Book { Id = 25, Title = "The Shining", Author = "Stephen King", ISBN = "978-0307743657", Description = "Horror at a haunted hotel", PublicationYear = 1977, GenreId = 8, PhotoUrl = "https://covers.openlibrary.org/b/id/8231997-L.jpg", Price = 17.99m },
                new Book { Id = 26, Title = "The Notebook", Author = "Nicholas Sparks", ISBN = "978-0446605236", Description = "Romantic drama", PublicationYear = 1996, GenreId = 9, PhotoUrl = "https://covers.openlibrary.org/b/id/8231853-L.jpg", Price = 13.99m },
                new Book { Id = 27, Title = "Man's Search for Meaning", Author = "Viktor E. Frankl", ISBN = "978-0807014295", Description = "Psychological memoir", PublicationYear = 1946, GenreId = 10, PhotoUrl = "https://covers.openlibrary.org/b/id/9254160-L.jpg", Price = 15.99m },
                new Book { Id = 28, Title = "Fahrenheit 451", Author = "Ray Bradbury", ISBN = "978-1451673319", Description = "Dystopian novel about book burning", PublicationYear = 1953, GenreId = 1, PhotoUrl = "https://covers.openlibrary.org/b/id/9254161-L.jpg", Price = 12.99m },
                new Book { Id = 29, Title = "Long Walk to Freedom", Author = "Nelson Mandela", ISBN = "978-0316548182", Description = "Autobiography of Nelson Mandela", PublicationYear = 1994, GenreId = 6, PhotoUrl = "https://covers.openlibrary.org/b/id/9254162-L.jpg", Price = 21.99m },
                new Book { Id = 30, Title = "The Wright Brothers", Author = "David McCullough", ISBN = "978-1476728759", Description = "Biography of the Wright brothers", PublicationYear = 2015, GenreId = 7, PhotoUrl = "https://covers.openlibrary.org/b/id/9254163-L.jpg", Price = 23.99m },
                new Book { Id = 31, Title = "The Haunting of Hill House", Author = "Shirley Jackson", ISBN = "978-0143039983", Description = "Classic haunted house horror", PublicationYear = 1959, GenreId = 8, PhotoUrl = "https://covers.openlibrary.org/b/id/8231998-L.jpg", Price = 14.99m },
                new Book { Id = 32, Title = "Outlander", Author = "Diana Gabaldon", ISBN = "978-0440212560", Description = "Time-travel romance", PublicationYear = 1991, GenreId = 9, PhotoUrl = "https://covers.openlibrary.org/b/id/8231854-L.jpg", Price = 18.99m },
                new Book { Id = 33, Title = "The Power of Now", Author = "Eckhart Tolle", ISBN = "978-1577314806", Description = "Spiritual self-help book", PublicationYear = 1997, GenreId = 10, PhotoUrl = "https://covers.openlibrary.org/b/id/9254164-L.jpg", Price = 20.99m },
                new Book { Id = 34, Title = "Lord of the Flies", Author = "William Golding", ISBN = "978-0399501487", Description = "Novel about boys stranded on an island", PublicationYear = 1954, GenreId = 1, PhotoUrl = "https://covers.openlibrary.org/b/id/8228692-L.jpg", Price = 11.99m },
                new Book { Id = 35, Title = "Alexander Hamilton", Author = "Ron Chernow", ISBN = "978-0143034759", Description = "Biography of Alexander Hamilton", PublicationYear = 2004, GenreId = 6, PhotoUrl = "https://covers.openlibrary.org/b/id/9254165-L.jpg", Price = 26.99m },
                new Book { Id = 36, Title = "Team of Rivals", Author = "Doris Kearns Goodwin", ISBN = "978-0743270755", Description = "History of Abraham Lincoln's cabinet", PublicationYear = 2005, GenreId = 7, PhotoUrl = "https://covers.openlibrary.org/b/id/9254166-L.jpg", Price = 27.99m },
                new Book { Id = 37, Title = "The Exorcist", Author = "William Peter Blatty", ISBN = "978-0061007224", Description = "Horror novel about demonic possession", PublicationYear = 1971, GenreId = 8, PhotoUrl = "https://covers.openlibrary.org/b/id/8231999-L.jpg", Price = 16.99m },
                new Book { Id = 38, Title = "The Fault in Our Stars", Author = "John Green", ISBN = "978-0142424179", Description = "Young adult romance", PublicationYear = 2012, GenreId = 9, PhotoUrl = "https://covers.openlibrary.org/b/id/8231855-L.jpg", Price = 15.99m },
                new Book { Id = 39, Title = "Mindset: The New Psychology of Success", Author = "Carol S. Dweck", ISBN = "978-0345472328", Description = "Self-help on growth mindset", PublicationYear = 2006, GenreId = 10, PhotoUrl = "https://covers.openlibrary.org/b/id/9254167-L.jpg", Price = 19.99m },
                new Book { Id = 40, Title = "The Catcher in the Rye", Author = "J.D. Salinger", ISBN = "978-0316769488", Description = "Classic coming-of-age novel", PublicationYear = 1951, GenreId = 1, PhotoUrl = "https://covers.openlibrary.org/b/id/8228693-L.jpg", Price = 12.99m },
                new Book { Id = 41, Title = "Einstein: His Life and Universe", Author = "Walter Isaacson", ISBN = "978-0743264730", Description = "Biography of Albert Einstein", PublicationYear = 2007, GenreId = 6, PhotoUrl = "https://covers.openlibrary.org/b/id/9254168-L.jpg", Price = 24.99m },
                new Book { Id = 42, Title = "Guns, Germs, and Steel", Author = "Jared Diamond", ISBN = "978-0393317558", Description = "History of societies", PublicationYear = 1997, GenreId = 7, PhotoUrl = "https://covers.openlibrary.org/b/id/9254169-L.jpg", Price = 21.99m },
                new Book { Id = 43, Title = "Pet Sematary", Author = "Stephen King", ISBN = "978-1501156700", Description = "Horror about a cursed burial ground", PublicationYear = 1983, GenreId = 8, PhotoUrl = "https://covers.openlibrary.org/b/id/8232000-L.jpg", Price = 17.99m },
                new Book { Id = 44, Title = "The Wedding Date", Author = "Jasmine Guillory", ISBN = "978-0399587665", Description = "Contemporary romance", PublicationYear = 2018, GenreId = 9, PhotoUrl = "https://covers.openlibrary.org/b/id/9254170-L.jpg", Price = 14.99m },
                new Book { Id = 45, Title = "The Subtle Art of Not Giving a F*ck", Author = "Mark Manson", ISBN = "978-0062457714", Description = "Self-help on living a good life", PublicationYear = 2016, GenreId = 10, PhotoUrl = "https://covers.openlibrary.org/b/id/9254171-L.jpg", Price = 18.99m },
                new Book { Id = 46, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", ISBN = "978-0743273565", Description = "Classic American novel", PublicationYear = 1925, GenreId = 1, PhotoUrl = "https://covers.openlibrary.org/b/id/7222276-L.jpg", Price = 10.99m },
                new Book { Id = 47, Title = "Unbroken", Author = "Laura Hillenbrand", ISBN = "978-0812974492", Description = "Biography of a WWII hero", PublicationYear = 2010, GenreId = 6, PhotoUrl = "https://covers.openlibrary.org/b/id/9254172-L.jpg", Price = 22.99m },
                new Book { Id = 48, Title = "The Immortal Life of Henrietta Lacks", Author = "Rebecca Skloot", ISBN = "978-1400052189", Description = "Biography and science history", PublicationYear = 2010, GenreId = 7, PhotoUrl = "https://covers.openlibrary.org/b/id/9254173-L.jpg", Price = 20.99m },
                new Book { Id = 49, Title = "Bird Box", Author = "Josh Malerman", ISBN = "978-0062259653", Description = "Post-apocalyptic horror", PublicationYear = 2014, GenreId = 8, PhotoUrl = "https://covers.openlibrary.org/b/id/9254174-L.jpg", Price = 15.99m },
                new Book { Id = 50, Title = "Red, White & Royal Blue", Author = "Casey McQuiston", ISBN = "978-1250316776", Description = "Romantic comedy novel", PublicationYear = 2019, GenreId = 9, PhotoUrl = "https://covers.openlibrary.org/b/id/9254175-L.jpg", Price = 16.99m }
            );

            builder.Entity<BookRating>()
                .HasKey(br => br.Id);
            builder.Entity<BookRating>()
                .HasOne(br => br.Book)
                .WithMany()
                .HasForeignKey(br => br.BookId);
            builder.Entity<BookRating>()
                .HasOne(br => br.User)
                .WithMany()
                .HasForeignKey(br => br.UserId);
            builder.Entity<BookRating>()
                .HasIndex(br => new { br.BookId, br.UserId }).IsUnique();

            // Configure Book-BookRating relationship
            builder.Entity<Book>()
                .HasMany(b => b.BookRatings)
                .WithOne(br => br.Book)
                .HasForeignKey(br => br.BookId);

            // Configure UserFavorite relationships
            builder.Entity<UserFavorite>()
                .HasKey(uf => uf.Id);
            builder.Entity<UserFavorite>()
                .HasOne(uf => uf.User)
                .WithMany()
                .HasForeignKey(uf => uf.UserId);
            builder.Entity<UserFavorite>()
                .HasOne(uf => uf.Book)
                .WithMany()
                .HasForeignKey(uf => uf.BookId);
            builder.Entity<UserFavorite>()
                .HasIndex(uf => new { uf.BookId, uf.UserId }).IsUnique();
        }
    }
}