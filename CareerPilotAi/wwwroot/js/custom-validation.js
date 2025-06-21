/**
 * Custom validation adapters for ASP.NET Core unobtrusive validation
 */

// Add validation method for allowed values
$.validator.addMethod("allowedvalues", function (value, element, params) {
    if (!value) {
        return true; // Allow empty values, use required attribute if needed
    }
    
    var allowedValues = params.split(',');
    return allowedValues.indexOf(value) !== -1;
}, "Please select a valid option from the list.");

// Add unobtrusive adapter for allowed values
$.validator.unobtrusive.adapters.addSingleVal("allowedvalues", "values"); 