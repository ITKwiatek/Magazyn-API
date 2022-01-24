using Magazyn_API.Model.Auth;
using Magazyn_API.Model.Order;
using Magazyn_API.Model.Order.FromExcelDto;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Magazyn_API.Data
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly string _connectionString;

        public ApplicationDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<GroupModel> Groups { get; set; }
        public DbSet<ComponentModel> Components { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ReleaseItem> ReleaseItems { get; set; }
        public DbSet<Release> Releases { get; set; }
        //public DbSet<Person> Persons { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<VirtualItem> VirtualItems { get; set; }
        public DbSet<VirtualOrder> VirtualOrders { get; set; }
        public DbSet<VirtualManyToMany> ManyToMany { get; set; }
        //public DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
{
            base.OnModelCreating(builder);
            builder.Entity<ReleaseItem>()
                .HasKey(r => new { r.ReleaseId, r.OrderItemId });
            builder.Entity<VirtualManyToMany>()
                .HasKey(r => new { r.OrderId, r.VirtualOrderId });
            builder.Entity<ApplicationUser>()
                .HasKey(p => p.Id);


            //Order Item has One Component
            builder.Entity<OrderItem>()
                .HasOne<ComponentModel>(o => o.Component)
                .WithMany()
                .HasForeignKey(i => i.ComponentId);

            //Device has one Project
            //Project has many Devices
            builder.Entity<Device>()
                .HasOne<GroupModel>(d => d.Group)
                .WithMany(p => p.Devices)
                .HasForeignKey(d => d.GroupId);


            //ReleaseItem has One OrderItem
            builder.Entity<OrderItem>()
                .HasMany<ReleaseItem>()
                .WithOne(r => r.OrderItem)
                .HasForeignKey(r => r.OrderItemId);

            //ReleaseItem has one Release
            //Release has many ReleaseItems
            builder.Entity<ReleaseItem>()
                .HasOne<Release>(ri => ri.Release)
                .WithMany(r => r.ReleaseItems)
                .HasForeignKey(ri => ri.ReleaseId)
                .OnDelete(DeleteBehavior.Restrict);

            //Release has one Receiver
            builder.Entity<Release>()
                .HasOne<ApplicationUser>(r => r.Receiver)
                .WithMany()
                .HasForeignKey(r => r.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
            //Release has one Order
            builder.Entity<Release>()
                .HasOne<OrderModel>(r => r.Order)
                .WithMany()
                .HasForeignKey(r => r.OrderId);

            //Order has one device
            //Device has many Orders
            builder.Entity<OrderModel>()
                .HasOne<Device>(o => o.Device)
                .WithMany(d => d.Orders)
                .HasForeignKey(o => o.DeviceId);
            //Order has one ConfirmedBy
            builder.Entity<OrderModel>()
                .HasOne<ApplicationUser>(o => o.ConfirmedBy)
                .WithMany()
                .HasForeignKey(o => o.ConfirmedById);
            ////Order has one Issuer
            //builder.Entity<OrderModel>()
            //    .HasOne<ApplicationUser>(o => o.Issuer)
            //    .WithMany()
            //    .HasForeignKey(o => o.IssuerId);
            ////Order has one Receiver
            //builder.Entity<OrderModel>()
            //    .HasOne<ApplicationUser>(o => o.Receiver)
            //    .WithMany()
            //    .HasForeignKey(o => o.ReceiverId);


            //Person one to many
            //Person has many ReleaseReceivers
            builder.Entity<Release>()
                .HasOne(r => r.Receiver)
                .WithMany()
                .HasForeignKey(r => r.ReceiverId);
            //builder.Entity<Person>()
            //    .HasMany(p => p.ReleaseReceivers)
            //    .WithOne(r => r.Receiver)
            //    .HasForeignKey(r => r.ReceiverId);
            //Person has many OrderIssuers

            //builder.Entity<OrderModel>()
            //    .HasOne(o => o.Issuer)
            //    .WithMany()
            //    .HasForeignKey(o => o.ReceiverId);
            //builder.Entity<Person>()
            //    .HasMany(p => p.OrderIssuers)
            //    .WithOne(r => r.Issuer)
            //    .HasForeignKey(r => r.IssuerId);



            //Person has many OrderConfirmings

            builder.Entity<OrderModel>()
                .HasOne(o => o.ConfirmedBy)
                .WithMany()
                .HasForeignKey(o => o.ConfirmedById);
            //builder.Entity<Person>()
            //    .HasMany(p => p.OrderConfirmings)
            //    .WithOne(r => r.ConfirmedBy)
            //    .HasForeignKey(r => r.ConfirmedById);

            //builder.Entity<Person>()
            //    .HasOne<ApplicationUser>(p => p.ApplicationUser)
            //    .WithOne()
            //    .HasPrincipalKey<ApplicationUser>(p => p.Id);

            builder.Entity<Device>()
                .Property(d => d.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Project>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<OrderItem>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<OrderModel>()
                .Property(o => o.Id)
                .ValueGeneratedOnAdd();

            //Virtual Order has many orders
            builder.Entity<VirtualOrder>().HasMany(v => v.Orders)
                .WithMany(o => o.VirtualOrders)
                .UsingEntity<VirtualManyToMany>(
                    v => v.HasOne(v => v.Order)
                    .WithMany().HasForeignKey(v => v.OrderId),
                    o => o.HasOne(o => o.VirtualOrder)
                    .WithMany().HasForeignKey(o => o.VirtualOrderId));

        }
    }
}
