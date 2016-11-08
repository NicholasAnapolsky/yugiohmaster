using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Milestone_3.Models
{
    [Table("SalesLT.ProductModel")]
    public class ProductModel
    {
        public int ProductModelID { get; set; }
        [Required]
        public string Name { get; set; }
        public string CatalogDescription { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
/*
 *     [ProductModelID]     INT                                                         IDENTITY (1, 1) NOT NULL,
    [Name]               [dbo].[Name]                                                NOT NULL,
    [CatalogDescription] XML(CONTENT [SalesLT].[ProductDescriptionSchemaCollection]) NULL,
    [rowguid]            UNIQUEIDENTIFIER                                            CONSTRAINT [DF_ProductModel_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [ModifiedDate]       DATETIME   
 */
