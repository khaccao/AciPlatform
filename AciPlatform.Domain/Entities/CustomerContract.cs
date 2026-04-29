using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities
{
    [Table("CustomerContracts")]
    public class CustomerContract
    {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [MaxLength(100)]
        public string ContractCode { get; set; }

        [MaxLength(255)]
        public string Title { get; set; }

        public DateTime? SignDate { get; set; }
        public DateTime? ExpireDate { get; set; }

        public double ContractValue { get; set; } = 0;

        [MaxLength(2000)]
        public string Notes { get; set; }

        public bool IsActive { get; set; } = true;
        
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
