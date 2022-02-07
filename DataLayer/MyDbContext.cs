using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        { }

        #region DbSets
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<InfoRequest> InfoRequests { get; set; }
        public DbSet<InfoRequestReply> InfoRequestReplies { get; set; }
        public DbSet<Nation> Nations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasQueryFilter(x => !x.IsDeleted);
                //map to table name
                entity.ToTable("Product");
                //map primary key
                entity.HasKey(k => k.Id);

                //Configure NotNull Column
                entity.Property(p => p.BrandId).IsRequired();

                //Configure length Column
                entity.Property(p => p.ShortDescription)
                     .HasMaxLength(20);  //.IsFixedLength(); from varchar to char


                entity.HasOne(p => p.Brand).WithMany(b => b.Products).HasForeignKey(fk => fk.BrandId); //foreign key
                                                                                                       //ha una relazione con <Brand> {tramite la proprieta (=> X) di Prod} su più prodotti {tramite la proprietà (=>Y) di brand}.product Possiede FK BrandID
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");
                entity.HasQueryFilter(x => !x.IsDeleted);

                entity.HasOne(u => u.User).WithOne(a => a.Account).HasForeignKey<User>(fk => fk.AccountId);
                entity.HasOne(u => u.Brand).WithOne(b => b.Account).HasForeignKey<Brand>(fk => fk.AccountId);

                entity.HasMany(u => u.InfoRequestReplies).WithOne(x => x.Account).HasForeignKey(fk => fk.AccountId);

                entity.Property(p => p.Password).IsRequired();
                entity.Property(p => p.Email).IsRequired();
                entity.Property(p => p.AccountType).IsRequired();
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");
                entity.HasQueryFilter(x => !x.IsDeleted);

                entity.Property(p => p.AccountId).IsRequired();
                entity.Property(p => p.BrandName).IsRequired();

            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasQueryFilter(x => !x.IsDeleted);

                entity.Property(p => p.Name).IsRequired();
            });

            modelBuilder.Entity<InfoRequest>(entity =>
            {
                entity.ToTable("InfoRequest");
                entity.HasQueryFilter(x => !x.IsDeleted);


                entity.HasMany(x => x.InfoRequestReplies).WithOne(x => x.InfoRequest).HasForeignKey(fk => fk.InfoRequestId);
                entity.HasOne(x => x.Nation).WithMany(x => x.InfoRequests).HasForeignKey(fk => fk.NationId);
                entity.HasOne(x => x.Product).WithMany(x => x.InfoRequests).HasForeignKey(fk => fk.ProductId);
                entity.HasOne(x => x.User).WithMany(x => x.InfoRequests).HasForeignKey(fk => fk.UserId);

                entity.Property(p => p.PhoneNumber).IsRequired();
            });

            modelBuilder.Entity<InfoRequestReply>(entity =>
            {
                entity.ToTable("InfoRequestReply");
                entity.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<Nation>(entity =>
            {
                entity.ToTable("Nation");
                entity.HasQueryFilter(x => !x.IsDeleted);

                entity.Property(p => p.Name).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasQueryFilter(x => !x.IsDeleted);

                entity.Property(p => p.Name).IsRequired();
                entity.Property(p => p.LastName).IsRequired();
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("ProductCategory");

                entity.HasKey(k => new { k.IdCategory, k.IdProduct });
                entity.Property(x => x.IdProduct).HasColumnName("IdProduct");
                entity.Property(x => x.IdCategory).HasColumnName("IdCategory");

                entity.HasOne(x => x.Category).WithMany(x => x.ProductCategory).HasForeignKey(fk => fk.IdCategory);
                entity.HasOne(x => x.Product).WithMany(x => x.ProductCategory).HasForeignKey(fk => fk.IdProduct);
            });

        }
    }
}
