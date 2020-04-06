using BackEnd.Data;
using System.Linq;

namespace BackEnd.Infrastructure
{
    public static class EntityExtensions
    {
        public static ConferenceDTO.SpeakerResponse MapSpeakerResponse(this Speaker speaker) =>
            new ConferenceDTO.SpeakerResponse
            {
                Id = speaker.Id,
                Name = speaker.Name,
                Bio = speaker.Bio,
                Website = speaker.Website,
                Sessions = speaker.SessionSpeakers?
                .Select(ss => new ConferenceDTO.Session
                {
                    Id = ss.SessionId,
                    Title = ss.Session.Title
                })
                .ToList()
            };

    }
}
