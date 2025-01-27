namespace API.DTOs.Household;

public class HouseholdDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreateDate { get; set; }
    public MemberDto Owner { get; set; } = null!;
    public IEnumerable<MemberDto> Members { get; set; } = [];

}
