using System.Linq;
using AutoMapper;
using Application.Models.PedidoModel;
using Application.Utils;
using Domain.Entities;

namespace Application.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PedidoPostRequest, Pedido>().ReverseMap();

            CreateMap<Pedido, PedidoDetalhadoResponse>()
                .ForMember(x => x.Status, m => m.MapFrom(x => EnumUtil.GetDescriptionFromEnumValue(x.Status)))
                .ForMember(x => x.Total, m => m.MapFrom(x => x.ItensPedido.Select(x => x.ValorProduto).Sum()));

            CreateMap<ItemPedido, PedidoProdutoDetalhadoResponse>()
                .ForMember(x => x.NomeProduto, m => m.MapFrom(x => x.NomeProduto))
                .ForMember(x => x.Valor, m => m.MapFrom(x => x.ValorProduto));

            CreateMap<Pedido, PedidoDetalhadoPorSenhaResponse>()
                .ForMember(x => x.Status, m => m.MapFrom(x => EnumUtil.GetDescriptionFromEnumValue(x.Status)))
                .ForMember(x => x.Total, m => m.MapFrom(x => x.ItensPedido.Select(x => x.ValorProduto).Sum()));

            CreateMap<ItemPedido, PedidoProdutoDetalhadoPorSenhaResponse>()
                .ForMember(x => x.NomeProduto, m => m.MapFrom(x => x.NomeProduto))
                .ForMember(x => x.Valor, m => m.MapFrom(x => x.ValorProduto));
        }
    }
}
