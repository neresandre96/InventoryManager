﻿// <auto-generated />
using System;
using InventoryManager.API.Data.App.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InventoryManager.API.Data.Migrations.App
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241126012920_SetNameUnique")]
    partial class SetNameUnique
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("InventoryManager.API.Data.App.Models.Entities.Ingredient", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("ActualWeight")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("MeasureUnit")
                        .HasColumnType("INTEGER");

                    b.Property<double>("MinWeight")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("TenantId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("InventoryManager.API.Data.App.Models.Entities.Movement", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("IngredientId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsEntry")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<double>("Quantity")
                        .HasColumnType("REAL");

                    b.Property<int>("TenantId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("IngredientId");

                    b.ToTable("Movements");
                });

            modelBuilder.Entity("InventoryManager.API.Data.App.Models.Entities.Movement", b =>
                {
                    b.HasOne("InventoryManager.API.Data.App.Models.Entities.Ingredient", "Ingredient")
                        .WithMany()
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ingredient");
                });
#pragma warning restore 612, 618
        }
    }
}