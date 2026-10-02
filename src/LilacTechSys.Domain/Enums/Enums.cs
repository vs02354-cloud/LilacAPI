namespace LilacTechSys.Domain.Enums
{
    public enum UserRole
    {
        Admin = 1,
        SuperAdmin = 2
    }

    public enum ApplicationStatus
    {
        Pending = 1,
        Reviewed = 2,
        Shortlisted = 3,
        InterviewScheduled = 4,
        Rejected = 5,
        Hired = 6
    }

    public enum QuoteStatus
    {
        New = 1,
        Reviewing = 2,
        Contacted = 3,
        Quoted = 4,
        Accepted = 5,
        Closed = 6
    }
}
