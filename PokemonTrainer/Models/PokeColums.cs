using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonTrainer.Models;
using Azure;
using Azure.Data.Tables;

public class PokeColum : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public int Id { get; set; }
    public string Name { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public string Type1 { get; set; }
    public string? Type2 { get; set; }

}