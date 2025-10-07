using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.Application.CustomValidationAttributes;

public class EnhancedEmailAttribute : RegularExpressionAttribute
{

 public EnhancedEmailAttribute() : base(@"^(([^<>()[\]\\.,;:\s@""]+(\.[^<>()[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$")
 {
    ErrorMessage = "Please enter a valid email address.";
 }

}