using Microsoft.EntityFrameworkCore;


namespace EFWebSiteTest
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<InfoRequest> InfoRequests { get; set; }
        public DbSet<InfoRequestReply> InfoRequestReplies { get; set; }
        public DbSet<Nation> Nations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
            modelBuilder.Entity<Product>().Map(m =>
            {
                m.Properties(p => new { p.StudentId, p.StudentName });
                m.ToTable("StudentInfo");
            }).Map(m => {
                m.Properties(p => new { p.StudentId, p.Height, p.Weight, p.Photo, p.DateOfBirth });
                m.ToTable("StudentInfoDetail");
            });*/
            //modelBuilder.Entity<Product>().Property(p => p.Id).HasColumnName("Id");


            modelBuilder.Entity<Product>(entity =>
            {

                //map to table name
                entity.ToTable("Product");
                //map primary key
                entity.HasKey(k => k.Id);

                //Configure NotNull Column
                entity
                .Property(p => p.BrandId)
                .IsRequired();

                //Configure length Column
                entity.Property(p => p.ShortDescription)
                     .HasMaxLength(20);  //.IsFixedLength(); from varchar to char


                entity.HasOne<Brand>(p => p.Brand).WithMany(b => b.Products).HasForeignKey(fk=>fk.BrandId); //foreign key
      //ha una relazione con <Brand> {tramite la proprieta (=> X) di Prod} su più prodotti {tramite la proprietà (=>Y) di brand}
                
                //entity.HasMany(n => n.ProductCategory).WithOne().HasForeignKey(fk => fk.IdProduct); alternativa
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.HasOne(u => u.User).WithOne(a=> a.Account).HasForeignKey<User>( fk => fk.AccountId);
                entity.HasOne(u => u.Brand).WithOne(b=> b.Account).HasForeignKey<Brand>(fk=> fk.AccountId);

                entity.HasMany(u=> u.InfoRequestReplies).WithOne(x=> x.Account).HasForeignKey(fk=>fk.AccountId);

            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");

                //entity.HasMany(x=>x.Products).WithOne(x=>x.Brand);//  Brand - Product
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

            });

            modelBuilder.Entity<InfoRequest>(entity =>
            {
                entity.ToTable("InfoRequest");

                entity.HasMany(x=>x.InfoRequestReplies).WithOne(x=>x.InfoRequest).HasForeignKey(fk => fk.InfoRequestId);  
                entity.HasOne(x=>x.Nation).WithMany().HasForeignKey(fk=> fk.NationId);
                entity.HasOne(x=>x.Product).WithMany(x=>x.InfoRequests).HasForeignKey(fk=> fk.ProductId);
                entity.HasOne(x => x.User).WithMany().HasForeignKey(fk => fk.UserId);
            });

            modelBuilder.Entity<InfoRequestReply>(entity =>
            {
                entity.ToTable("InfoRequestReply");

            });
            modelBuilder.Entity<Nation>(entity =>
            {
                entity.ToTable("Nation");

            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("ProductCategory");
                
                entity.HasKey(k => new { k.IdCategory, k.IdProduct });
                entity.Property(x=> x.IdProduct).HasColumnName("IdProduct");
                entity.Property(x => x.IdCategory).HasColumnName("IdCategory");



                //entity.HasOne(x => x.Category).WithMany(y => y.ProductCategory);
                //entity.HasOne(x => x.Product).WithMany(y => y.ProductCategory);
                entity.HasOne(x => x.Category).WithMany(x=>x.ProductCategory).HasForeignKey(fk => fk.IdCategory);
                entity.HasOne(x => x.Product).WithMany(x => x.ProductCategory).HasForeignKey(fk => fk.IdProduct);
            });


            /*
            product - category  X
            Account - User      X
            Account - Brand     X
            Brand   - Product   X
            Request - Reply     X
            Request - Nation    X
            Request - User      X
            Request - Product   X
            Reply   - Account   X
             */
        }
    }
}
