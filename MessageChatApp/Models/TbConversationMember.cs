using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MessageChatApp.Models;

[PrimaryKey("ConversationId", "UserId")]
[Table("tbConversationMembers")]
public partial class TbConversationMember
{
    [Key]
    public int ConversationId { get; set; }

    [Key]
    public int UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? JoinedAt { get; set; }

    [ForeignKey("ConversationId")]
    [InverseProperty("TbConversationMembers")]
    public virtual TbConversation Conversation { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("TbConversationMembers")]
    public virtual TbUser User { get; set; } = null!;
}
