using System.ComponentModel;

namespace Domain.Enums
{
    public enum Status : short
    {
        [Description("Em Analise")]
        EmAnalise,

        [Description("Reprovado")]
        Reprovado,

        [Description("Recebido")]
        Pendente,

        [Description("Em preparação")]
        EmPreparo,

        [Description("Pronto")]
        Pronto,

        [Description("Entregue")]
        Entregue,
    }
}
