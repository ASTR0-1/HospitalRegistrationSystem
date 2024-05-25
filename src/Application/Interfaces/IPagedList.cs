using HospitalRegistrationSystem.Application.Utility.PagedData;

namespace HospitalRegistrationSystem.Application.Interfaces;

/// <summary>
///     Represents a paged list of items.
/// </summary>
public interface IPagedList
{
    /// <summary>
    ///     Gets the metadata of the paged list.
    /// </summary>
    MetaData MetaData { get; }
}
