﻿using DataAccessLayer.Models.Enums;

namespace MVC.Models.Order;

public class OrderRefundViewModel
{
    public long UserId { get; set; }
    public long Id { get; set; }
    public OrderState OrderState { get; set; }
}
