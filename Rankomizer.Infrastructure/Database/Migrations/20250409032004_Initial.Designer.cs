﻿// <auto-generated />
using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Rankomizer.Infrastructure.Database;

#nullable disable

namespace Rankomizer.Infrastructure.Database.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250409032004_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claim_value");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.HasKey("Id")
                        .HasName("pk_asp_net_role_claims");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_asp_net_role_claims_role_id");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claim_value");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_asp_net_user_claims");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_asp_net_user_claims_user_id");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("login_provider");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text")
                        .HasColumnName("provider_key");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text")
                        .HasColumnName("provider_display_name");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("LoginProvider", "ProviderKey")
                        .HasName("pk_asp_net_user_logins");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_asp_net_user_logins_user_id");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.HasKey("UserId", "RoleId")
                        .HasName("pk_asp_net_user_roles");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_asp_net_user_roles_role_id");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("login_provider");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("UserId", "LoginProvider", "Name")
                        .HasName("pk_asp_net_user_tokens");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.Collection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by_user_id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image_url");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_collections");

                    b.HasIndex("CreatedByUserId")
                        .HasDatabaseName("ix_collections_created_by_user_id");

                    b.ToTable("Collections", (string)null);
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.CollectionItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CollectionId")
                        .HasColumnType("uuid")
                        .HasColumnName("collection_id");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uuid")
                        .HasColumnName("item_id");

                    b.Property<int>("Order")
                        .HasColumnType("integer")
                        .HasColumnName("order");

                    b.HasKey("Id")
                        .HasName("pk_collection_items");

                    b.HasIndex("CollectionId")
                        .HasDatabaseName("ix_collection_items_collection_id");

                    b.HasIndex("ItemId")
                        .HasDatabaseName("ix_collection_items_item_id");

                    b.ToTable("CollectionItems", (string)null);
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.Duel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("GauntletId")
                        .HasColumnType("uuid")
                        .HasColumnName("gauntlet_id");

                    b.Property<Guid>("RosterItemAId")
                        .HasColumnType("uuid")
                        .HasColumnName("roster_item_a_id");

                    b.Property<Guid>("RosterItemBId")
                        .HasColumnType("uuid")
                        .HasColumnName("roster_item_b_id");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid?>("WinnerRosterItemId")
                        .HasColumnType("uuid")
                        .HasColumnName("winner_roster_item_id");

                    b.HasKey("Id")
                        .HasName("pk_duels");

                    b.HasIndex("GauntletId")
                        .HasDatabaseName("ix_duels_gauntlet_id");

                    b.HasIndex("RosterItemAId")
                        .HasDatabaseName("ix_duels_roster_item_a_id");

                    b.HasIndex("RosterItemBId")
                        .HasDatabaseName("ix_duels_roster_item_b_id");

                    b.HasIndex("WinnerRosterItemId")
                        .HasDatabaseName("ix_duels_winner_roster_item_id");

                    b.ToTable("Duels", (string)null);
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.Gauntlet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CollectionId")
                        .HasColumnType("uuid")
                        .HasColumnName("collection_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_gauntlets");

                    b.HasIndex("CollectionId")
                        .HasDatabaseName("ix_gauntlets_collection_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_gauntlets_user_id");

                    b.ToTable("Gauntlets", (string)null);
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.Item", b =>
                {
                    b.Property<Guid>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("item_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image_url");

                    b.Property<int>("ItemType")
                        .HasColumnType("integer")
                        .HasColumnName("item_type");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("ItemId");

                    b.ToTable("Items", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.RosterItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("GauntletId")
                        .HasColumnType("uuid")
                        .HasColumnName("gauntlet_id");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uuid")
                        .HasColumnName("item_id");

                    b.Property<int>("Losses")
                        .HasColumnType("integer")
                        .HasColumnName("losses");

                    b.Property<double>("Score")
                        .HasColumnType("double precision")
                        .HasColumnName("score");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<int>("Wins")
                        .HasColumnType("integer")
                        .HasColumnName("wins");

                    b.HasKey("Id")
                        .HasName("pk_roster_items");

                    b.HasIndex("GauntletId")
                        .HasDatabaseName("ix_roster_items_gauntlet_id");

                    b.HasIndex("ItemId")
                        .HasDatabaseName("ix_roster_items_item_id");

                    b.ToTable("RosterItems", (string)null);
                });

            modelBuilder.Entity("Rankomizer.Domain.User.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_name");

                    b.HasKey("Id")
                        .HasName("pk_asp_net_roles");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Rankomizer.Domain.User.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer")
                        .HasColumnName("access_failed_count");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("email");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("email_confirmed");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("lockout_enabled");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("lockout_end");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_email");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_user_name");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("phone_number_confirmed");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text")
                        .HasColumnName("security_stamp");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("two_factor_enabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("user_name");

                    b.HasKey("Id")
                        .HasName("pk_asp_net_users");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.Movie", b =>
                {
                    b.HasBaseType("Rankomizer.Domain.Catalog.Item");

                    b.Property<string>("ImdbId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("imdb_id");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("release_date");

                    b.Property<JsonDocument>("SourceJson")
                        .HasColumnType("jsonb")
                        .HasColumnName("source_json");

                    b.Property<int>("TmdbId")
                        .HasColumnType("integer")
                        .HasColumnName("tmdb_id");

                    b.ToTable("Movies", (string)null);
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.Painting", b =>
                {
                    b.HasBaseType("Rankomizer.Domain.Catalog.Item");

                    b.Property<string>("Artist")
                        .HasColumnType("text")
                        .HasColumnName("artist");

                    b.Property<string>("Location")
                        .HasColumnType("text")
                        .HasColumnName("location");

                    b.Property<string>("Medium")
                        .HasColumnType("text")
                        .HasColumnName("medium");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<int>("YearCreated")
                        .HasColumnType("integer")
                        .HasColumnName("year_created");

                    b.ToTable("Paintings", (string)null);
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.Song", b =>
                {
                    b.HasBaseType("Rankomizer.Domain.Catalog.Item");

                    b.Property<string>("Album")
                        .HasColumnType("text")
                        .HasColumnName("album");

                    b.Property<string>("Artist")
                        .HasColumnType("text")
                        .HasColumnName("artist");

                    b.Property<JsonDocument>("SourceJson")
                        .HasColumnType("jsonb")
                        .HasColumnName("source_json");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.ToTable("Songs", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Rankomizer.Domain.User.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_role_claims_asp_net_roles_role_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Rankomizer.Domain.User.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_claims_asp_net_users_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Rankomizer.Domain.User.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_logins_asp_net_users_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Rankomizer.Domain.User.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_roles_asp_net_roles_role_id");

                    b.HasOne("Rankomizer.Domain.User.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_roles_asp_net_users_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Rankomizer.Domain.User.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_tokens_asp_net_users_user_id");
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.Collection", b =>
                {
                    b.HasOne("Rankomizer.Domain.User.ApplicationUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_collections_asp_net_users_created_by_user_id");

                    b.Navigation("CreatedByUser");
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.CollectionItem", b =>
                {
                    b.HasOne("Rankomizer.Domain.Catalog.Collection", "Collection")
                        .WithMany("Items")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_collection_items_collections_collection_id");

                    b.HasOne("Rankomizer.Domain.Catalog.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_collection_items_items_item_id");

                    b.Navigation("Collection");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.Duel", b =>
                {
                    b.HasOne("Rankomizer.Domain.Catalog.Gauntlet", "Gauntlet")
                        .WithMany("Duels")
                        .HasForeignKey("GauntletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_duels_gauntlets_gauntlet_id");

                    b.HasOne("Rankomizer.Domain.Catalog.RosterItem", "RosterItemA")
                        .WithMany()
                        .HasForeignKey("RosterItemAId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_duels_roster_items_roster_item_a_id");

                    b.HasOne("Rankomizer.Domain.Catalog.RosterItem", "RosterItemB")
                        .WithMany()
                        .HasForeignKey("RosterItemBId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_duels_roster_items_roster_item_b_id");

                    b.HasOne("Rankomizer.Domain.Catalog.RosterItem", "WinnerRosterItem")
                        .WithMany()
                        .HasForeignKey("WinnerRosterItemId")
                        .HasConstraintName("fk_duels_roster_items_winner_roster_item_id");

                    b.Navigation("Gauntlet");

                    b.Navigation("RosterItemA");

                    b.Navigation("RosterItemB");

                    b.Navigation("WinnerRosterItem");
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.Gauntlet", b =>
                {
                    b.HasOne("Rankomizer.Domain.Catalog.Collection", "Collection")
                        .WithMany()
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_gauntlets_collections_collection_id");

                    b.HasOne("Rankomizer.Domain.User.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_gauntlets_asp_net_users_user_id");

                    b.Navigation("Collection");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.RosterItem", b =>
                {
                    b.HasOne("Rankomizer.Domain.Catalog.Gauntlet", "Gauntlet")
                        .WithMany("RosterItems")
                        .HasForeignKey("GauntletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_roster_items_gauntlets_gauntlet_id");

                    b.HasOne("Rankomizer.Domain.Catalog.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_roster_items_items_item_id");

                    b.Navigation("Gauntlet");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.Movie", b =>
                {
                    b.HasOne("Rankomizer.Domain.Catalog.Item", null)
                        .WithOne()
                        .HasForeignKey("Rankomizer.Domain.Catalog.Movie", "ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_movies_items_item_id");
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.Painting", b =>
                {
                    b.HasOne("Rankomizer.Domain.Catalog.Item", null)
                        .WithOne()
                        .HasForeignKey("Rankomizer.Domain.Catalog.Painting", "ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_paintings_items_item_id");
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.Song", b =>
                {
                    b.HasOne("Rankomizer.Domain.Catalog.Item", null)
                        .WithOne()
                        .HasForeignKey("Rankomizer.Domain.Catalog.Song", "ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_songs_items_item_id");
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.Collection", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Rankomizer.Domain.Catalog.Gauntlet", b =>
                {
                    b.Navigation("Duels");

                    b.Navigation("RosterItems");
                });
#pragma warning restore 612, 618
        }
    }
}
