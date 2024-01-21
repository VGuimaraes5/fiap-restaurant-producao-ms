﻿using System.Threading.Tasks;

namespace Application.UseCases
{
    public interface IUseCase<in TRequest>
    {
        void Execute(TRequest request);
    }

    public interface IUseCase<in TRequest, out TResponse>
    {
        TResponse Execute(TRequest request);
    }

    public interface IUseCaseAsync<in TRequest>
    {
        Task ExecuteAsync(TRequest request);
    }

    public interface IUseCaseIEnumerableAsync<TResponse>
    {
        Task<TResponse> ExecuteAsync();
    }

    public interface IUseCaseIEnumerableAsync<TRequest, TResponse>
    {
        Task<TResponse> ExecuteAsync(TRequest request);
    }

    public interface IUseCaseAsync<TRequest, TResponse>
    {
        Task<TResponse> ExecuteAsync(TRequest request);
    }
}
