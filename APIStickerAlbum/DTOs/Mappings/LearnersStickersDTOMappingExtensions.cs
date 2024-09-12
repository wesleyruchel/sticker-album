using APIStickerAlbum.Models;

namespace APIStickerAlbum.DTOs.Mappings;

public static class LearnersStickersDTOMappingExtensions
{
    public static IEnumerable<LearnersStickersDTO>? ToLearnersStickersDTOList(this IEnumerable<LearnersSticker> learnersStickers)
    {
        if (learnersStickers is null || !learnersStickers.Any())
            return new List<LearnersStickersDTO>();

        return learnersStickers.Select(learnersStickers => new LearnersStickersDTO
        {
            StickerId = learnersStickers.StickerId,
            Status = learnersStickers.Status,
            ImageUrl = learnersStickers?.ImageUrl,
        });

    }
}
