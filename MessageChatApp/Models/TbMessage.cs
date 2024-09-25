using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MessageChatApp.Models;

[Table("tbMessages")]
public partial class TbMessage
{
    [Key]
    public int MessageId { get; set; }

    public int ConversationId { get; set; }

    public int SenderId { get; set; }

    public string? Content { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SentAt { get; set; }

    [ForeignKey("ConversationId")]
    [InverseProperty("TbMessages")]
    public virtual TbConversation Conversation { get; set; } = null!;

    [ForeignKey("SenderId")]
    [InverseProperty("TbMessages")]
    public virtual TbUser Sender { get; set; } = null!;

    [InverseProperty("Message")]
    public virtual ICollection<TbMessageStatus> TbMessageStatuses { get; set; } = new List<TbMessageStatus>();
}
