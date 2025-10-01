namespace Domain;

public class Fee
{
  public int Id { get; set; }
  public required int Amount { get; set; }
  public required int Threshold { get; set; }
  public int FeeGroupId { get; set; }
}
