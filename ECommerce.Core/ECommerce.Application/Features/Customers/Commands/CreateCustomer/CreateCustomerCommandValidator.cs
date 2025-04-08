using AutoMapper;
using ECommerce.Application.Contract;
using ECommerce.Domain.Entities;
using FluentValidation;

namespace ECommerce.Application.Features;

public class CreateCustomerCommandValidator:AbstractValidator<CreateCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    public CreateCustomerCommandValidator(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;

        RuleFor(cu => cu.Email)
         .NotNull().WithMessage("{PropertyName} Is Required")
         .NotEmpty()
         .MaximumLength(50).WithMessage("{PropertyName} Must Not exceed 50 Characters");

        RuleFor(cu => cu.Name)
         .NotNull().WithMessage("{PropertyName} Is Required")
         .NotEmpty()
         .MaximumLength(100).WithMessage("{PropertyName} Must Not exceed 100 Characters");

        RuleFor(cu => cu)
            .MustAsync(EmailIsUnique)
            .WithMessage("the Event Name And Date Is Alrewady Exist");
        _mapper = mapper;
    }

    private async Task<bool> EmailIsUnique(CreateCustomerCommand command, CancellationToken token)
    {
        return await _customerRepository.EmailIsAlreadyExist(_mapper.Map<Customer>(command));
    }
}
