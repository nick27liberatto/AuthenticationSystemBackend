namespace Tests
{
    using AutoMapper;
    using Domain.Interfaces;
    using FluentValidation;
    using Infrastructure.Repository;
    using MediatR;
    using Moq;

    public class Fixture
    {
        public Mock<IMapper> MapperMock { get; private set; }
        public Mock<IMediator> MediatorMock { get; private set; }
        public Mock<IValidator<object>> ValidatorMock { get; private set; }

        public Fixture()
        {
            // AutoMapper
            MapperMock = new Mock<IMapper>();
            MapperMock
                .Setup(m => m.Map<object>(It.IsAny<object>()))
                .Returns((object src) => src); // mapeia para ele mesmo no mock

            // Mediator
            MediatorMock = new Mock<IMediator>();
            MediatorMock
                .Setup(m => m.Send(It.IsAny<IRequest<object>>(), default))
                .ReturnsAsync(new object()); // sempre retorna algo

            // FluentValidation
            ValidatorMock = new Mock<IValidator<object>>();
            ValidatorMock
                .Setup(v => v.Validate(It.IsAny<object>()))
                .Returns(new FluentValidation.Results.ValidationResult());
        }
    }
}
