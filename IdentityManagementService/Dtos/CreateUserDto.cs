﻿namespace IdentityManagementService.Dtos;

public record CreateUserDto(
    string Login, 
    string Password, 
    string FirstName, 
    string LastName, 
    string? MiddleName,
    string Email, 
    Guid RoleId);
