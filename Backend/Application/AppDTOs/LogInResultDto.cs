using AppDTOs.Enumerations;

namespace AppDTOs;

public class LogInResultDto
{
    public LogInResultDto(LogInResult result)
    {
        Result = result;
    }

    public LogInResultDto(UserDto user)
    {
        Result = LogInResult.Success;
        User = user;
    }

    public LogInResult Result { get; set; }
    public UserDto? User { get; set; }
}
