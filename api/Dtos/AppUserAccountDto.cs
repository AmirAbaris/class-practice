using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos;

public record RegisterDto(
    string Email,
    string Password,
    string ConfirmPassword
);

public record LoginDto(
    string Email,
    string Password
);

