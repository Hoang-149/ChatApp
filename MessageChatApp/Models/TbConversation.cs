using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MessageChatApp.Models;

[Table("tbConversations")]
public partial class TbConversation
{
    [Key]
    public int ConversationId { get; set; }

    [StringLength(256)]
    public string? ConversationName { get; set; }

    public bool IsGroup { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Conversation")]
    public virtual ICollection<TbConversationMember> TbConversationMembers { get; set; } = new List<TbConversationMember>();

    [InverseProperty("Conversation")]
    public virtual ICollection<TbMessage> TbMessages { get; set; } = new List<TbMessage>();
}