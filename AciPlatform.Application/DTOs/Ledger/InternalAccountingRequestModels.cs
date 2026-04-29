using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AciPlatform.Application.DTOs.Ledger;

public class PaymentVoucherRequestModel
{
    [Required]
    public double Amount { get; set; }

    [Required]
    [MaxLength(255)]
    public string ReceiverName { get; set; } // Người nhận tiền

    [MaxLength(500)]
    public string Reason { get; set; } // Lý do chi

    [Required]
    public string DebitAccount { get; set; } // VD: 331 (Trả nợ nhà cung cấp) hoặc 642 (Chi phí)

    [Required]
    public string CreditAccount { get; set; } // VD: 111 (Tiền mặt) hoặc 112 (Chuyển khoản)

    public int? SupplierId { get; set; } // Nếu chi trả cho NCC cụ thể

    public DateTime VoucherDate { get; set; } = DateTime.Now;
    
    public int IsInternal { get; set; } = 1; // Sổ thuế hay nội bộ
}

public class ApproveVoucherRequestModel
{
    [Required]
    public long LedgerId { get; set; } // ID của bút toán nháp

    public bool IsApproved { get; set; } = true;

    [MaxLength(500)]
    public string Note { get; set; } // Ghi chú duyệt/từ chối
}

public class WarehouseReceiptRequestModel
{
    [Required]
    [MaxLength(50)]
    public string ReceiptNumber { get; set; } // Mã phiếu nhập kho (VD: NK-0001)

    [Required]
    public int SupplierId { get; set; } // Nhà cung cấp

    public string WarehouseCode { get; set; } // Mã kho (nếu có chia nhiều kho)

    public DateTime ReceiptDate { get; set; } = DateTime.Now;

    [MaxLength(500)]
    public string Note { get; set; } // Ghi chú nhập kho

    [Required]
    public List<WarehouseReceiptItemModel> Items { get; set; } = new();

    public int IsInternal { get; set; } = 1;
}

public class WarehouseReceiptItemModel
{
    [Required]
    public int GoodsId { get; set; } // ID hàng hóa vật tư

    [Required]
    public double Quantity { get; set; }

    [Required]
    public double UnitPrice { get; set; } // Đơn giá nhập

    // Tài khoản hạch toán sẽ tự nội suy: Nợ 152/156, Có 331
}
