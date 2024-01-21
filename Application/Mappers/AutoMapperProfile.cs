﻿using System.Linq;
using AutoMapper;
using Application.Models.CategoriaModel;
using Application.Models.ClienteModel;
using Application.Models.PedidoModel;
using Application.Models.ProdutoModel;
using Application.Utils;
using Domain.Entities;

namespace Application.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ClienteResponse, Cliente>().ReverseMap();
            CreateMap<ClientePostRequest, Cliente>().ReverseMap();

            CreateMap<CategoriaResponse, Categoria>().ReverseMap();

            CreateMap<ProdutoResponse, Produto>().ReverseMap();
            CreateMap<ProdutoPostRequest, Produto>().ReverseMap();
            CreateMap<ProdutoPutRequest, Produto>().ReverseMap();

            CreateMap<PedidoPostRequest, Pedido>().ReverseMap();
            CreateMap<PedidoResponse, Pedido>().ReverseMap();

            CreateMap<Pedido, PedidoDetalhadoResponse>()
                .ForMember(x => x.Status, m => m.MapFrom(x => EnumUtil.GetDescriptionFromEnumValue(x.Status)))
                .ForMember(x => x.Total, m => m.MapFrom(x => x.ItensPedido.Select(x => x.Produto.Valor).Sum()));

            CreateMap<ItemPedido, PedidoProdutoDetalhadoResponse>()
                .ForMember(x => x.NomeProduto, m => m.MapFrom(x => x.Produto.Nome))
                .ForMember(x => x.Valor, m => m.MapFrom(x => x.Produto.Valor))
                .ForMember(x => x.NomeCategoria, m => m.MapFrom(x => x.Produto.Categoria.Nome));

            CreateMap<Pedido, PedidoDetalhadoPorSenhaResponse>()
                .ForMember(x => x.Status, m => m.MapFrom(x => EnumUtil.GetDescriptionFromEnumValue(x.Status)))
                .ForMember(x => x.Total, m => m.MapFrom(x => x.ItensPedido.Select(x => x.Produto.Valor).Sum()))
                .ForMember(x => x.StatusPagamento, m => m.MapFrom(x => EnumUtil.GetDescriptionFromEnumValue(x.Pagamento.Status)));

            CreateMap<ItemPedido, PedidoProdutoDetalhadoPorSenhaResponse>()
                .ForMember(x => x.NomeProduto, m => m.MapFrom(x => x.Produto.Nome))
                .ForMember(x => x.Valor, m => m.MapFrom(x => x.Produto.Valor))
                .ForMember(x => x.NomeCategoria, m => m.MapFrom(x => x.Produto.Categoria.Nome));

            CreateMap<Pedido, HistoricoClienteResponse>()
                .ForMember(x => x.Total, m => m.MapFrom(x => x.ItensPedido.Select(x => x.Produto.Valor).Sum()));

            CreateMap<ItemPedido, HistoricoClienteProdutoUseCaseResponse>()
                .ForMember(x => x.NomeProduto, m => m.MapFrom(x => x.Produto.Nome))
                .ForMember(x => x.Valor, m => m.MapFrom(x => x.Produto.Valor))
                .ForMember(x => x.NomeCategoria, m => m.MapFrom(x => x.Produto.Categoria.Nome));
        }
    }
}
