using Application.DTOs.Request;
using Domain.Enums;
namespace Application.Interfaces;
public interface IRequestService
{
   Task<RequestDto> CreateRequestAsync(CreateRequestDto createRequestDto,int companyId,int adminId);
   Task<RequestDto> GetRequestByIdAsync(int requestId);
   Task<RequestDto> UpdateRequestAsync(int requestId,UpdateRequestDto updateRequestDto);
   Task DeleteRequestAsync(int requestId);
   Task<IEnumerable<RequestDto>> GetAllAsync();
}