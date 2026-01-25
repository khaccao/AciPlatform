namespace AciPlatform.Domain.Constants;

public static class ResultErrorConstants
{
    public const string USER_IS_NOT_EXIST = "User does not exist";
    public const string ERROR_PASS = "Invalid password";
    public const string LOGIN_SUCCESS = "Login success";
    public const string USER_MISS_PASSWORD = "Password is required";
    public const string USER_MISS_USERNAME = "Username is required";
    public const string USER_USNEXIST = "Username already exists";
    public const string USER_EMPTY_OR_DELETE = "User not found or deleted";
    public const string MODEL_NULL = "Model cannot be null";
    public const string MODEL_MISS = "Required fields are missing";
}

public enum ErrorEnum
{
    SUCCESS = 200,
    USER_IS_NOT_EXIST = 404,
    ERROR_PASS = 400
}

