﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sharebook.Storage.API.Data;

namespace Sharebook.Storage.API.Migrations
{
    [DbContext(typeof(ScienceDbContext))]
    [Migration("20201126112939_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Contributors")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CoordinatesId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedAt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EntitiesId")
                        .HasColumnType("int");

                    b.Property<int>("FavoriteCount")
                        .HasColumnType("int");

                    b.Property<bool>("Favorited")
                        .HasColumnType("bit");

                    b.Property<string>("FullText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GeoId")
                        .HasColumnType("int");

                    b.Property<string>("IdStr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InReplyToScreenName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InReplyToStatusId")
                        .HasColumnType("int");

                    b.Property<string>("InReplyToStatusIdStr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InReplyToUserId")
                        .HasColumnType("int");

                    b.Property<string>("InReplyToUserIdStr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsQuoteStatus")
                        .HasColumnType("bit");

                    b.Property<string>("Lang")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PlaceId")
                        .HasColumnType("int");

                    b.Property<bool>("PossiblySensitive")
                        .HasColumnType("bit");

                    b.Property<bool>("PossiblySensitiveEditable")
                        .HasColumnType("bit");

                    b.Property<int>("QuoteCount")
                        .HasColumnType("int");

                    b.Property<int>("ReplyCount")
                        .HasColumnType("int");

                    b.Property<int>("RetweetCount")
                        .HasColumnType("int");

                    b.Property<bool>("Retweeted")
                        .HasColumnType("bit");

                    b.Property<string>("Source")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Suffix")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SupplementalLanguage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Truncated")
                        .HasColumnType("bit");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CoordinatesId");

                    b.HasIndex("EntitiesId");

                    b.HasIndex("GeoId");

                    b.HasIndex("PlaceId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.Properties.Coordinates", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("GeoId")
                        .HasColumnType("int");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("GeoId");

                    b.ToTable("Coordinates");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.Properties.Geo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Geo");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.Properties.Place", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("BoundingBoxId")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountryCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GeometryId")
                        .HasColumnType("int");

                    b.Property<string>("IdStr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PlaceId")
                        .HasColumnType("int");

                    b.Property<int>("PlaceType")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BoundingBoxId");

                    b.HasIndex("GeometryId");

                    b.HasIndex("PlaceId");

                    b.ToTable("Place");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.TwitterEntities.HashtagEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TweetEntitiesId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TweetEntitiesId");

                    b.ToTable("Hashtags");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.TwitterEntities.MediaEntity", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("DisplayURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExpandedURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdStr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MediaType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MediaURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MediaURLHttps")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TweetEntitiesId")
                        .HasColumnType("int");

                    b.Property<string>("URL")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TweetEntitiesId");

                    b.ToTable("Medias");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.TwitterEntities.SymbolEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TweetEntitiesId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TweetEntitiesId");

                    b.ToTable("SymbolEntity");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.TwitterEntities.TweetEntities", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.HasKey("Id");

                    b.ToTable("TweetEntities");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.TwitterEntities.UrlEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("DisplayedURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExpandedURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TweetEntitiesId")
                        .HasColumnType("int");

                    b.Property<string>("URL")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TweetEntitiesId");

                    b.ToTable("UrlEntity");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.TwitterEntities.UserMentionEntity", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("IdStr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ScreenName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TweetEntitiesId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TweetEntitiesId");

                    b.ToTable("UserMentionEntity");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.Post", b =>
                {
                    b.HasOne("Sharebook.Storage.API.Data.Models.Properties.Coordinates", "Coordinates")
                        .WithMany()
                        .HasForeignKey("CoordinatesId");

                    b.HasOne("Sharebook.Storage.API.Data.Models.TwitterEntities.TweetEntities", "Entities")
                        .WithMany()
                        .HasForeignKey("EntitiesId");

                    b.HasOne("Sharebook.Storage.API.Data.Models.Properties.Geo", "Geo")
                        .WithMany()
                        .HasForeignKey("GeoId");

                    b.HasOne("Sharebook.Storage.API.Data.Models.Properties.Place", "Place")
                        .WithMany()
                        .HasForeignKey("PlaceId");

                    b.Navigation("Coordinates");

                    b.Navigation("Entities");

                    b.Navigation("Geo");

                    b.Navigation("Place");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.Properties.Coordinates", b =>
                {
                    b.HasOne("Sharebook.Storage.API.Data.Models.Properties.Geo", null)
                        .WithMany("Coordinates")
                        .HasForeignKey("GeoId");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.Properties.Place", b =>
                {
                    b.HasOne("Sharebook.Storage.API.Data.Models.Properties.Geo", "BoundingBox")
                        .WithMany()
                        .HasForeignKey("BoundingBoxId");

                    b.HasOne("Sharebook.Storage.API.Data.Models.Properties.Geo", "Geometry")
                        .WithMany()
                        .HasForeignKey("GeometryId");

                    b.HasOne("Sharebook.Storage.API.Data.Models.Properties.Place", null)
                        .WithMany("ContainedWithin")
                        .HasForeignKey("PlaceId");

                    b.Navigation("BoundingBox");

                    b.Navigation("Geometry");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.TwitterEntities.HashtagEntity", b =>
                {
                    b.HasOne("Sharebook.Storage.API.Data.Models.TwitterEntities.TweetEntities", null)
                        .WithMany("Hashtags")
                        .HasForeignKey("TweetEntitiesId");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.TwitterEntities.MediaEntity", b =>
                {
                    b.HasOne("Sharebook.Storage.API.Data.Models.TwitterEntities.TweetEntities", null)
                        .WithMany("Medias")
                        .HasForeignKey("TweetEntitiesId");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.TwitterEntities.SymbolEntity", b =>
                {
                    b.HasOne("Sharebook.Storage.API.Data.Models.TwitterEntities.TweetEntities", null)
                        .WithMany("Symbols")
                        .HasForeignKey("TweetEntitiesId");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.TwitterEntities.UrlEntity", b =>
                {
                    b.HasOne("Sharebook.Storage.API.Data.Models.TwitterEntities.TweetEntities", null)
                        .WithMany("Urls")
                        .HasForeignKey("TweetEntitiesId");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.TwitterEntities.UserMentionEntity", b =>
                {
                    b.HasOne("Sharebook.Storage.API.Data.Models.TwitterEntities.TweetEntities", null)
                        .WithMany("UserMentions")
                        .HasForeignKey("TweetEntitiesId");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.Properties.Geo", b =>
                {
                    b.Navigation("Coordinates");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.Properties.Place", b =>
                {
                    b.Navigation("ContainedWithin");
                });

            modelBuilder.Entity("Sharebook.Storage.API.Data.Models.TwitterEntities.TweetEntities", b =>
                {
                    b.Navigation("Hashtags");

                    b.Navigation("Medias");

                    b.Navigation("Symbols");

                    b.Navigation("Urls");

                    b.Navigation("UserMentions");
                });
#pragma warning restore 612, 618
        }
    }
}
