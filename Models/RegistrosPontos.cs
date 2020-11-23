using System;
using System.ComponentModel.DataAnnotations;

namespace ProvaTecnica.Models
{
    public class RegistrosPontos
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        public string NomeUsuario { get; set; }

        public string Tipo { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan Hora { get; set; }
    }
}
