﻿namespace BookHubWebAPI.Api.WishList.Create;

public class CreateWishListDto
{
    public long UserId { get; set; }
    public string? Description { get; set; }
}
