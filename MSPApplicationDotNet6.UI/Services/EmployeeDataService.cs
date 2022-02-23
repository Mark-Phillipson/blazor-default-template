using MSPApplication.Shared;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MSPApplicationDotNet6.UI.Services
{
	public class EmployeeDataService : IEmployeeDataService
	{
		private readonly HttpClient _httpClient;

		public EmployeeDataService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<IEnumerable<Employee>> GetAllEmployees(int? jobCategoryId = null)
		{
			var url = $"api/employee/getallemployees/";
			if (jobCategoryId != null)
			{
				url = $"api/employee/getallemployees/{jobCategoryId}/";
			}
			var result = await JsonSerializer.DeserializeAsync<IEnumerable<Employee>>
				(await _httpClient.GetStreamAsync(url), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			return result;
		}

		public async Task<Employee> GetEmployeeDetails(int employeeId)
		{
			var result = await JsonSerializer.DeserializeAsync<Employee>
				(await _httpClient.GetStreamAsync($"api/employee/getemployeebyid/{employeeId}/"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

			return result;
		}

		public async Task<Employee> AddEmployee(Employee employee)
		{
			var employeeJson =
				new StringContent(JsonSerializer.Serialize(employee), Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync("api/employee", employeeJson);

			if (response.IsSuccessStatusCode)
			{
				return await JsonSerializer.DeserializeAsync<Employee>(await response.Content.ReadAsStreamAsync());
			}

			return null;
		}

		public async Task UpdateEmployee(Employee employee)
		{
			var employeeJson =
				new StringContent(JsonSerializer.Serialize(employee), Encoding.UTF8, "application/json");

			await _httpClient.PutAsync("api/employee", employeeJson);
		}

		public async Task DeleteEmployee(int employeeId)
		{
			await _httpClient.DeleteAsync($"api/employee/{employeeId}");
		}

		public async Task<PostcodeInfo> GetPostcodeData(string postcode)
		{
			try
			{
				var result = await JsonSerializer.DeserializeAsync<PostcodeInfo>
			(await _httpClient.GetStreamAsync($"http://api.postcodes.io/postcodes/{postcode.Trim().Replace(" ", "")}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
				if (result.status == 200)
				{
					return result;
				}
			}
			catch (System.Exception)
			{
				return null;
			}
			//var result = await _httpClient.GetStringAsync($"http://api.postcodes.io/postcodes/{postcode.Trim().Replace(" ", "")}");
			return null;
		}

	}
}
