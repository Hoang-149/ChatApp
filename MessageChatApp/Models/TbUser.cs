using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MessageChatApp.Models;

[Table("tbUsers")]
[Index("Email", Name = "UQ__tbUsers__A9D105349499971D", IsUnique = true)]
public partial class TbUser
{
    [Key]
    public int UserId { get; set; }

    [StringLength(100)]
    public string UserName { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(256)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(256)]
    public string? ProfilePicture { get; set; }

    [StringLength(256)]
    public string? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<TbConversationMember> TbConversationMembers { get; set; } = new List<TbConversationMember>();

    [InverseProperty("User")]
    public virtual ICollection<TbMessageStatus> TbMessageStatuses { get; set; } = new List<TbMessageStatus>();

    [InverseProperty("Sender")]
    public virtual ICollection<TbMessage> TbMessages { get; set; } = new List<TbMessage>();
}
