﻿using Microsoft.EntityFrameworkCore;

namespace Migrations;

public class TicTacToeContext : DbContext
{
    public TicTacToeContext(DbContextOptions<TicTacToeContext> options): base(options) {}
}