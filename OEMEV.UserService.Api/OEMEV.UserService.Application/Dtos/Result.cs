namespace OEMEV.UserService.Application.Dtos
{
	public class Result<T>
	{
		public T? Data { get; set; }
		public bool Success => Error == null;
		public string? Error { get; set; }

		public static Result<T> Ok(T data) => new Result<T> { Data = data };
		public static Result<T> Fail(string error) => new Result<T> { Error = error };
	}

}
