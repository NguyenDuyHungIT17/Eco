using Eco.Domain.Common;
using Eco.Domain.Enum;

namespace Eco.Domain.Entities.Identities;

public class UserProfile : BaseEntity
{
    public Guid UserId { get; set; }

    public string DisplayName { get; set; } = default!;

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Avatar { get; set; }

    public Gender Gender { get; set; }

    public DateOnly Birthday { get; set; }

    public string Country { get; set; }

    public string Timezone { get; set; }

    public string Language { get; set; }

    public string Bio { get; set; }
}