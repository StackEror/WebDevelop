using MediatR;

namespace WebDevelopment.Application.Queries.Users.GetAllRoles;

public class GetAllRolesQuery() : IRequest<Dictionary<string, string>>;