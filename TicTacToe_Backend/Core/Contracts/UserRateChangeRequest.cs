﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts;

public record UserRateChangeRequest
{
    public string UserId { get; set; }

    public int ChangeDelta { get; set; }
}
