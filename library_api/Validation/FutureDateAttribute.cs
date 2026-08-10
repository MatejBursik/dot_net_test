using System.ComponentModel.DataAnnotations;

namespace library_api.Validation;

public class FutureDateAttribute : ValidationAttribute {
    public override bool IsValid(object? value) {
        if (value is not DateTime date) {
            return false;
        }

        return date > DateTime.UtcNow;
    }

    public override string FormatErrorMessage(string name) {
        return $"{name} must be a future date.";
    }
}