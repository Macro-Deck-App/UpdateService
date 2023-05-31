namespace MacroDeck.UpdateService.Core.DataAccess.Entities;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedTimestamp { get; set; } = DateTime.Now;
}