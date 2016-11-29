using FluentValidation;
using FluentValidation.Attributes;
using Milestone_3.Models;
using MileStone2A.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MileStone2A.Models
{
    [Table("SalesLT.Product")]
    [Validator(typeof(ProductsValidator))]
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Remote("UniqueProductName", "Manager", ErrorMessage = "This Product Name already exists, choose another name.")]
        public string Name { get; set; }

        [Remote("UniqueProductNumber", "Manager", ErrorMessage = "This Product Number already exists, choose another name.")]
        [Display(Name = "Product Number")]
        public string ProductNumber { get; set; }

        public string Color { get; set; }

        [Display(Name = "Standard Cost")]
        [Range(0, (double)Decimal.MaxValue, ErrorMessage = "The Standard Cost cannot be negative.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal StandardCost { get; set; }

        [Display(Name = "List Price")]
        [Range(0, (double)Decimal.MaxValue, ErrorMessage = "The List Price cannot be negative.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal ListPrice { get; set; }

        [StringLength(5, MinimumLength = 1)]
        public string Size { get; set; }

        [Range(0.1, (double)Decimal.MaxValue, ErrorMessage = "The Weight must be greater than 0.1")]
        public decimal? Weight { get; set; }

        [Display(Name = "Product Category")]
        [ForeignKey("ProductCategory")]
        public int ProductCategoryID { get; set; }

        [Display(Name = "Product Model")]
        [ForeignKey("ProductModel")]
        public int ProductModelID { get; set; }

        [Display(Name = "Sell Start Date")]
        [DataType(DataType.Date, ErrorMessage = "The Sell Start Date must be a proper date of mm-dd-yyyy format.")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}")]
        public DateTime SellStartDate { get; set; }

        [Display(Name = "Sell End Date")]
        [DataType(DataType.Date, ErrorMessage = "The Sell End Date must be a proper date of mm-dd-yyyy format.")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}")]
        public DateTime? SellEndDate { get; set; }

        [Display(Name = "Discontinued Date")]
        [DataType(DataType.Date, ErrorMessage = "The Discontinued Date must be a proper date of mm-dd-yyyy format.")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}")]
        public DateTime? DiscontinuedDate { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Thumbnail Photo")]
        public byte[] ThumbNailPhoto { get; set; }

        [Display(Name = "Photo")]
        public string ThumbnailPhotoFileName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid rowguid { get; set; }

        [Display(Name = "Modified Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}")]
        public DateTime ModifiedDate { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }

        public virtual ProductModel ProductModel { get; set; }
    }
}

public class ProductsValidator : AbstractValidator<Product>
{
    public ProductsValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("The Name must be included.");
        RuleFor(x => x.ProductNumber).NotEmpty().WithMessage("The Product Number must be included.");
        RuleFor(x => x.Color).NotEmpty().WithMessage("The Color must be included.");
        RuleFor(x => x.Size).NotEmpty().WithMessage("The Size must be included.");
        RuleFor(x => x.StandardCost).NotEmpty().WithMessage("The Standard Cost must be included.");
        RuleFor(x => x.ListPrice).NotEmpty().WithMessage("The List Price must be included.")
            .GreaterThan(x => x.StandardCost).WithMessage("The List Price must be greater than the Standard Cost.");
        RuleFor(x => x.Weight).NotEmpty().WithMessage("The Weight must be included.");
        RuleFor(x => x.ProductCategoryID).NotEmpty().WithMessage("The Product Category must be included.");
        RuleFor(x => x.ProductModelID).NotEmpty().WithMessage("The Product Model must be included.");
        RuleFor(x => x.SellStartDate).NotEmpty().WithMessage("The Sell Start Date must be included.");
    }
}

/*
 * 
 *  [ProductID]              INT              IDENTITY (1, 1) NOT NULL,
    [Name]                   [dbo].[Name]     NOT NULL,
    [ProductNumber]          NVARCHAR (25)    NOT NULL,
    [Color]                  NVARCHAR (15)    NULL,
    [StandardCost]           MONEY            NOT NULL,
    [ListPrice]              MONEY            NOT NULL,
    [Size]                   NVARCHAR (5)     NULL,
    [Weight]                 DECIMAL (8, 2)   NULL,
    [ProductCategoryID]      INT              NULL,
    [ProductModelID]         INT              NULL,
    [SellStartDate]          DATETIME         NOT NULL,
    [SellEndDate]            DATETIME         NULL,
    [DiscontinuedDate]       DATETIME         NULL,
    [ThumbNailPhoto]         VARBINARY (MAX)  NULL,
    [ThumbnailPhotoFileName] NVARCHAR (50)    NULL,
    [rowguid]                UNIQUEIDENTIFIER CONSTRAINT [DF_Product_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [ModifiedDate]           DATETIME         CONSTRAINT [DF_Product_ModifiedDate] DEFAULT (getdate()) NOT NULL,
 */
