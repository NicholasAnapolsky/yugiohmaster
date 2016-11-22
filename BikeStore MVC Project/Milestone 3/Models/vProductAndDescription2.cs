using Milestone_3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace MileStone2A.Models
{
    [Table("SalesLT.vProductAndDescription2")]
    public class ProductAndDescription2
    {
        [Key]
        public int ProductModelID { get; set; }
        public string ProductModel { get; set; }
        public string Culture { get; set; }
        public string Description { get; set; }
        public int ProductCategoryID { get; set; }

        public virtual ProductModel ProductModelTable { get; set; }
    }
/*
    p.[ProductModelID] 
--    ,p.[Name] 
    ,pm.[Name] AS[ProductModel]
    , pmx.[Culture] 
    ,pd.[Description] ,
p.ProductCategoryID
FROM[SalesLT].[Product]
    p
   INNER JOIN[SalesLT].[ProductModel]
    pm
  ON p.[ProductModelID] = pm.[ProductModelID]
  INNER JOIN[SalesLT].[ProductModelProductDescription]
    pmx
 ON pm.[ProductModelID] = pmx.[ProductModelID]
 INNER JOIN[SalesLT].[ProductDescription]
    pd
ON pmx.[ProductDescriptionID] = pd.[ProductDescriptionID]
;
*/
}
