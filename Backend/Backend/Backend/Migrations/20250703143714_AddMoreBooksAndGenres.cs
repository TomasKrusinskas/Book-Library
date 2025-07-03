using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreBooksAndGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Description", "GenreId", "ISBN", "PublicationYear", "Title" },
                values: new object[,]
                {
                    { 3, "Stephen Hawking", "Cosmology explained for the general public", 2, "978-0553380163", 1988, "A Brief History of Time" },
                    { 4, "Andy Weir", "Survival on Mars", 3, "978-0553418026", 2011, "The Martian" },
                    { 5, "Dan Brown", "Mystery thriller novel", 5, "978-0307474278", 2003, "The Da Vinci Code" },
                    { 6, "Harper Lee", "Classic of modern American literature", 1, "978-0060935467", 1960, "To Kill a Mockingbird" },
                    { 7, "Yuval Noah Harari", "History of humankind", 2, "978-0062316097", 2011, "Sapiens: A Brief History of Humankind" },
                    { 8, "Frank Herbert", "Epic science fiction novel", 3, "978-0441172719", 1965, "Dune" },
                    { 9, "J.K. Rowling", "First book in the Harry Potter series", 4, "978-0590353427", 1997, "Harry Potter and the Sorcerer's Stone" },
                    { 10, "Gillian Flynn", "Psychological thriller and mystery", 5, "978-0307588371", 2012, "Gone Girl" },
                    { 16, "Aldous Huxley", "Dystopian science fiction", 1, "978-0060850524", 1932, "Brave New World" },
                    { 22, "George Orwell", "Political satire", 1, "978-0451526342", 1945, "Animal Farm" },
                    { 28, "Ray Bradbury", "Dystopian novel about book burning", 1, "978-1451673319", 1953, "Fahrenheit 451" },
                    { 34, "William Golding", "Novel about boys stranded on an island", 1, "978-0399501487", 1954, "Lord of the Flies" },
                    { 40, "J.D. Salinger", "Classic coming-of-age novel", 1, "978-0316769488", 1951, "The Catcher in the Rye" },
                    { 46, "F. Scott Fitzgerald", "Classic American novel", 1, "978-0743273565", 1925, "The Great Gatsby" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 6, "Life stories of notable people", "Biography" },
                    { 7, "Books about historical events and people", "History" },
                    { 8, "Scary and supernatural stories", "Horror" },
                    { 9, "Love stories and romantic novels", "Romance" },
                    { 10, "Books for personal development", "Self-Help" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Description", "GenreId", "ISBN", "PublicationYear", "Title" },
                values: new object[,]
                {
                    { 11, "Walter Isaacson", "Biography of Steve Jobs", 6, "978-1451648539", 2011, "Steve Jobs" },
                    { 12, "Anne Frank", "Diary of Anne Frank during WWII", 7, "978-0553296983", 1947, "The Diary of a Young Girl" },
                    { 13, "Stephen King", "Horror novel about a shapeshifting entity", 8, "978-1501142970", 1986, "It" },
                    { 14, "Jane Austen", "Classic romance novel", 9, "978-1503290563", 1813, "Pride and Prejudice" },
                    { 15, "Stephen R. Covey", "Self-help classic", 10, "978-0743269513", 1989, "The 7 Habits of Highly Effective People" },
                    { 17, "Tara Westover", "Memoir of a woman who leaves her survivalist family", 6, "978-0399590504", 2018, "Educated" },
                    { 18, "Barbara W. Tuchman", "History of the start of WWI", 7, "978-0345476098", 1962, "The Guns of August" },
                    { 19, "Bram Stoker", "Classic horror novel", 8, "978-0486411095", 1897, "Dracula" },
                    { 20, "Jojo Moyes", "Romantic drama novel", 9, "978-0143124542", 2012, "Me Before You" },
                    { 21, "James Clear", "Guide to building good habits", 10, "978-0735211292", 2018, "Atomic Habits" },
                    { 23, "Michelle Obama", "Memoir of the former First Lady", 6, "978-1524763138", 2018, "Becoming" },
                    { 24, "Mary Beard", "History of Rome", 7, "978-1631492228", 2015, "SPQR: A History of Ancient Rome" },
                    { 25, "Stephen King", "Horror at a haunted hotel", 8, "978-0307743657", 1977, "The Shining" },
                    { 26, "Nicholas Sparks", "Romantic drama", 9, "978-0446605236", 1996, "The Notebook" },
                    { 27, "Viktor E. Frankl", "Psychological memoir", 10, "978-0807014295", 1946, "Man's Search for Meaning" },
                    { 29, "Nelson Mandela", "Autobiography of Nelson Mandela", 6, "978-0316548182", 1994, "Long Walk to Freedom" },
                    { 30, "David McCullough", "Biography of the Wright brothers", 7, "978-1476728759", 2015, "The Wright Brothers" },
                    { 31, "Shirley Jackson", "Classic haunted house horror", 8, "978-0143039983", 1959, "The Haunting of Hill House" },
                    { 32, "Diana Gabaldon", "Time-travel romance", 9, "978-0440212560", 1991, "Outlander" },
                    { 33, "Eckhart Tolle", "Spiritual self-help book", 10, "978-1577314806", 1997, "The Power of Now" },
                    { 35, "Ron Chernow", "Biography of Alexander Hamilton", 6, "978-0143034759", 2004, "Alexander Hamilton" },
                    { 36, "Doris Kearns Goodwin", "History of Abraham Lincoln's cabinet", 7, "978-0743270755", 2005, "Team of Rivals" },
                    { 37, "William Peter Blatty", "Horror novel about demonic possession", 8, "978-0061007224", 1971, "The Exorcist" },
                    { 38, "John Green", "Young adult romance", 9, "978-0142424179", 2012, "The Fault in Our Stars" },
                    { 39, "Carol S. Dweck", "Self-help on growth mindset", 10, "978-0345472328", 2006, "Mindset: The New Psychology of Success" },
                    { 41, "Walter Isaacson", "Biography of Albert Einstein", 6, "978-0743264730", 2007, "Einstein: His Life and Universe" },
                    { 42, "Jared Diamond", "History of societies", 7, "978-0393317558", 1997, "Guns, Germs, and Steel" },
                    { 43, "Stephen King", "Horror about a cursed burial ground", 8, "978-1501156700", 1983, "Pet Sematary" },
                    { 44, "Jasmine Guillory", "Contemporary romance", 9, "978-0399587665", 2018, "The Wedding Date" },
                    { 45, "Mark Manson", "Self-help on living a good life", 10, "978-0062457714", 2016, "The Subtle Art of Not Giving a F*ck" },
                    { 47, "Laura Hillenbrand", "Biography of a WWII hero", 6, "978-0812974492", 2010, "Unbroken" },
                    { 48, "Rebecca Skloot", "Biography and science history", 7, "978-1400052189", 2010, "The Immortal Life of Henrietta Lacks" },
                    { 49, "Josh Malerman", "Post-apocalyptic horror", 8, "978-0062259653", 2014, "Bird Box" },
                    { 50, "Casey McQuiston", "Romantic comedy novel", 9, "978-1250316776", 2019, "Red, White & Royal Blue" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
