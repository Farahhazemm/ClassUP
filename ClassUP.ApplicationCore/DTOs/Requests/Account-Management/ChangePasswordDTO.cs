using System.ComponentModel.DataAnnotations;

public class ChangePasswordDTO : IValidatableObject
{
    [Required(ErrorMessage = "Current password is required.")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = null!;

    [Required(ErrorMessage = "New password is required.")]
    [StringLength(100, MinimumLength = 8,
        ErrorMessage = "New password must be between 8 and 100 characters.")]
    [DataType(DataType.Password)]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$",
        ErrorMessage = "Password must contain uppercase, lowercase, number, and special character."
    )]
    public string NewPassword { get; set; } = null!;

     // i should Ensure oldpass != newPass
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CurrentPassword != null && NewPassword != null && CurrentPassword == NewPassword)
        {
            yield return new ValidationResult(
                "New password must be different from the current password.",
                new[] { nameof(NewPassword) });
        }
    }
}