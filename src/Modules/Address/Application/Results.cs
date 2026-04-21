namespace Modules.Address.Application.Results;

public record AddressResult(
    bool Success,
    AddressDto? Address,
    string? Error
)
{
    public static AddressResult Ok(AddressDto address) => new(true, address, null);
    public static AddressResult Bad(string error) => new(false, null, error);
};

public record AddressListResult(
    bool Success,
    List<AddressDto> Addresses,
    string? Error
)
{
    public static AddressListResult Ok(List<AddressDto> addresses) => new(true, addresses, null);
    public static AddressListResult Bad(string error) => new(false, new List<AddressDto>(), error);
};

public record AddressDto(
    int Id,
    int UserId,
    string Label,
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country,
    bool IsDefault
);