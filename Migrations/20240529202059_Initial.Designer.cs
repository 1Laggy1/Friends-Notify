﻿// <auto-generated />
using Friends_Notify.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Friends_Notify.Migrations
{
    [DbContext(typeof(FriendsNotifyDbContext))]
    [Migration("20240529202059_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("Friends_Notify.Models.TrackUsers", b =>
                {
                    b.Property<ulong>("TrackingUserId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasIndex("TrackingUserId");

                    b.HasIndex("UserId");

                    b.ToTable("TrackUsers");
                });

            modelBuilder.Entity("Friends_Notify.Models.User", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Friends_Notify.Models.TrackUsers", b =>
                {
                    b.HasOne("Friends_Notify.Models.User", "TrackingUser")
                        .WithMany()
                        .HasForeignKey("TrackingUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Friends_Notify.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TrackingUser");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
