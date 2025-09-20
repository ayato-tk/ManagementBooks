using FluentValidation;
using GestaoLivros.Application.Requests.Books.Validations;
using GestaoLivros.Application.Requests.Genre.Validations;
using GestaoLivros.Application.Requests.Publisher.Validations;
using GestaoLivros.Application.Requests.User.Validators;
using GestaoLivros.Application.User.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoLivros.Application.Extensions;

public static class ValidationExtensions
{
    public static void AddValidations(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<SignInUserCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<ResetPasswordCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateBookCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateBookCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<DeleteBookCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateGenreCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<DeleteGenreCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<DeletePublisherCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdatePublisherCommandValidator>();
    }
}