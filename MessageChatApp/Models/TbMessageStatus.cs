using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MessageChatApp.Models;

[PrimaryKey("MessageId", "UserId")]
[Table("tbMessageStatus")]
public partial class TbMessageStatus
{
    [Key]
    public int MessageId { get; set; }

    [Key]
    public int UserId { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? StatusAt { get; set; }

    [ForeignKey("MessageId")]
    [InverseProperty("TbMessageStatuses")]
    public virtual TbMessage Message { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("TbMessageStatuses")]
    public virtual TbUser User { get; set; } = null!;
}
