using APIStickerAlbum.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace APIStickerAlbum.DTOs.Mappings;

public static class AlbumDTOMappingExtensions
{
    public static Album? ToAlbum(this AlbumCreateDTO albumCreateDTO)
    {
        if (albumCreateDTO is null)
            return null;

        return new Album
        {
            Title = albumCreateDTO.Title!,
            Description = albumCreateDTO.Description,
            Community = albumCreateDTO.Community
        };
    }

    public static Album? ToAlbum(this AlbumUpdateDTO albumUpdateDTO)
    {
        if (albumUpdateDTO is null)
            return null;

        return new Album
        {
            Id = albumUpdateDTO.Id,
            Title = albumUpdateDTO.Title!,
            Description = albumUpdateDTO.Description,
            Community = albumUpdateDTO.Community,
            Shared = albumUpdateDTO.Shared,
            Blocked = albumUpdateDTO.Blocked,
        };
    }

    public static AlbumDetailsDTO? ToAlbumDetailsDTO(this Album album)
    {
        if (album is null)
            return null;

        return new AlbumDetailsDTO
        {
            Id = album.Id,
            ImageUrl = album.ImageUrl,
            Title = album.Title,
            Description = album.Description,
            Community = album.Community,
            Shared = album.Shared,
            Blocked = album.Blocked,
            CreatedAt = album.CreatedAt
        };
    }

    public static IEnumerable<AlbumDetailsDTO> ToAlbumDetailsDTOList(this IEnumerable<Album> albums)
    {
        if (albums is null || !albums.Any())
            return new List<AlbumDetailsDTO>();

        return albums.Select(album => new AlbumDetailsDTO
        {
            Id = album.Id,
            ImageUrl = album.ImageUrl,
            Title = album.Title,
            Description = album.Description,
            Community = album.Community,
            Shared = album.Shared,
            Blocked = album.Blocked,
            CreatedAt = album.CreatedAt
        }).ToList();
    }
}
